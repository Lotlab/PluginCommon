using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Lotlab.PluginCommon.FFXIV.Parser
{
    public class NetworkParser
    {
        Dictionary<string, Tuple<Type, Type>> TypeDict = new Dictionary<string, Tuple<Type, Type>>();
        Dictionary<UInt16, string> OpcodeDict = new Dictionary<ushort, string>();

        /// <summary>
        /// Create instance of network parser
        /// </summary>
        public NetworkParser(bool lookupPacket = true) 
        {
            if (lookupPacket)
            {
                LookupPacket();
            }
        }

        /// <summary>
        /// Add packet type for parse
        /// </summary>
        /// <typeparam name="T">IPCPacketBase</typeparam>
        /// <typeparam name="T2">Inner struct</typeparam>
        public void AddType<T, T2>() where T : IPCPacketBase<T2>, new() where T2 : struct
        {
            var t1 = typeof(T);
            var t2 = typeof(T2);

            AddType(t1, t2);
        }

        /// <summary>
        /// Add packet type for parse and set it's opcode
        /// </summary>
        /// <typeparam name="T">IPCPacketBase</typeparam>
        /// <typeparam name="T2">Inner struct</typeparam>
        public void AddType<T, T2>(UInt16 opcode) where T : IPCPacketBase<T2>, new() where T2 : struct
        {
            AddType<T, T2>();
            SetOpcode<T, T2>(opcode);
        }

        /// <summary>
        /// Set opcode for packet
        /// </summary>
        /// <typeparam name="T">IPCPacketBase</typeparam>
        /// <typeparam name="T2">Inner struct</typeparam>
        /// <param name="opcode"></param>
        public void SetOpcode<T, T2>(UInt16 opcode) where T : IPCPacketBase<T2>, new() where T2 : struct
        {
            SetOpcode(typeof(T).Name, opcode);
        }

        /// <summary>
        /// Set opcode for packet
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="opcode"></param>
        public void SetOpcode(string typeName, UInt16 opcode)
        {
            OpcodeDict[opcode] = typeName;
        }

        /// <summary>
        /// Set opcode for packets
        /// </summary>
        /// <param name="opcodes"></param>
        public void SetOpcodes(IDictionary<UInt16, string> opcodes)
        {
            foreach (var item in opcodes)
            {
                OpcodeDict[item.Key] = item.Value;
            }
        }

        void AddType(Type t1, Type t2)
        {
            TypeDict[t1.Name] = new Tuple<Type, Type>(t1, t2);
        }

        /// <summary>
        /// Lookup all packet definitions in all assembly
        /// </summary>
        public void LookupPacket()
        {
            var baseType = typeof(IPCPacketBase<>);

            var asm = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in asm)
            {
                // simple filter
                if (assembly.FullName.StartsWith("System") || assembly.FullName.StartsWith("Microsoft"))
                    continue;

                // lookup all types
                var types = assembly.GetTypes();
                foreach (var item in types)
                {
                    if (!item.IsAbstract && item.IsAssignableToGenericType(baseType))
                    {
                        var args = item.GetGenericBaseTypeOf(baseType).GetGenericArguments();
                        if (args.Length != 1)
                            continue;

                        AddType(item, args[0]);
                    }
                }
            }
        }

        /// <summary>
        /// Parse network packet as IPC packet
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IPCPacketBase ParsePacket(byte[] array)
        {
            // Parse segment
            var SegHeader = ByteArrayToStructure<SegmentHeader>(array);
            if (SegHeader.type != SegmentType.IPC) return null;

            // Parse IPC
            var IPCHeader = ByteArrayToStructure<IPCHeader>(array);
            if (IPCHeader.reserved != 0x0014) return null;
            
            // Get packet type
            var opcode = IPCHeader.type;
            if (!OpcodeDict.ContainsKey(opcode)) return null;

            var packetName = OpcodeDict[opcode];
            if (!TypeDict.ContainsKey(packetName)) return null;

            // Convert data
            var type = TypeDict[packetName];
            var data = ByteArrayToStructure(type.Item2, array);

            // Construct target object
            var constr = type.Item1.GetConstructor(new Type[1] { type.Item2 });
            if (constr != null)
                return constr.Invoke(new object[] { data }) as IPCPacketBase;

            constr = type.Item1.GetConstructor(new Type[0]);
            if (constr != null)
            {
                var obj = constr.Invoke(null) as IPCPacketBase;
                type.Item1.GetProperty("Value").SetValue(obj, data);
                return obj;
            }

            throw new ConstructorNotFoundException($"Constructor of {type.Item1} is not found.");
        }

        /// <summary>
        /// Parse packet IPC header
        /// </summary>
        /// <param name="array"></param>
        /// <returns>null if is not IPC packet</returns>
        public IPCHeader? ParseIPCHeader(byte[] array)
        {
            // Parse segment
            var SegHeader = ByteArrayToStructure<SegmentHeader>(array);
            if (SegHeader.type != SegmentType.IPC) return null;

            // Parse IPC
            var IPCHeader = ByteArrayToStructure<IPCHeader>(array);
            if (IPCHeader.reserved != 0x0014) return null;

            return IPCHeader;
        }

        /// <summary>
        /// Parse as specific packet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public T ParseAsPacket<T, T2>(byte[] array) where T : IPCPacketBase<T2>, new() where T2 : struct
        {
            return new T() { Value = ByteArrayToStructure<T2>(array) };
        }

        /// <summary>
        /// Parse as specific packet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public T ParseAsPacket<T>(byte[] array) where T : struct
        {
            return ByteArrayToStructure<T>(array);
        }

        T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            return (T)ByteArrayToStructure(typeof(T), bytes);
        }

        /// <summary>
        /// Convert byte array to object
        /// </summary>
        /// <param name="t"></param>
        /// <param name="bytes"></param>
        /// <see cref="https://stackoverflow.com/a/2887"/>
        /// <returns></returns>
        object ByteArrayToStructure(Type t, byte[] bytes)
        {
            object stuff;
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                stuff = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), t);
            }
            finally
            {
                handle.Free();
            }
            return stuff;
        }
    }


    [Serializable]
    public class ConstructorNotFoundException : Exception
    {
        public ConstructorNotFoundException() { }
        public ConstructorNotFoundException(string message) : base(message) { }
        public ConstructorNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ConstructorNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

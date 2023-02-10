using System;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace Lotlab.PluginCommon
{
    /// <summary>
    /// A Wrapper class for another unknown class
    /// </summary>
    public abstract class ClassProxy
    {
        /// <summary>
        /// Instance of wrapped object
        /// </summary>
        protected object Instance { get; }

        /// <summary>
        /// Wrapped object type
        /// </summary>
        protected Type ObjType { get; }

        public ClassProxy(Type type, object instance)
        {
            ObjType = type;
            Instance = instance;
        }

        public ClassProxy(object instance): this(instance.GetType(), instance)
        {
        }

        /// <summary>
        /// Call specific method
        /// </summary>
        /// <remarks>Parameter of this method is void</remarks>
        /// <param name="name">method name</param>
        /// <returns></returns>
        protected object CallMethod([CallerMemberName] string name = "")
        {
            return ObjType.GetMethod(name, new Type[] { }).Invoke(Instance, null);
        }
        /// <summary>
        /// Call specific method
        /// </summary>
        /// <param name="t1">Param type</param>
        /// <param name="param1">param</param>
        /// <param name="name">method name</param>
        /// <returns></returns>
        protected object CallMethod(Type t1, object param1, [CallerMemberName] string name = "")
        {
            return ObjType.GetMethod(name, new Type[] { t1 }).Invoke(Instance, new object[] { param1 });
        }
        /// <summary>
        /// Call specific method
        /// </summary>
        /// <param name="t">Param types</param>
        /// <param name="param">Params</param>
        /// <param name="name">method name</param>
        /// <returns></returns>
        protected object CallMethod(Type[] t, object[] param, [CallerMemberName] string name = "")
        {
            return ObjType.GetMethod(name, t).Invoke(Instance, param);
        }
        /// <summary>
        /// Call specific method
        /// </summary>
        /// <typeparam name="T1">Param1 Type</typeparam>
        /// <param name="param1">Param1</param>
        /// <param name="name">method name</param>
        /// <returns></returns>
        protected object CallMethod<T1>(T1 param1, [CallerMemberName] string name = "")
        {
            return ObjType.GetMethod(name, new Type[] { typeof(T1) }).Invoke(Instance, new object[] { param1 });
        }
        /// <summary>
        /// Call specific method
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="name">method name</param>
        /// <returns></returns>
        protected object CallMethod<T1, T2>(T1 param1, T2 param2, [CallerMemberName] string name = "")
        {
            return ObjType.GetMethod(name, new Type[] { typeof(T1), typeof(T2) }).Invoke(Instance, new object[] { param1, param2 });
        }
        /// <summary>
        /// Call specific method
        /// </summary>
        /// <param name="name">method name</param>
        /// <returns></returns>
        protected object CallMethod<T1, T2, T3>(T1 param1, T2 param2, T3 param3, [CallerMemberName] string name = "")
        {
            return ObjType.GetMethod(name, new Type[] { typeof(T1), typeof(T2), typeof(T3) }).Invoke(Instance, new object[] { param1, param2, param3 });
        }
        /// <summary>
        /// Call specific method
        /// </summary>
        /// <param name="name">method name</param>
        /// <returns></returns>
        protected object CallMethod<T1, T2, T3, T4>(T1 param1, T2 param2, T3 param3, T4 param4, [CallerMemberName] string name = "")
        {
            return ObjType.GetMethod(name, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }).Invoke(Instance, new object[] { param1, param2, param3, param4 });
        }
        /// <summary>
        /// Call specific method
        /// </summary>
        /// <param name="name">method name</param>
        /// <returns></returns>
        protected object CallMethod<T1, T2, T3, T4, T5>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, [CallerMemberName] string name = "")
        {
            return ObjType.GetMethod(name, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) }).Invoke(Instance, new object[] { param1, param2, param3, param4, param5 });
        }
        /// <summary>
        /// Call specific method
        /// </summary>
        /// <param name="name">method name</param>
        /// <returns></returns>
        protected object CallMethod<T1, T2, T3, T4, T5, T6>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, [CallerMemberName] string name = "")
        {
            return ObjType.GetMethod(name, new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) }).Invoke(Instance, new object[] { param1, param2, param3, param4, param5, param6 });
        }

        /// <summary>
        /// Get property value
        /// </summary>
        /// <param name="name">property name</param>
        /// <returns></returns>
        protected object PropertyGet([CallerMemberName] string name = "", BindingFlags flags = BindingFlags.Instance | BindingFlags.Public)
        {
            return ObjType.GetProperty(name, flags).GetValue(Instance);
        }
        /// <summary>
        /// Set property value
        /// </summary>
        /// <param name="val">value</param>
        /// <param name="name">property name</param>
        protected void PropertySet(object val, [CallerMemberName] string name = "", BindingFlags flags = BindingFlags.Instance | BindingFlags.Public)
        {
            ObjType.GetProperty(name, flags).SetValue(Instance, val);
        }
        /// <summary>
        /// Add event handler
        /// </summary>
        /// <param name="del"></param>
        /// <param name="name">Event name</param>
        protected void EventAdd(Delegate del, [CallerMemberName] string name = "")
        {
            var t = GetEventDelegateType(name);
            var targ = Delegate.CreateDelegate(t, del.Target, del.Method);
            ObjType.GetEvent(name).AddEventHandler(Instance, targ);
        }
        /// <summary>
        /// Remove event handler
        /// </summary>
        /// <param name="del"></param>
        /// <param name="name">Event name</param>
        protected void EventRemove(Delegate del, [CallerMemberName] string name = "")
        {
            var t = GetEventDelegateType(name);
            var targ = Delegate.CreateDelegate(t, del.Target, del.Method);
            ObjType.GetEvent(name).RemoveEventHandler(Instance, targ);
        }
        /// <summary>
        /// Get delegate type of specific event
        /// </summary>
        /// <param name="name">Event name</param>
        /// <returns></returns>
        protected Type GetEventDelegateType(string name)
        {
            return ObjType.GetEvent(name).EventHandlerType;
        }

        /// <summary>
        /// Set field value
        /// </summary>
        /// <param name="val"></param>
        /// <param name="name">field name</param>
        protected void FieldSet(object val, [CallerMemberName] string name = "", BindingFlags flags = BindingFlags.Instance | BindingFlags.Public)
        {
            ObjType.GetField(name, flags).SetValue(Instance, val);
        }
        /// <summary>
        /// Get field value
        /// </summary>
        /// <param name="name">field name</param>
        /// <returns></returns>
        protected object FieldGet([CallerMemberName] string name = "", BindingFlags flags = BindingFlags.Instance | BindingFlags.Public)
        {
            return ObjType.GetField(name, flags).GetValue(Instance);
        }

        /// <summary>
        /// Get type of specific name
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static Type GetTypeOfName(string typeName)
        {
            var asm = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in asm)
            {
                Type t = assembly.GetType(typeName, false);
                if (t != null)
                    return t;
            }
            return null;
        }
    }
}

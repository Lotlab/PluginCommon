namespace Lotlab.PluginCommon.FFXIV.Parser
{
    /// <summary>
    /// IPC Packet
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class IPCPacketBase<T> : IPCPacketBase, INetworkPacket<T> where T : struct
    {
        /// <summary>
        /// Value of packet body
        /// </summary>
        public T Value { get; set; }
    }

    /// <summary>
    /// Base class of IPCPacketBase
    /// </summary>
    /// <remarks>
    /// DO NOT USE THIS!
    /// Use IPCPacketBase<T> instead.
    /// </remarks>
    public interface INetworkPacket<T> where T : struct
    {
        T Value { get; set; }
    }

    /// <summary>
    /// Base class of IPCPacketBase
    /// </summary>
    /// <remarks>
    /// DO NOT USE THIS!
    /// Use IPCPacketBase<T> instead.
    /// </remarks>
    public abstract class IPCPacketBase { }
}

/// <summary>
/// CommonNetwork.h
/// </summary>
/// <remarks>
/// The following content is copied and modified from Sapphire.
/// </remarks>
/// <see cref="https://github.com/SapphireServer/Sapphire/blob/master/src/common/Network/CommonNetwork.h"/>

using System;
using System.Runtime.InteropServices;

namespace Lotlab.PluginCommon.FFXIV.Parser
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SegmentHeader
    {
        /** The size of the segment header and its data. */
        public UInt32 size;
        /** The session ID this segment describes. */
        public UInt32 source_actor;
        /** The session ID this packet is being delivered to. */
        public UInt32 target_actor;
        /** The segment type. (1, 2, 3, 7, 8, 9, 10) */
        [MarshalAs(UnmanagedType.U2)]
        public SegmentType type;

        UInt16 padding;
    }

    public enum SegmentType : UInt16
    {
        SessionInit = 1,
        IPC = 3,
        KeepAlive = 7,
        //Response = 8,
        EncryptInit = 9,
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct IPCHeader
    {
        public SegmentHeader segmentHeader;

        public UInt16 reserved;
        public UInt16 type;
        public UInt16 padding;
        public UInt16 serverId;
        public UInt32 timestamp;
        UInt32 padding1;
    }
}

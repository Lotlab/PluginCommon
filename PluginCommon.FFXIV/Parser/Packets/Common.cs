/// <remarks>
/// The following content is copy and modified from Sapphire.
/// </remarks>
/// <see cref="https://github.com/SapphireServer/Sapphire"/>
/// 

using System.Runtime.InteropServices;

using uint64_t = System.UInt64;
using int64_t = System.Int64;
using uint32_t = System.UInt32;
using int32_t = System.Int32;
using uint16_t = System.UInt16;
using int16_t = System.Int16;
using uint8_t = System.Byte;
using int8_t = System.Byte;

namespace Lotlab.PluginCommon.FFXIV.Parser.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FFXIVARR_POSITION3
    {
        public float x;
        public float y;
        public float z;

        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LandIdent
    {
        public uint16_t landId; //00
        public uint16_t wardNum; //02
        public uint16_t territoryTypeId; //04
        public uint16_t worldId; //06

        public override string ToString()
        {
            return $"(World: {worldId}, Map: {territoryTypeId}, Ward: {wardNum}, Land: {landId})";
        }
    };
}

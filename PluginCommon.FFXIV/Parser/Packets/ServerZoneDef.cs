/// <summary>
/// ServerZoneDef.h
/// </summary>
/// <remarks>
/// The following content is copied and modified from Sapphire.
/// </remarks>
/// <see cref="https://github.com/SapphireServer/Sapphire/blob/master/src/common/Network/PacketDef/Zone/ServerZoneDef.h"/>
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
    public struct FFXIVIpcItemInfo
    {
        public IPCHeader ipc;

        public uint32_t containerSequence;
        public uint32_t unknown;
        public uint16_t containerId;
        public uint16_t slot;
        public uint32_t quantity;
        public uint32_t catalogId;
        public uint32_t reservedFlag;
        public uint64_t signatureId;
        public uint8_t hqFlag;
        public uint8_t unknown2;
        public uint16_t condition;
        public uint16_t spiritBond;
        public uint16_t stain;
        public uint32_t glamourCatalogId;
        public uint16_t materia1;
        public uint16_t materia2;
        public uint16_t materia3;
        public uint16_t materia4;
        public uint16_t materia5;
        public uint8_t buffer1;
        public uint8_t buffer2;
        public uint8_t buffer3;
        public uint8_t buffer4;
        public uint8_t buffer5;
        public uint8_t padding;
        public uint32_t unknown10;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FFXIVIpcObjectSpawn
    {
        public IPCHeader ipc;

        public uint8_t spawnIndex;
        public uint8_t objKind;
        public uint8_t state;
        public uint8_t unknown3;
        public uint32_t objId;
        public uint32_t actorId;
        public uint32_t levelId;
        public uint32_t unknown10;
        public uint32_t someActorId14;
        public uint32_t gimmickId;
        public float scale;
        public int16_t unknown20a;
        public uint16_t rotation;
        public int16_t unknown24a;
        public int16_t unknown24b;
        public uint16_t flag;
        public int16_t unknown28c;
        public uint32_t housingLink;
        public FFXIVARR_POSITION3 position;
        public int16_t unknown3C;
        public int16_t unknown3E;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FFXIVIpcLandInfoSign
    {
        public IPCHeader ipc;

        public LandIdent landIdent;
        public uint64_t ownerId; // ither contentId or fcId
        public uint32_t unknow1;
        public uint8_t houseIconAdd;
        public uint8_t houseSize;
        public uint8_t houseType;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 23)]
        public byte[] estateName;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 193)]
        public byte[] estateGreeting;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)]
        public byte[] ownerName;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public byte[] fcTag;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public uint8_t[] tag;
    };

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FFXIVIpcHousingWardInfo
    {
        public IPCHeader ipc;

        public LandIdent landIdent;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct HouseInfoEntry
        {
            public uint32_t housePrice;
            public uint8_t infoFlags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public uint8_t[] houseAppeal;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] estateOwnerName;
        }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public HouseInfoEntry[] houseInfoEntry;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public uint8_t[] purchaseType;

        public uint32_t padding;
    };
}

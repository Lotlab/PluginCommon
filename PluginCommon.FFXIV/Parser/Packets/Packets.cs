/// <summary>
/// ServerZoneDef.h
/// </summary>
/// <remarks>
/// The following content is copied and modified from Sapphire.
/// </remarks>
/// <see cref="https://github.com/SapphireServer/Sapphire/blob/master/src/common/Network/PacketDef/Zone/ServerZoneDef.h"/>
/// 

namespace Lotlab.PluginCommon.FFXIV.Parser.Packets
{
    public class ItemInfo : IPCPacketBase<FFXIVIpcItemInfo> 
    {
        public override string ToString()
        {
            return $"Item ID: {Value.catalogId}, Slot: ({Value.containerSequence}, {Value.containerId}, {Value.slot}), Quantity: {Value.quantity}";
        }
    }

    public class ObjectSpawn : IPCPacketBase<FFXIVIpcObjectSpawn> 
    {
        /// <summary>
        /// Object Index
        /// </summary>
        /// <remarks>
        /// For indoor object, you should use (ObjIndex + (LandID) << 8) as real index.
        /// </remarks>
        public byte HousingObjIndex => (byte)(Value.housingLink & 0xFF);

        /// <summary>
        /// For yard object, this is housing land id.
        /// For indoor object, this is higher 8 bit of objectIndex
        /// </summary>
        public byte HousingLandID => (byte)((Value.housingLink >> 8) & 0xFF);

        public byte HousingUnknown => (byte)((Value.housingLink >> 16) & 0xFF);

        public byte HousingObjSubIndex => (byte)((Value.housingLink >> 24) & 0xFF);

        public override string ToString()
        {
            return $"Object Spawn. Index: {Value.spawnIndex}, Kind: {Value.objKind}, State: {Value.state}, ObjID: {Value.objId}, ActorID: {Value.actorId}, LevelID: {Value.levelId}, HousingLink: ({HousingObjIndex}, {HousingLandID}, {HousingUnknown}, {HousingObjSubIndex}), Pos: {Value.position}";
        }
    }

    public class LandInfoSign : IPCPacketBase<FFXIVIpcLandInfoSign> 
    {
        public override string ToString()
        {
            return $"House {Value.landIdent}. Size: {Value.houseSize}, Name: {Value.estateName}, Owner: {Value.ownerName}<{Value.fcTag}>";
        }
    }

    public class HousingWardInfo : IPCPacketBase<FFXIVIpcHousingWardInfo> 
    {
        public override string ToString()
        {
            return $"Ward Info: {Value.landIdent}";
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lotlab.PluginCommon.FFXIV.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lotlab.PluginCommon.FFXIV.Parser.Packets;

namespace Lotlab.PluginCommon.FFXIV.Parser.Tests
{
    [TestClass()]
    public class NetworkParserTests
    {
        [TestMethod()]
        public void LookupPacketTest()
        {
            var parser = new NetworkParser();
            parser.LookupPacket();
        }

        [TestMethod()]
        public void ParseItemInfoTest()
        {
            var data = "60000000cccccccccccccccc030000001400050300002d009ed0e661000000002c020000d1071100ac0d030001000000f96a00000000000000000000000000000000265800000000000000000000000000000000000000000000000000000000";
            var parser = new NetworkParser();

            parser.SetOpcode<ItemInfo, FFXIVIpcItemInfo>(0x0305);
            var packet = parser.ParsePacket(data.ToByteArray());
            Assert.IsNotNull(packet);
            Assert.IsInstanceOfType(packet, typeof(ItemInfo));

            var item = packet as ItemInfo;
            Assert.AreEqual(0x022cU, item.Value.containerSequence);
            Assert.AreEqual(1116113U, item.Value.unknown);
            Assert.AreEqual(3500U, item.Value.containerId);
            Assert.AreEqual(3U, item.Value.slot);
            Assert.AreEqual(1U, item.Value.quantity);
            Assert.AreEqual(27385U, item.Value.catalogId);
            Assert.AreEqual(0U, item.Value.hqFlag);
            Assert.AreEqual(22566U, item.Value.condition);
        }

        [TestMethod()]
        public void ParseLandInfoTest()
        {
            var data = "60000000cccccccccccccccc030000001400050300002d009ed0e6610000000000000000540127001581e97df410221100000000010102e8a385e4bfaee5a5bde99abe000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000e5b79de7abafe5bab7e688900000000000000000000000000000000000000000000000000000000000";
            var parser = new NetworkParser();

            parser.SetOpcode<LandInfoSign, FFXIVIpcLandInfoSign>(0x0305);
            var packet = parser.ParsePacket(data.ToByteArray());
            Assert.IsNotNull(packet);
            Assert.IsInstanceOfType(packet, typeof(LandInfoSign));

            var land = (packet as LandInfoSign).Value;

            Assert.AreEqual((Int16)0, land.landIdent.landId);
            Assert.AreEqual((Int16)0, land.landIdent.wardNum);
            Assert.AreEqual((Int16)0x0154, land.landIdent.territoryTypeId);
            Assert.AreEqual((Int16)0x0027, land.landIdent.worldId);

            Assert.AreEqual(1234567890123456789U, land.ownerId);
            Assert.AreEqual((byte)1, land.houseIconAdd);
            Assert.AreEqual((byte)1, land.houseSize);
            Assert.AreEqual((byte)2, land.houseType);

            Assert.AreEqual("装修好难", land.estateName.GetUTF8String());
            Assert.AreEqual("", land.estateGreeting.GetUTF8String());
            Assert.AreEqual("川端康成", land.ownerName.GetUTF8String());
            Assert.AreEqual("", land.fcTag.GetUTF8String());
        }

        [TestMethod()]
        public void ParseAsPacketTest()
        {
            var data = "60000000cccccccccccccccc030000001400050300002d009ed0e66100000000";
            var parser = new NetworkParser();
            var segment = parser.ParseAsPacket<IPCHeader>(data.ToByteArray());

            Assert.AreEqual(0x60U, segment.segmentHeader.size);
            Assert.AreEqual(0xccccccccU, segment.segmentHeader.source_actor);
            Assert.AreEqual(0xccccccccU, segment.segmentHeader.target_actor);
            Assert.AreEqual(SegmentType.IPC, segment.segmentHeader.type);

            Assert.AreEqual(0x14U, segment.reserved);
            Assert.AreEqual(0x305U, segment.type);
            Assert.AreEqual(0x002dU, segment.serverId);
            Assert.AreEqual(0x61e6d09eU, segment.timestamp);
        }
    }
}
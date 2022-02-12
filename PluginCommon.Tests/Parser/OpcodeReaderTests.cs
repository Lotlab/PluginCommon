using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lotlab.PluginCommon.FFXIV.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lotlab.PluginCommon.FFXIV.Parser.Tests
{
    [TestClass()]
    public class OpcodeReaderTests
    {
        [TestMethod()]
        public void ParseMachinaStyleOpcodeTest()
        {
            var str =
@"StatusEffectList|bc
StatusEffectList2|1ff
StatusEffectList3|2af
BossStatusEffectList|7e";

            var result = new MachinaOpcodeReader().Parse(str);

            Assert.AreEqual("StatusEffectList", result[0xbc]);
            Assert.AreEqual("StatusEffectList2", result[0x1ff]);
            Assert.AreEqual("StatusEffectList3", result[0x2af]);
            Assert.AreEqual("BossStatusEffectList", result[0x7e]);
        }

        [TestMethod()]
        public void ParseCStyleOpcodeTest()
        {
            var str = @"
// Server Zone
UpdateHpMpTp = 0x03C8,
PlayerStats = 0x0142,
ActorControl = 0x012E,
ActorControlSelf = 0x0303,
ActorControlTarget = 0x01D2,
Playtime = 0x0351,

// Client Zone
UpdatePositionHandler = 154,
ClientTrigger = 124,
ChatHandler = 884,
";
            var result = new WizardOpcodeReader().Parse(str);

            Assert.AreEqual("UpdateHpMpTp", result[0x03C8]);
            Assert.AreEqual("PlayerStats", result[0x0142]);
            Assert.AreEqual("ActorControl", result[0x012E]);
            Assert.AreEqual("ActorControlSelf", result[0x0303]);
            Assert.AreEqual("ActorControlTarget", result[0x01D2]);
            Assert.AreEqual("Playtime", result[0x0351]);

            Assert.AreEqual("UpdatePositionHandler", result[154]);
            Assert.AreEqual("ClientTrigger", result[124]);
            Assert.AreEqual("ChatHandler", result[884]);
        }
    }
}
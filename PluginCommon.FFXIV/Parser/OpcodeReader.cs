using System;
using System.IO;
using System.Collections.Generic;

namespace Lotlab.PluginCommon.FFXIV.Parser
{
    /// <summary>
    /// Simple opcode file reader.
    /// You could write your own opcode file parser
    /// </summary>
    public abstract class OpcodeReaderBase
    {
        public abstract Dictionary<ushort, string> Parse(string content);

        public Dictionary<ushort, string> ReadFile(string path) 
        {
            var content = File.ReadAllText(path);
            return Parse(content);
        }
    }

    /// <summary>
    /// Machina style opcode parser
    /// </summary>
    /// <example>
    /// Input example:
    /// ﻿StatusEffectList|bc
    /// StatusEffectList2|1ff
    /// StatusEffectList3|2af
    /// </example>
    public class MachinaOpcodeReader : OpcodeReaderBase
    {
        public override Dictionary<ushort, string> Parse(string content)
        {
            Dictionary<ushort, string> dict = new Dictionary<ushort, string>();
            var lines = content.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var seg = line.Split('|');
                if (seg.Length != 2) continue;

                var name = seg[0].Trim();
                var opcode = Convert.ToUInt16(seg[1].Trim(), 16);

                dict[opcode] = name;
            }

            return dict;
        }
    }

    /// <summary>
    /// Opcode Wizard (C/C++) style opcode parser
    /// </summary>
    /// <example>
    /// // Server side
    /// ﻿StatusEffectList = 0xbc
    /// StatusEffectList2 = 0x1ff
    /// StatusEffectList3 = 0x2af
    /// </example>
    public class WizardOpcodeReader : OpcodeReaderBase
    {
        public override Dictionary<ushort, string> Parse(string content)
        {
            Dictionary<ushort, string> dict = new Dictionary<ushort, string>();
            var lines = content.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (line.StartsWith("//") || string.IsNullOrWhiteSpace(line))
                    continue;

                var seg = line.Split('=');
                if (seg.Length != 2) continue;

                var name = seg[0].Trim();
                var opcodeStr = seg[1].Trim().TrimEnd(new char[] { ',', ';' });
                if (opcodeStr.StartsWith("0x"))
                {
                    var opcode = Convert.ToUInt16(opcodeStr.Remove(0, 2), 16);
                    dict[opcode] = name;
                }
                else
                {
                    var opcode = Convert.ToUInt16(opcodeStr);
                    dict[opcode] = name;
                }
            }

            return dict;
        }
    }
}

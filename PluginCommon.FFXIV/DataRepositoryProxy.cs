using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Lotlab.PluginCommon.FFXIV
{
    /// <summary>
    /// Wrapper for IDataRepository
    /// </summary>
    public class DataRepositoryProxy : ClassProxy
    {
        public DataRepositoryProxy(object instance) : base(instance)
        {
        }

        /// <summary>
        /// Returns the currently-selected plugin language.  Note that in the future this may change to automatically detect the language from game memory.
        /// </summary>
        /// <returns>1=EN, 2=FR, 3=DE, 4=JP</returns>
        public int GetSelectedLanguageID()
        {
            return (int)CallMethod();
        }

        /// <summary>
        /// Returns a reference to the .Net process for the currently selected FFXIV game instance
        /// </summary>
        /// <returns>System.Diagnostics.Process reference</returns>
        public Process GetCurrentFFXIVProcess()
        {
            return (Process)CallMethod();
        }

        /// <summary>
        /// Loads and returns an unsorted dictionary for the specified resource type
        /// </summary>
        /// <param name="resourceType">The type of resource to load</param>
        /// <returns>unsorted dictionary containing all resource id's and strings of the specified type</returns>
        public IDictionary<uint, string> GetResourceDictionary(ResourceType resourceType)
        {
            return (IDictionary<uint, string>)CallMethod(GetTypeOfName("FFXIV_ACT_Plugin.Common.ResourceType"), resourceType);
        }

        /// <summary>
        /// Returns the current territory identifier from game memory
        /// </summary>
        /// <returns>a uint specifying the current territory id</returns>
        public uint GetCurrentTerritoryID()
        {
            return (uint)CallMethod();
        }

        /// <summary>
        /// Returns the unique identifier for the current active player, which can be used to locate their Combatant record.
        /// </summary>
        /// <returns>the player's identifier</returns>
        public uint GetCurrentPlayerID()
        {
            return (uint)CallMethod();
        }

        /// <summary>
        /// Retrieves a list containing all active combatants, including players and mobs.  Note that this method will only refresh the data every 100ms, and 
        ///   shares a single instance of the list object with all callers
        /// </summary>
        /// <returns>a read-only collection containing all currently loaded combatants</returns>
        public ReadOnlyCollection<CombatantProxy> GetCombatantList()
        {
            var objs = (IReadOnlyCollection<object>)CallMethod();
            var list = new List<CombatantProxy>();

            foreach (var item in objs)
            {
                list.Add(new CombatantProxy(item));
            }

            return new ReadOnlyCollection<CombatantProxy>(list);
        }

        /// <summary>
        /// Returns a class containing information about the logged-in player
        /// </summary>
        /// <returns>Player stats class</returns>
        public PlayerProxy GetPlayer()
        {
            return new PlayerProxy(CallMethod());
        }

        /// <summary>
        /// Returns the current Date/Time last reported to the game client by the server
        /// </summary>
        /// <returns>a date/time with the server's last communicated time</returns>
        public DateTime GetServerTimestamp()
        {
            return (DateTime)CallMethod();
        }

        /// <summary>
        /// Returns the game version, parsed from ffxivgame.ver file
        /// </summary>
        /// <returns>a string containing the contents of ffxivgame.ver</returns>
        public string GetGameVersion()
        {
            return (string)CallMethod();
        }

        /// <summary>
        /// Returns a boolean indicating whether in-game chatlog text is being logged or hidden.
        /// </summary>
        public bool IsChatLogAvailable()
        {
            return (bool)CallMethod();
        }
    }

    /// <summary>
    /// Wrapper for Player
    /// </summary>
    public class PlayerProxy : ClassProxy
    {
        public PlayerProxy(object instance) : base(instance)
        {
        }

        public uint JobID { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint Str { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint Dex { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint Vit { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint Intel { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint Mnd { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint Pie { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint Attack { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint DirectHit { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint Crit { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint AttackMagicPotency { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint HealMagicPotency { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint Det { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint SkillSpeed { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint SpellSpeed { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint Tenacity { get => (uint)PropertyGet(); set => PropertySet(value); }
        public UInt64 LocalContentId { get => (UInt64)PropertyGet(); set => PropertySet(value); }
    }

    /// <summary>
    /// Wrapper for NetworkBuff
    /// </summary>
    public class NetworkBuffProxy : ClassProxy
    {
        public NetworkBuffProxy(object instance) : base(instance)
        {
        }

        public UInt16 BuffID { get => (UInt16)PropertyGet(); set => PropertySet(value); }
        public UInt16 BuffExtra { get => (UInt16)PropertyGet(); set => PropertySet(value); }
        public DateTime Timestamp { get => (DateTime)PropertyGet(); set => PropertySet(value); }
        public float Duration { get => (float)PropertyGet(); set => PropertySet(value); }
        public uint ActorID { get => (uint)PropertyGet(); set => PropertySet(value); }
        public string ActorName { get => (string)PropertyGet(); set => PropertySet(value); }
        public uint TargetID { get => (uint)PropertyGet(); set => PropertySet(value); }
        public string TargetName { get => (string)PropertyGet(); set => PropertySet(value); }
    }

    /// <summary>
    /// Wrapper for Combatant
    /// </summary>
    public class CombatantProxy : ClassProxy
    {
        public CombatantProxy(object instance) : base(instance)
        {
        }

        public uint ID { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint OwnerID { get => (uint)PropertyGet(); set => PropertySet(value); }
        public byte type { get => (byte)PropertyGet(); set => PropertySet(value); }
        public int Job { get => (int)PropertyGet(); set => PropertySet(value); }
        public int Level { get => (int)PropertyGet(); set => PropertySet(value); }
        public string Name { get => (string)PropertyGet(); set => PropertySet(value); }
        public uint CurrentHP { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint MaxHP { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint CurrentMP { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint MaxMP { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint CurrentCP { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint MaxCP { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint CurrentGP { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint MaxGP { get => (uint)PropertyGet(); set => PropertySet(value); }
        public Boolean IsCasting { get => (Boolean)PropertyGet(); set => PropertySet(value); }
        public uint CastBuffID { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint CastTargetID { get => (uint)PropertyGet(); set => PropertySet(value); }
        public Single CastDurationCurrent { get => (Single)PropertyGet(); set => PropertySet(value); }
        public Single CastDurationMax { get => (Single)PropertyGet(); set => PropertySet(value); }
        public Single PosX { get => (Single)PropertyGet(); set => PropertySet(value); }
        public Single PosY { get => (Single)PropertyGet(); set => PropertySet(value); }
        public Single PosZ { get => (Single)PropertyGet(); set => PropertySet(value); }
        public Single Heading { get => (Single)PropertyGet(); set => PropertySet(value); }
        public uint CurrentWorldID { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint WorldID { get => (uint)PropertyGet(); set => PropertySet(value); }
        public string WorldName { get => (string)PropertyGet(); set => PropertySet(value); }
        public uint BNpcNameID { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint BNpcID { get => (uint)PropertyGet(); set => PropertySet(value); }
        public uint TargetID { get => (uint)PropertyGet(); set => PropertySet(value); }
        public byte EffectiveDistance { get => (byte)PropertyGet(); set => PropertySet(value); }

        public PartyType PartyType { get => (PartyType)PropertyGet(); set => PropertySet(value); }
        public IntPtr Address { get => (IntPtr)PropertyGet(); set => PropertySet(value); }
        public int Order { get => (int)PropertyGet(); set => PropertySet(value); }

        public NetworkBuffProxy[] NetworkBuffs
        {
            get
            {
                var objs = (object[])PropertyGet();
                var ret = new NetworkBuffProxy[objs.Length];

                for (int i = 0; i < objs.Length; i++)
                {
                    ret[i] = new NetworkBuffProxy(objs[i]);
                }

                return ret;
            }
            set => throw new NotImplementedException();
        }
    }

    /// <summary>
    /// PartyType
    /// </summary>
    /// <remarks>
    /// Copy from FFXIV_ACT_Plugin
    /// </remarks>
    public enum PartyType
    {
        None = 0,
        Party = 1,
        Alliance = 2
    }

    /// <summary>
    /// ResourceType
    /// </summary>
    /// <remarks>
    /// Copy from FFXIV_ACT_Plugin
    /// </remarks>
    public enum ResourceType
    {
        BuffList_EN,
        BuffList_FR,
        BuffList_DE,
        BuffList_JP,
        SkillList_EN,
        SkillList_FR,
        SkillList_DE,
        SkillList_JP,
        WorldList_EN,
        ZoneList_EN,
        TerritoryList_EN,
        ItemList_EN,
        MountList_EN,
        AutoAttackList_EN,
        BuffList_KR,
        SkillList_KR
    }
}

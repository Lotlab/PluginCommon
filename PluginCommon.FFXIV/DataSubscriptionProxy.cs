using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Lotlab.PluginCommon.FFXIV
{
    /// <summary>
    /// Wrapper for IDataSubscription
    /// </summary>
    public class DataSubscriptionProxy : ClassProxy
    {
        public DataSubscriptionProxy(object instance) : base(instance)
        {
        }

        public event NetworkReceivedDelegate NetworkReceived { add => EventAdd(value); remove => EventRemove(value); }
        public event NetworkSentDelegate NetworkSent { add => EventAdd(value); remove => EventRemove(value); }
        public event CombatantAddedDelegate CombatantAdded { add => EventAdd(value); remove => EventRemove(value); }
        public event CombatantRemovedDelegate CombatantRemoved { add => EventAdd(value); remove => EventRemove(value); }
        public event PrimaryPlayerDelegate PrimaryPlayerChanged { add => EventAdd(value); remove => EventRemove(value); }
        public event ZoneChangedDelegate ZoneChanged { add => EventAdd(value); remove => EventRemove(value); }
        public event PlayerStatsChangedDelegate PlayerStatsChanged { add => EventAdd(value); remove => EventRemove(value); }
        public event PartyListChangedDelegate PartyListChanged { add => EventAdd(value); remove => EventRemove(value); }
        public event LogLineDelegate LogLine { add => EventAdd(value); remove => EventRemove(value); }
        public event ParsedLogLineDelegate ParsedLogLine { add => EventAdd(value); remove => EventRemove(value); }
        public event ProcessChangedDelegate ProcessChanged { add => EventAdd(value); remove => EventRemove(value); }
    }

    public delegate void NetworkReceivedDelegate(string connection, long epoch, byte[] message);
    public delegate void NetworkSentDelegate(string connection, long epoch, byte[] message);
    public delegate void CombatantAddedDelegate(object Combatant);
    public delegate void CombatantRemovedDelegate(object Combatant);
    public delegate void PrimaryPlayerDelegate();
    public delegate void ZoneChangedDelegate(uint ZoneID, string ZoneName);
    public delegate void PlayerStatsChangedDelegate(object playerStats);
    public delegate void PartyListChangedDelegate(ReadOnlyCollection<uint> partyList, int partySize);
    public delegate void LogLineDelegate(uint EventType, uint Seconds, string logline);
    public delegate void ParsedLogLineDelegate(uint sequence, int messagetype, string message);
    public delegate void ProcessChangedDelegate(Process process);
}

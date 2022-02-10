namespace Lotlab.PluginCommon.FFXIV
{
    /// <summary>
    /// Wrapper class for FFXIV_ACT_Plugin.FFXIV_ACT_Plugin
    /// </summary>
    public class ACTPluginProxy : ClassProxy
    {
        /// <summary>
        /// Create instance of FFXIV_ACT_Plugin
        /// </summary>
        /// <param name="instance">FFXIV_ACT_Plugin instance</param>
        public ACTPluginProxy(object instance) : base(instance)
        {
            DataSubscription = new DataSubscriptionProxy(PropertyGet(nameof(DataSubscription)));
            DataRepository = new DataRepositoryProxy(PropertyGet(nameof(DataRepository)));
        }

        public DataSubscriptionProxy DataSubscription { get; }
        public DataRepositoryProxy DataRepository { get; }
        public bool PluginStarted { get => (bool)PropertyGet(); }

        /// <summary>
        /// Determine given object is FFXIV_ACT_Plugin
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsFFXIVPlugin(object instance)
        {
            return instance.GetType().FullName == "FFXIV_ACT_Plugin.FFXIV_ACT_Plugin";
        }
    }
}

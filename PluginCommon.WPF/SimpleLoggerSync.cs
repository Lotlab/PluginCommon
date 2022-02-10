using System.Windows.Data;

namespace Lotlab.PluginCommon
{
    public class SimpleLoggerSync : SimpleLogger
    {
        public SimpleLoggerSync(string file, LogLevel filter = LogLevel.INFO, int maxObserveLogs = 1000) : base(file, filter, maxObserveLogs)
        {
            this.EnableLogSync();
        }
    }

    public static class SimpleLoggerExt
    {
        public static void EnableLogSync(this SimpleLogger logger)
        {
            BindingOperations.EnableCollectionSynchronization(logger.ObserveLogs, logger.logLock);
        }
    }
}
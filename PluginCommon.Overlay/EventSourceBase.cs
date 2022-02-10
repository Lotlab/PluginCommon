using Newtonsoft.Json.Linq;
using System;
using RainbowMage.OverlayPlugin;
using System.Collections.Generic;

namespace Lotlab.PluginCommon.Overlay
{
    /// <summary>
    /// Simple EventSourceBase
    /// </summary>
    /// <remarks>
    /// Rewritten from original EventSourceBase, remove timer.
    /// </remarks>
    /// <see cref="https://github.com/ngld/OverlayPlugin/blob/master/OverlayPlugin.Core/EventSourceBase.cs"/>
    public abstract class EventSourceBase : IEventSource
    {
        public string Name { get; protected set; }
        protected TinyIoCContainer container;
        private EventDispatcherProxy dispatcher;
        private bool updateRunning = false;

        protected ILogger logger;
        protected Dictionary<string, JObject> eventCache = new Dictionary<string, JObject>();

        // Backwards compat
        public EventSourceBase(ILogger _)
        {
            Init(Registry.GetContainer());
        }

        public EventSourceBase(TinyIoCContainer c)
        {
            Init(c);
        }

        private void Init(TinyIoCContainer c)
        {
            container = c;
            logger = container.Resolve<ILogger>();

            var type = ClassProxy.GetTypeOfName("RainbowMage.OverlayPlugin.EventDispatcher");
            dispatcher = new EventDispatcherProxy(container.Resolve(type));
        }

        public abstract System.Windows.Forms.Control CreateConfigControl();

        public abstract void LoadConfig(IPluginConfig config);

        public abstract void SaveConfig(IPluginConfig config);

        protected void Log(RainbowMage.OverlayPlugin.LogLevel level, string message, params object[] args)
        {
            logger.Log(level, message, args);
        }

        public virtual void Dispose()
        {
        }

        public virtual void Start()
        {
        }

        public virtual void Stop()
        {
        }

        protected void RegisterEventTypes(List<string> types)
        {
            dispatcher.RegisterEventTypes(types);
        }
        protected void RegisterEventType(string type)
        {
            dispatcher.RegisterEventType(type);
        }
        protected void RegisterEventType(string type, Func<JObject> initCallback)
        {
            dispatcher.RegisterEventType(type, initCallback);
        }

        protected void RegisterCachedEventTypes(List<string> types)
        {
            foreach (var type in types)
            {
                RegisterCachedEventType(type);
            }
        }

        protected void RegisterCachedEventType(string type)
        {
            eventCache[type] = null;
            dispatcher.RegisterEventType(type, () => eventCache[type]);
        }

        protected void RegisterEventHandler(string name, Func<JObject, JToken> handler)
        {
            dispatcher.RegisterHandler(name, handler);
        }

        protected void DispatchEvent(JObject e)
        {
            dispatcher.DispatchEvent(e);
        }

        protected void DispatchAndCacheEvent(JObject e)
        {
            eventCache[e["type"].ToString()] = e;
            dispatcher.DispatchEvent(e);
        }

        protected bool HasSubscriber(string eventName)
        {
            return dispatcher.HasSubscriber(eventName);
        }
    }
}

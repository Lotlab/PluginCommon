using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Lotlab.PluginCommon.Overlay
{
    /// <summary>
    /// Proxy for OverlayPlugin.EventDispatcher
    /// </summary>
    /// <remarks>
    /// Any method with IEventReceiver param is ignored.
    /// </remarks>
    public class EventDispatcherProxy : ClassProxy
    {
        public EventDispatcherProxy(object instance) : base(instance)
        {
        }

        public void RegisterHandler(string name, Func<JObject, JToken> handler)
        {
            CallMethod(name, handler);
        }

        public void RegisterEventTypes(List<string> names)
        {
            CallMethod(names);
        }

        public void RegisterEventType(string name)
        {
            CallMethod(name);
        }

        public void RegisterEventType(string name, Func<JObject> initCallback)
        {
            CallMethod(name, initCallback);
        }

        // Can be used to check that an event will be delivered before building
        // an expensive JObject that would otherwise be thrown away.
        public bool HasSubscriber(string eventName)
        {
            return (bool)CallMethod<string>(eventName);
        }

        public void DispatchEvent(JObject e)
        {
            CallMethod(e);
        }

        public JToken CallHandler(JObject e)
        {
            return (JToken)CallMethod(e);
        }
    }
}

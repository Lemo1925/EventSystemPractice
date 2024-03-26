using System.Collections.Generic;

namespace EventSystem
{
    // 事件单元
    public interface IEventCell {
        public string Name { get; set; }
    }

    public class EventManager 
    {
        // 事件句柄
        public delegate void EventAction(IEventCell game_event);
        // 用字典存储事件
        private static Dictionary<string, EventAction> channel = new Dictionary<string, EventAction>();
        
        public static void AddListener(string name, EventAction action)
        {
            // 将事件注册到字典中
            if (!channel.ContainsKey(name))
                channel[name] = null;
            channel[name] += action;
        }

        public static void RemoveListener(string name, EventAction action)
        {
            // 从字典中注销事件
            if (channel.ContainsKey(name))
                channel[name] -= action;
        }

        public static void Tick(IEventCell @event) 
        {
            // 驱动事件方法
            if (channel.TryGetValue(@event.Name, out EventAction action))
                action?.Invoke(@event);
        }
    }
}


using System.Collections.Generic;

namespace EventSystem
{
    // �¼���Ԫ
    public interface IEventCell {
        public string Name { get; set; }
    }

    public class EventManager 
    {
        // �¼����
        public delegate void EventAction(IEventCell game_event);
        // ���ֵ�洢�¼�
        private static Dictionary<string, EventAction> channel = new Dictionary<string, EventAction>();
        
        public static void AddListener(string name, EventAction action)
        {
            // ���¼�ע�ᵽ�ֵ���
            if (!channel.ContainsKey(name))
                channel[name] = null;
            channel[name] += action;
        }

        public static void RemoveListener(string name, EventAction action)
        {
            // ���ֵ���ע���¼�
            if (channel.ContainsKey(name))
                channel[name] -= action;
        }

        public static void Tick(IEventCell @event) 
        {
            // �����¼�����
            if (channel.TryGetValue(@event.Name, out EventAction action))
                action?.Invoke(@event);
        }
    }
}


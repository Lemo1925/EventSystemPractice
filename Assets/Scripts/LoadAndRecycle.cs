using EventSystem;
using GameObjectPool;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadAndRecycle : MonoBehaviour
{
    Button[] buttons;
    Dictionary<string, Stack<GameObject>> goDic = new Dictionary<string, Stack<GameObject>>();

    class PoolEvent : IEventCell
    {
        public string Name { get; set; }
        public PoolEvent(string name)
        {
            Name = name;
        }
    }

    private void Awake()
    {
        buttons = transform.GetComponentsInChildren<Button>();
    }

    void Start()
    {
        foreach (Button button in buttons) {
            PoolEvent gameEvent = new PoolEvent(button.name);
            EventManager.AddListener(button.name, OnBtnClick);
            button.onClick.AddListener(() => EventManager.Tick(gameEvent));
        }
    }

    public void OnBtnClick(IEventCell @event)
    {
        string type = @event.Name.Split('-')[0];
        string name = @event.Name.Split('-')[1];
        
        if (type == "Load")
        {
            if(!goDic.ContainsKey(name)) goDic.Add(name, new Stack<GameObject>());
            var go = ObjectPool.Instance.GetObject(name);
            var pos = new Vector3(
                x:Random.Range(-4.5f, 4.5f),
                y:go.transform.position.y,
                z:Random.Range(-4.5f, 4.5f)
                );
            go.transform.position = pos;
            goDic[name].Push(go);
        }
        else if (type == "Recycle")
        {
            if (goDic.ContainsKey(name) && goDic[name].Count != 0)
                ObjectPool.Instance.RecycleObj(goDic[name].Pop());
        }
    }
}

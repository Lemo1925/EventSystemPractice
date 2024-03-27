using EventSystem;
using GameObjectPool;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Obsolete("LoadAndRecycle is Disposed")]
public class LoadAndRecycle : MonoBehaviour
{
    /// <summary>
    /// 该类型已弃用
    /// </summary>
    Button[] buttons;
    Dictionary<string, Stack<GameObject>> goDic = new Dictionary<string, Stack<GameObject>>();

    class PoolEvent : IEventCell
    {
        public string Index { get; set; }
        public PoolEvent(string name) => Index = name;
    }

    private void Awake() => buttons = transform.GetComponentsInChildren<Button>();

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
        string type = @event.Index.Split('-')[0];
        string name = @event.Index.Split('-')[1];
        
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

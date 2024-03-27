using GameObjectPool;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventSystem;

namespace LoadRecycleObject
{

    public class LoadAndRecyclePanel : MonoBehaviour
    {
        public List<GameObject> items = new List<GameObject>();
        Dictionary<string, Stack<GameObject>> goDic = new Dictionary<string, Stack<GameObject>>();
        PanelButton[] buttons;
        Dropdown select;

        private void Awake()
        {
            buttons = GetComponentsInChildren<PanelButton>();
            select = GetComponentInChildren<Dropdown>();
            foreach (var item in items)
                select.options.Add(new Dropdown.OptionData { text = item.name });
        }
        
        void Start()
        {
            ObjectPool.Instance.Items = items;
            ObjectPool.Instance.InitPool();
            foreach (var button in buttons)
            {
                button.dropdown = select;
                EventManager.AddListener(button.Index, OnBtnClick);
                button.Button.onClick.AddListener(() => EventManager.Tick(button));
            }
        }

        public void OnBtnClick(IEventCell button)
        {
            string option = ((PanelButton)button).dropdown.captionText.text;
            ButtonType type = ((PanelButton)button).Type;
            switch (type)
            {
                case ButtonType.LOAD:
                    if (!goDic.ContainsKey(option)) goDic.Add(option, new Stack<GameObject>());
                    var go = ObjectPool.Instance.GetObject(option);
                    var pos = new Vector3(
                        x: Random.Range(-4.5f, 4.5f),
                        y: go.transform.position.y,
                        z: Random.Range(-4.5f, 4.5f)
                        );
                    go.transform.position = pos;
                    goDic[option].Push(go);
                    break;
                case ButtonType.RECYCLE:
                    if (goDic.ContainsKey(option) && goDic[option].Count != 0)
                        ObjectPool.Instance.RecycleObj(goDic[option].Pop());
                    break;
            }
        }
    }
}


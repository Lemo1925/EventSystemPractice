using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameObjectPool
{
    public class ObjectPool : MonoBehaviour
    {
        // 传递需要放入池中的对象
        public List<GameObject> Items { get; set; } = new List<GameObject>();

        const int Capacity = 18;
        public Dictionary<string, Queue<GameObject>> ObjectDic = new Dictionary<string, Queue<GameObject>>(Capacity);
        public static ObjectPool Instance;

        public void Awake()
        {
            if (Instance == null) Instance = this;
            InitPool();
        }
        
        public void InitPool()
        {
            foreach (var go in Items)
            {
                // 初始化对象池字典，以及Hierarchy
                ObjectDic[go.name] = new Queue<GameObject>();
                var root = new GameObject(string.Format("{0}Pool", go.name));
                root.transform.SetParent(transform);

                var obj = Instantiate(go, root.transform);
                obj.SetActive(false);

                ObjectDic[go.name].Enqueue(obj);
            }
        }
       
        public GameObject GetObject(string name)
        {
            if (!ObjectDic.ContainsKey(name))
                throw new Exception($"{name}不在池中");

            if (ObjectDic[name].Count == 1)
            {
                Transform root = transform.Find(string.Format("{0}Pool", name));
                for (int i = 0; i < Capacity; i++)
                {
                    GameObject obj = Instantiate(ObjectDic[name].Peek(), root);
                    obj.SetActive(false);
                    ObjectDic[name].Enqueue(obj);
                }
            }
            GameObject gameObject = ObjectDic[name].Dequeue();
            gameObject.SetActive(true);
            return gameObject;
        }
        
        public void RecycleObj(GameObject obj)
        {
            var key = obj.name.Replace("(Clone)", "");
            if (!ObjectDic.ContainsKey(key)) return;
            obj.SetActive(false);
            ObjectDic[key].Enqueue(obj);
        }
    }
}


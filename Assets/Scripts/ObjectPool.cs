using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameObjectPool
{
    public class ObjectPool : MonoBehaviour
    {
        // 传递需要放入池中的对象
        [SerializeField] List<GameObject> gameObjects;

        const int Capacity = 10;
        private Dictionary<string, List<GameObject>> ObjectDic = new Dictionary<string, List<GameObject>>(Capacity);
        public static ObjectPool Instance;

        public void Awake()
        {
            if (Instance == null) Instance = this;
            InitPool();
        }
        
        private void InitPool()
        {
            foreach (var go in gameObjects)
            {
                // 初始化对象池字典，以及Hierarchy
                ObjectDic[go.name] = new List<GameObject>(new GameObject[] { go });
                new GameObject(string.Format("{0}Pool",go.name)).transform.SetParent(transform);
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
                    GameObject obj = Instantiate(ObjectDic[name][0], root.transform);
                    obj.SetActive(false);
                    ObjectDic[name].Add(obj);
                }
            }
            GameObject gameObject = ObjectDic[name][0];
            gameObject.SetActive(true);
            ObjectDic[name].RemoveAt(1);
            return gameObject;
        }
        
        public void RecycleObj(GameObject obj)
        {
            var key = obj.name.Replace("(Clone)", "");
            if (!ObjectDic.ContainsKey(key)) return;
            obj.SetActive(false);
            ObjectDic[key].Add(obj);
        }
    }
}


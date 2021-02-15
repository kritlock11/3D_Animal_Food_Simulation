using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Assets.Scripts
{
    public class PoolManager<T> where T : PoolObject
    {
        private List<T> _pool;
        private Transform _parent;

        public PoolManager(Transform parent)
        {
            _pool = new List<T>();
            _parent = parent;
        }

        public void Initialize(int count, T prefab)
        {
            for (var i = 0; i < count; i++)
            {
                AddObject(prefab);
            }
        }

        public T GetObject()
        {
            foreach (var o in _pool.Where(o => o.gameObject.activeInHierarchy == false))
            {
                return o;
            }

            AddObject(_pool[0]);
            return _pool[_pool.Count - 1];
        }

        public void AddObject(T prefab)
        {
            var go = Object.Instantiate(prefab).GetComponent<T>();
            go.name = prefab.name;
            go.transform.SetParent(_parent);
            go.gameObject.SetActive(false);
            _pool.Add(go);
        }

        public void ClearAll()
        {
            _pool.Clear();
        }
    }
}

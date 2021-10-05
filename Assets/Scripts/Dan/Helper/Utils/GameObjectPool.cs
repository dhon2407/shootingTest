using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace Dan.Helper.Utils
{
    public class GameObjectPool<T> where T : PoolableMonobehaviour
    {
        private readonly PoolableMonobehaviour _prefab;
        private readonly PoolableMonobehaviour[] _prefabListList;
        private readonly int _size;
        private readonly List<PoolableMonobehaviour> _availableObjectsPool;
        private readonly GameObject _pool;

        private GameObjectPool(GameObject poolGameObject, PoolableMonobehaviour prefab, int size)
        {
            _pool = poolGameObject;
            _prefab = prefab;
            _size = size;
            _availableObjectsPool = new List<PoolableMonobehaviour>(size);
        }
        
        private GameObjectPool(GameObject poolGameObject, PoolableMonobehaviour[] prefabList, int size)
        {
            _pool = poolGameObject;
            _prefabListList = prefabList;
            _size = size;
            _availableObjectsPool = new List<PoolableMonobehaviour>(size);
        }

        public static GameObjectPool<T> CreateInstance(T prefab, int size, Transform parent = null)
        {
            var pool = new GameObjectPool<T>(new GameObject($"PoolOf{prefab.name}"), prefab, size);
            pool.SetParent(parent);
            pool.Populate();

            return pool;
        }
        
        public static GameObjectPool<T> CreateInstance(PoolableMonobehaviour[] prefab, int size, Transform parent = null)
        {
            var pool = new GameObjectPool<T>(new GameObject($"PoolOf{prefab[0].name}"), prefab, size);
            pool.SetParent(parent);
            pool.PopulateList();

            return pool;
        }
        
        public static GameObjectPool<T> CreateUIInstance(T prefab, int size, Vector3 position, Transform parent = null)
        {
            var pool = new GameObjectPool<T>(new GameObject($"PoolOf{prefab.name}", typeof(RectTransform)), prefab, size);
            pool.SetParent(parent);
            pool.Populate();
            pool.SetPosition(position);

            return pool;
        }

        public void SetPosition(Vector3 position)
        {
            Timing.RunCoroutine(SetPositionOnNextFrame());
                
            IEnumerator<float> SetPositionOnNextFrame()
            {
                yield return Timing.WaitForOneFrame;
                _pool.transform.position = position;
            }
        }

        public T GetObject()
        {
            if (_availableObjectsPool.Count <= 0)
                return null;
            
            var instance = _availableObjectsPool[0];
            _availableObjectsPool.RemoveAt(0);
            instance.gameObject.SetActive(true);
            return instance as T;
        }

        private void Populate()
        {
            for (int i = 0; i < _size; i++)
                CreatePoolObjectInstance();
        }
        
        private void PopulateList()
        {
            for (int i = 0; i < _size; i++)
                CreatePoolObjectInstanceList();
        }
        
        private void SetParent(Transform parent)
        {
            _pool.transform.SetParent(parent);
        }

        private void CreatePoolObjectInstance()
        {
            var poolableObject = Object.Instantiate(_prefab, Vector3.zero, Quaternion.identity, _pool.transform);
            poolableObject.gameObject.SetActive(false);
            _availableObjectsPool.Add(poolableObject);
            poolableObject.OnPoolableObjectDisable += ReturnToPoolable;
        }
        
        private void CreatePoolObjectInstanceList()
        {
            var poolableObject = Object.Instantiate(_prefabListList.GetRandom(), Vector3.zero, Quaternion.identity, _pool.transform);
            poolableObject.gameObject.SetActive(false);
            _availableObjectsPool.Add(poolableObject);
            poolableObject.OnPoolableObjectDisable += ReturnToPoolable;
        }

        private void ReturnToPoolable(PoolableMonobehaviour poolableObject)
        {
            if (_pool == null || poolableObject == null)
                return;
            
            poolableObject.transform.SetParent(_pool.transform);
            _availableObjectsPool.Add(poolableObject);
        }
    }
}
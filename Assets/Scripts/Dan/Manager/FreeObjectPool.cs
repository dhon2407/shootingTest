using UnityEngine;

namespace Dan.Manager
{
    public class FreeObjectPool : MonoBehaviour
    {
        private static FreeObjectPool _instance;

        public static Transform Transform => _instance.transform;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }
    }
}
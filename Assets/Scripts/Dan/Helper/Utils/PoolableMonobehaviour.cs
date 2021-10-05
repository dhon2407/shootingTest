using System;
using UnityEngine;

namespace Dan.Helper.Utils
{
    public class PoolableMonobehaviour : MonoBehaviour
    {
        public event Action<PoolableMonobehaviour> OnPoolableObjectDisable;
        public virtual void OnDisable() => OnPoolableObjectDisable?.Invoke(this);
    }
}
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class PoolObject : MonoBehaviour
    {
        public Guid Id { get; protected set; }
        public float TimeLeft { get; protected set; }


        public virtual void OnCreate()
        {
            gameObject.SetActive(true);
        }

        public virtual void ReturnToPool()
        {
            gameObject.SetActive(false);
        }
    }
}

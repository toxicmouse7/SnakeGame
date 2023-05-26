using System;
using UnityEngine;
using UnityEngine.Events;

namespace Apple
{
    public class Apple : MonoBehaviour
    {
        public readonly UnityEvent Destroyed = new();
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Snake.Snake>() is null) return;
            
            Destroy(gameObject);
            Destroyed.Invoke();
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
    public class SnakeBodyPart : MonoBehaviour
    {
        public Vector2 Direction { get; set; } = Vector2.up;
        public readonly Queue<(Vector2 Point, Vector2 Direction)> MovementBuffer = new();
        public Rigidbody2D Rigidbody2D { get; private set; }

        private void Start()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
        }
    }
}
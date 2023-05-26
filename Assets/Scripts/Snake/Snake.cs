using System;
using System.Collections.Generic;
using System.Linq;
using Controls;
using Extensions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using InputControl = Controls.InputControl;

namespace Snake
{
    [RequireComponent(typeof(SnakeBodyPart))]
    public class Snake : MonoBehaviour
    {
        [FormerlySerializedAs("Speed")] public float speed = 1.5f;
        [FormerlySerializedAs("Body")] public List<SnakeBodyPart> body = new();
        [FormerlySerializedAs("BodyPrefab")] public GameObject bodyPrefab;

        [FormerlySerializedAs("Input Control")] [SerializeField]
        private InputControl inputControl;

        private Keyboard _keyboard;
        private Rigidbody2D _rb;
        private SnakeBodyPart _head;

        private float DistanceDelta => Mathf.Sqrt(speed) * 0.03f;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _head = GetComponent<SnakeBodyPart>();
            inputControl.SwipePerformed.AddListener(ChangeHeadDirection);
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            _rb.MovePosition(_rb.position + _head.Direction * (Time.deltaTime * speed));

            foreach (var part in body)
            {
                var direction = part.Direction;

                if (part.MovementBuffer.Any())
                {
                    var heading = part.MovementBuffer.Peek().Point - part.Rigidbody2D.position;
                    var distance = heading.magnitude;

                    direction = heading / distance;
                    direction.Round();
                }

                part.Rigidbody2D.MovePosition(part.Rigidbody2D.position + direction * (Time.deltaTime * speed));

                if (!part.MovementBuffer.Any() ||
                    !(Vector2.Distance(part.Rigidbody2D.position, part.MovementBuffer.Peek().Point) < DistanceDelta))
                {
                    continue;
                }

                part.Direction = part.MovementBuffer.Peek().Direction;
                part.MovementBuffer.Dequeue();
                AlignBodyPart(body.IndexOf(part));
            }
        }

        private void ChangeHeadDirection(Vector2 direction)
        {
            _head.Direction = direction;

            foreach (var part in body)
            {
                part.MovementBuffer.Enqueue((_rb.position, direction));
            }

            var targetRotation = Quaternion.LookRotation(transform.forward, direction);
            var rotation = Quaternion
                .RotateTowards(transform.rotation, targetRotation, 90);

            _rb.SetRotation(rotation);
        }

        private void AddNewPart()
        {
            var newPart = Instantiate(bodyPrefab,
                body.Last().Rigidbody2D.position - body.Last().Direction,
                body.Last().transform.rotation,
                transform);

            var snakeBodyPart = newPart.GetComponent<SnakeBodyPart>();

            snakeBodyPart.Direction = body.Last().Direction;
            body.Last()
                .MovementBuffer
                .ForEach(point => snakeBodyPart.MovementBuffer.Enqueue(point));

            body.Add(snakeBodyPart);
        }

        private void SpeedUp()
        {
            speed += 0.25f;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var bodyPart = other.GetComponent<SnakeBodyPart>();
            var apple = other.GetComponent<Apple.Apple>();
            
            if (apple is not null)
            {
                AddNewPart();
                SpeedUp();
            }
            
            if (bodyPart is not null && bodyPart != body.First())
            {
                Destroy(gameObject);
            }
        }


        private void AlignBodyPart(int index)
        {
            if (index == 0)
                body[0].Rigidbody2D.MovePosition(_rb.position - _head.Direction);
            else
                body[index].Rigidbody2D.MovePosition(body[index - 1].Rigidbody2D.position - body[index - 1].Direction);
        }
    }
}
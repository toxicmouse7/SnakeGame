using System;
using Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Controls
{
    public class InputControl : MonoBehaviour
    {
        public UnityEvent<Vector2> SwipePerformed { get; private set; }
        private Vector2 _initialPosition;
        private SwipeControl _swipeControl; 

        private Vector2 CurrentPosition => _swipeControl.Touch.Position.ReadValue<Vector2>();

        private void Awake()
        {
            SwipePerformed = new UnityEvent<Vector2>();
            _swipeControl = new SwipeControl();
        }

        private void OnEnable()
        {
            _swipeControl.Enable();
        }

        private void OnDisable()
        {
            _swipeControl.Disable();
        }

        private void Start()
        {
            _swipeControl.Touch.Press.performed += _ => _initialPosition = CurrentPosition;
            _swipeControl.Touch.Press.canceled += _ => DetectSwipe();
        }

        private void DetectSwipe()
        {
            var delta = CurrentPosition - _initialPosition;
            var direction = delta.normalized.Rounded();

            if (direction != Vector2.zero)
                SwipePerformed.Invoke(direction);
        }
    }
}
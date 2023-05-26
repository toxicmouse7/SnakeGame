using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Field
{
    public class Field : MonoBehaviour
    {
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        public Vector2 GetRandomPoint()
        {
            var screenPosition = _camera.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width),
                Random.Range(0, Screen.height), _camera.farClipPlane / 2));

            return screenPosition;
        }
    }
}
using UnityEngine;
using UnityEngine.Serialization;

namespace Apple
{
    public class AppleSpawner : MonoBehaviour
    {
        [FormerlySerializedAs("Field")] public Field.Field field;
        [FormerlySerializedAs("Apple Prefab")] public GameObject applePrefab;

        private void Start()
        {
            SpawnNewApple();
        }

        private void SpawnNewApple()
        {
            var apple = Instantiate(applePrefab,
                field.GetRandomPoint(),
                Quaternion.identity)
                .GetComponent<Apple>();

            apple.Destroyed.AddListener(SpawnNewApple);
        }
    }
}
using UnityEngine;

namespace Extensions
{
    public static class Vector2Extensions
    {
        public static void Round(this Vector2 vector2)
        {
            vector2.x = Mathf.Round(vector2.x);
            vector2.y = Mathf.Round(vector2.y);
        }

        public static Vector2 Rounded(this Vector2 vector2)
        {
            return new Vector2
            {
                x = Mathf.Round(vector2.x),
                y = Mathf.Round(vector2.y)
            };
        }
    }
}
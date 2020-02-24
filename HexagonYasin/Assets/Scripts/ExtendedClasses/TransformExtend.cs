using UnityEngine;

namespace YasinYuce.HexagonYasin
{
    public static class TransformExtend
    {
        public static float GetAngle(this Transform piece, Vector2 pos)
        {
            float xDiff = pos.x - piece.position.x;
            float yDiff = pos.y - piece.position.y;

            float angle = Mathf.Atan2(yDiff, xDiff) * 180.0f / Mathf.PI;
            if (angle < 0) { angle += 360f; }
            return angle;
        }
    }
}
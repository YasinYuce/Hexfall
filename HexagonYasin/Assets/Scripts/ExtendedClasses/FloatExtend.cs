using UnityEngine;

namespace YasinYuce.HexagonYasin
{
    public static class FloatExtend
    {
        public static float GetDifferenceBetweenTwoAngle(this float angle1, float angle2)
        {
            float difference = Mathf.Abs(angle1 - angle2);
            if (difference > 270f)
            {
                if (angle2 > angle1)
                {
                    angle2 -= 360f;
                    return Mathf.Abs(angle1 - angle2);
                }
                else
                {
                    angle1 -= 360f;
                    return Mathf.Abs(angle2 - angle1);
                }
            }
            else
            {
                return difference;
            }
        }
    }
}

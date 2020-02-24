using UnityEngine;

namespace YasinYuce.HexagonYasin
{
    public static class IntExtend
    {
        public static int GetColumn(this int index, int yLength)
        {
            return (index / yLength);
        }

        public static int GetRow(this int index, int yLength)
        {
            return (index % yLength);
        }
    }
}
using UnityEngine;
using System.Collections.Generic;

namespace YasinYuce.HexagonYasin
{
    public static class ListExtend
    {
        public static int FindLowestYIndexOnAMapPieceList(this List<Transform> selecteds)
        {
            float lowestY = 100f;// :(
            int lowestYIndex = 0;
            for (int i = 0; i < selecteds.Count; i++)
            {
                if (selecteds[i].position.y < lowestY)
                {
                    lowestY = selecteds[i].position.y;
                    lowestYIndex = i;
                }
            }
            return lowestYIndex;
        }

        public static bool CheckIfItsHaveAHighestXValue(this List<Transform> selecteds)
        {
            int lowestYIndex = selecteds.FindLowestYIndexOnAMapPieceList();
            for (int i = 0; i < 3; i++)
            {
                if ((selecteds[lowestYIndex].position.x + 0.01f) < selecteds[i].position.x)
                {
                    return false;
                }
            }
            return true;
        }

        public static Vector2 GiveMiddePointOfVectors(this List<Vector2> vectorsToProcess)
        {
            Vector2 point = Vector2.zero;
            int count = vectorsToProcess.Count;
            for (int i = 0; i < count; i++)
            {
                point += vectorsToProcess[i];
            }

            point.x /= count;
            point.y /= count;
            return point;
        }

        public static Vector3 GiveMiddePointOfVectors(this List<Vector3> vectorsToProcess)
        {
            Vector3 point = Vector3.zero;
            int count = vectorsToProcess.Count;
            for (int i = 0; i < count; i++)
            {
                point += vectorsToProcess[i];
            }

            point.x /= count;
            point.y /= count;
            point.z /= count;
            return point;
        }
    }
}
using UnityEngine;
using System.Collections.Generic;

namespace YasinYuce.HexagonYasin
{
    [CreateAssetMenu(fileName = "GameMode01", menuName = "DemoProject/GameMode")]
    public class GameMode : ScriptableObject
    {
        public int GridXLength, GridYLength;
        public List<Color> Colors;
        public int ScoreMultiplier;
        public int BombScoreLimit;

        [Header("For The Future elements")]
        public List<MapElementInfo> GridElements;
        public List<ExplosionTypes> ExplosionTypes;
    }

    [System.Serializable]
    public class MapElementInfo
    {
        public List<GridPiece> GridPieces;
        public GameObject SelectorPrefab;
    }

    public enum ExplosionTypes { TriangleExplosion }
}
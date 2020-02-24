using UnityEngine;

namespace YasinYuce.HexagonYasin
{
    public interface IColored
    {
        int ColorIndex { get; set; }
        
        void ChangeColor(int colorIndex, Color c);
    }
}
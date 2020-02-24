using UnityEngine;

namespace YasinYuce.HexagonYasin
{
    public class HexagonPiece : GridPiece, IColored
    {
        public int ColorIndex { get; set; }

        public void ChangeColor(int colorIndex, Color c)
        {
            ColorIndex = colorIndex;
            GetComponent<SpriteRenderer>().color = c;
        }

        public override void CorrectRotation()
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
    }
}
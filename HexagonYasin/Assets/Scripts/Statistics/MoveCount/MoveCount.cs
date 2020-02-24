using UnityEngine;
using UnityEngine.UI;

namespace YasinYuce.HexagonYasin
{
    public class MoveCount : MonoBehaviour
    {
        public int CurrentMoveCount { get; private set; }

        public Text moveText;

        public void StartGame()
        {
            CurrentMoveCount = 0;
            writeToText();
        }

        public void Moved()
        {
            CurrentMoveCount++;
            writeToText();
        }

        private void writeToText()
        {
            moveText.text = CurrentMoveCount.ToString();
        }
    }
}
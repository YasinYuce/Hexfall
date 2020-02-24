using UnityEngine;

namespace YasinYuce.HexagonYasin
{
    public class Stats : MonoBehaviour
    {
        public Score Score;
        public MoveCount MoveCount;

        public void Initialize(GameManager gameManager)
        {
            Score.Initialize(gameManager);
        }

        public void StartGame(int scoreMultiplier)
        {
            Score.StartGame(scoreMultiplier);
            MoveCount.StartGame();
        }
    }
}
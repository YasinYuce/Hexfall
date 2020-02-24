using System;
using UnityEngine;
using UnityEngine.UI;

namespace YasinYuce.HexagonYasin
{
    public class Score : MonoBehaviour
    {
        public int CurrentScore { get; private set; }
        private int mHighScore;
        public Text ScoreText, HighScoreText;

        private int mScoreMultiplier;
        private const string highScoreString = "highScore";

        private GameManager mGameManager;

        public GameObject MovingTextPrefab;
        private Pool<MovingScoreText> mMovingTextPool;

        public void Initialize(GameManager gameManager)
        {
            mGameManager = gameManager;
            mHighScore = PlayerPrefs.GetInt(highScoreString, 0);
            mMovingTextPool = new Pool<MovingScoreText>(new PrefabFactory<MovingScoreText>(MovingTextPrefab, gameManager.transform));
        }

        public void StartGame(int scoreMultiplier)
        {
            mScoreMultiplier = scoreMultiplier;
            CurrentScore = 0;
            ScoreWriteToText();
            HighScoreWriteToText();
        }

        public void AddScode(int count, Vector3 groupsMiddlePos)
        {
            int addedScore = mScoreMultiplier * count;
            CurrentScore += addedScore;
            if (CurrentScore > mHighScore)
            {
                mHighScore = CurrentScore;
                HighScoreWriteToText();
            }
            ScoreWriteToText();
            TextAnim(addedScore, groupsMiddlePos);
        }

        private void TextAnim(int addedScore, Vector3 groupsMiddlePos)
        {
            MovingScoreText textAnim = mMovingTextPool.Allocate();
            Action action = null;
            action = () => { mMovingTextPool.Release(textAnim); textAnim.Release -= action; };
            textAnim.Release += action;

            textAnim.Move(addedScore, groupsMiddlePos);
        }

        private void ScoreWriteToText()
        {
            ScoreText.text = CurrentScore.ToString();
        }

        private void HighScoreWriteToText()
        {
            PlayerPrefs.SetInt(highScoreString, mHighScore);
            HighScoreText.text = mHighScore.ToString();
        }
    }
}
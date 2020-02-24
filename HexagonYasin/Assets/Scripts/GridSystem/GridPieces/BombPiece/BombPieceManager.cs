using UnityEngine;
using System.Collections.Generic;

namespace YasinYuce.HexagonYasin
{
    public static class BombPieceManager
    {
        private static List<BombPiece> mLiveBombsCheck = new List<BombPiece>();
        private static GridManager mGridManager;
        private static GameManager mGameManager;

        public static void Initialize(GameManager gameManager, GridManager gridManager)
        {
            mGameManager = gameManager;
            mGridManager = gridManager;
        }

        public static void AddBombPiece(BombPiece bombPiece, int startedNumber)
        {
            bombPiece.SetData(startedNumber, Random.Range(5, 7)); 
            mLiveBombsCheck.Add(bombPiece);
            bombPiece.CountDown(bombPiece.NumberToReach - startedNumber);
        }

        public static void RemoveBombPiece(BombPiece bombPiece)
        {
            if (mLiveBombsCheck.Contains(bombPiece))
            {
                mLiveBombsCheck.Remove(bombPiece);
            }
        }

        public static bool IsBombPieceExploded(int currentNumber)
        {
            for (int i = 0; i < mLiveBombsCheck.Count; i++)
            {
                mLiveBombsCheck[i].CountDown(mLiveBombsCheck[i].NumberToReach - currentNumber);
                if ((mLiveBombsCheck[i].NumberToReach) <= currentNumber)
                {
                    mGridManager.ResetAll(true);
                    mGameManager.GameOver();
                    return true;
                }
            }
            return false;
        }
    }
}
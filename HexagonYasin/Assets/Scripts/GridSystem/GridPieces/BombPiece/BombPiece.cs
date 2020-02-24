using System;
using UnityEngine;

namespace YasinYuce.HexagonYasin
{
    public class BombPiece : HexagonPiece
    {
        private int mStartedNumberOfMove, mCountDownAmount;
        public int NumberToReach { get { return mStartedNumberOfMove + mCountDownAmount; } }

        public void CountDown(int leftMoves)
        {
            GetComponentInChildren<TextMesh>().text = leftMoves.ToString();
        }

        private void OnEnable()
        {
            BombPieceManager.AddBombPiece(this, mMoveCount.CurrentMoveCount);
        }

        private void OnDisable()
        {
            BombPieceManager.RemoveBombPiece(this);
        }

        public void SetData(int startedNumber, int movesleft)
        {
            mStartedNumberOfMove = startedNumber;
            mCountDownAmount = movesleft;
        }

        public override void CorrectRotation()
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}
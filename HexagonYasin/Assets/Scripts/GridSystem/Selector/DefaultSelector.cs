using UnityEngine;
using System.Collections;

namespace YasinYuce.HexagonYasin
{
    public abstract class DefaultSelector : MonoBehaviour
    {
        protected GameManager mGameManager;
        protected GridManager mGridManager;
        protected InputManager mInputManager;
        protected SelectorManager mSelectorManager;
        protected MoveCount mMoveCount;

        public virtual void TurnSelectedIndexes(int angleDir) { throw new System.NotImplementedException(); }

        public virtual IEnumerator TurnRoutine(int angleDir) { throw new System.NotImplementedException(); }

        public virtual void RotateSelector(IndexGroup selectedIndexes) { throw new System.NotImplementedException(); }

        public virtual void Initialize(GameManager gameManager, GridManager gridManager, InputManager inputManager, SelectorManager selectorManager, MoveCount moveCount)
        {
            mGameManager = gameManager;
            mGridManager = gridManager;
            mInputManager = inputManager;
            mSelectorManager = selectorManager;
            mMoveCount = moveCount;
        }

        public void StartTurnRoutine(int angleDir)
        {
            StartCoroutine(TurnRoutine(angleDir));
        }
    }
}
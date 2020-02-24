using UnityEngine;
using System.Collections;
using System;

namespace YasinYuce.HexagonYasin
{
    public abstract class GridPiece : MonoBehaviour, IResettable
    {
        protected GridManager mGridManager;
        protected SelectorManager mSelectorManager;
        protected MoveCount mMoveCount;

        public Action Release { get; set; }

        public void Initialize(GridManager gridManager, SelectorManager selectorManager, MoveCount moveCount)
        {
            mGridManager = gridManager;
            mSelectorManager = selectorManager;
            mMoveCount = moveCount;
        }

        public virtual void SelectGroup(Vector3 Pos)
        {
            mSelectorManager.SetSelectorObjectByPieceType(this.GetType());
            mSelectorManager.SelectedPieceGroup = mGridManager.GiveClosestIndexes(this, Pos, 2);
            mSelectorManager.SelectGroup();
        }

        public IEnumerator MoveToPosRoutine(Vector2 posToGo)
        {
            float deltaPos = Vector3.Distance(transform.localPosition, posToGo) > 3.9f ? 0.2f : 0.08f;
            for (int i = 0; i < 80; i++)
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition, posToGo, deltaPos);
                if (Vector3.Distance(transform.localPosition, posToGo) < 0.001f)
                {
                    transform.localPosition = posToGo;
                    yield break;
                }
                yield return new WaitForFixedUpdate();
            }
        }

        public virtual void CorrectRotation()
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        public void Reset()
        {
            gameObject.SetActive(false);
            Release?.Invoke();
        }
    }
}
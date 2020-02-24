using UnityEngine;
using System.Collections.Generic;

namespace YasinYuce.HexagonYasin
{
    public class SelectorManager : MonoBehaviour
    {
        protected GameManager mGameManager;
        protected GridManager mGridManager;
        protected InputManager mInputManager;
        protected SelectorManager mSelectorManager;
        protected MoveCount mMoveCount;

        [HideInInspector] public GameObject CurrentSelectorObject;
        private List<GameObject> mSelectorObjects;

        public IndexGroup SelectedPieceGroup;

        private List<MapElementInfo> mMapElements;

        public void Initialize(GameManager gameManager, GridManager gridManager, InputManager inputManager, SelectorManager selectorManager, MoveCount moveCount)
        {
            mGameManager = gameManager;
            mGridManager = gridManager;
            mInputManager = inputManager;
            mSelectorManager = selectorManager;
            mMoveCount = moveCount;
        }

        public void StartGame(float oneSideScale, List<MapElementInfo> mapElements)
        {
            mMapElements = mapElements;
            SelectedPieceGroup = new IndexGroup();
            mSelectorObjects = new List<GameObject>();
            for (int i = 0; i < mMapElements.Count; i++)
            {
                mSelectorObjects.Add(Instantiate(mMapElements[i].SelectorPrefab, transform));
                mSelectorObjects[i].transform.localScale = new Vector3(oneSideScale, oneSideScale, 1f);
                mSelectorObjects[i].GetComponent<DefaultSelector>().Initialize(mGameManager, mGridManager, mInputManager, mSelectorManager, mMoveCount);
                mSelectorObjects[i].SetActive(false);
            }
        }

        public void SelectGroup()
        {
            Vector2 mPoint = SelectedPieceGroup.MiddlePoint(mGridManager);

            CurrentSelectorObject.transform.position = mPoint;
            CurrentSelectorObject.GetComponent<DefaultSelector>().RotateSelector(SelectedPieceGroup);
            CurrentSelectorObject.SetActive(true);
        }

        public void SetSelectorObjectByPieceType(System.Type piece)
        {
            ResetSelectAction();
            CurrentSelectorObject = mSelectorObjects[mMapElements.FindIndex(x => x.GridPieces.Exists(y => y.GetType() == piece))];
        }

        public void ResetSelectAction()
        {
            SelectedPieceGroup.Indexes.Clear();
            if (CurrentSelectorObject)
            {
                CurrentSelectorObject.SetActive(false);
                CurrentSelectorObject = null;
            }
        }

        public float GiveAngleToCurrentSelector()
        {
            return CurrentSelectorObject.transform.GetAngle(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
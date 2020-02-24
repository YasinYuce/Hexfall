using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace YasinYuce.HexagonYasin
{
    public class TriangleSelector : DefaultSelector
    {
        public override IEnumerator TurnRoutine(int angleDir)
        {
            mInputManager.IsReadyForInput = false;

            List<Transform> selecteds = mSelectorManager.SelectedPieceGroup.GiveGroup(mGridManager);
            InitializeSelecteds(selecteds);

            Vector3 startEuler = mSelectorManager.CurrentSelectorObject.transform.rotation.eulerAngles;

            int indexTurnDir = selecteds.CheckIfItsHaveAHighestXValue() == true ? angleDir : -angleDir;

            for (int i = 0; i < 3; i++)
            {
                for (int x = 0; x < 10; x++)
                {
                    startEuler.z += (12f * angleDir);
                    mSelectorManager.CurrentSelectorObject.transform.rotation = Quaternion.Euler(startEuler);
                    yield return new WaitForFixedUpdate();
                }

                for (int b = 0; b < selecteds.Count; b++)
                {
                    selecteds[b].GetComponent<GridPiece>().CorrectRotation();
                }

                TurnSelectedIndexes(indexTurnDir);

                yield return new WaitForSeconds(0.05f);
                if (ExplosionSystem.IsAnyExplosion)
                {
                    ResetSelecteds(selecteds);
                    mSelectorManager.CurrentSelectorObject.SetActive(false);
                    mMoveCount.Moved();
                    yield return mGameManager.StartCoroutine(ExplosionSystem.Explode());
                    yield break;
                }
            }

            ResetSelecteds(selecteds);
            mInputManager.IsReadyForInput = true;
        }

        private void InitializeSelecteds(List<Transform> selecteds)
        {
            for (int i = 0; i < selecteds.Count; i++)
            {
                selecteds[i].parent = mSelectorManager.CurrentSelectorObject.transform;
                selecteds[i].GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
        }

        private void ResetSelecteds(List<Transform> selecteds)
        {
            for (int i = 0; i < selecteds.Count; i++)
            {
                selecteds[i].parent = mGridManager.transform;
                selecteds[i].GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
        }

        public override void TurnSelectedIndexes(int angleDir)
        {
            IndexGroup tempProcessList = new IndexGroup(mSelectorManager.SelectedPieceGroup);
            tempProcessList.Indexes.Sort();
            GridPiece low = mGridManager.GridMap[tempProcessList.Indexes[0]],
            mid = mGridManager.GridMap[tempProcessList.Indexes[1]],
            high = mGridManager.GridMap[tempProcessList.Indexes[2]];

            if (angleDir == -1)
            {//clockwise
                mGridManager.GridMap[tempProcessList.Indexes[0]] = mid;
                mGridManager.GridMap[tempProcessList.Indexes[1]] = high;
                mGridManager.GridMap[tempProcessList.Indexes[2]] = low;
            }
            else
            {//counter-clockwise
                mGridManager.GridMap[tempProcessList.Indexes[0]] = high;
                mGridManager.GridMap[tempProcessList.Indexes[1]] = low;
                mGridManager.GridMap[tempProcessList.Indexes[2]] = mid;
            }
        }

        public override void RotateSelector(IndexGroup selectedIndexes)
        {
            List<Transform> selecteds = new List<Transform>();
            for (int i = 0; i < selectedIndexes.Indexes.Count; i++)
            {
                selecteds.Add(mGridManager.GridMap[selectedIndexes.Indexes[i]].transform);
            }

            int lowestYIndex = selecteds.FindLowestYIndexOnAMapPieceList();
            Vector2 mPointOf2 = Vector2.zero;
            for (int i = 0; i < selecteds.Count; i++)
            {
                if (i != lowestYIndex)
                {
                    Vector2 tempPos = selecteds[i].position;
                    mPointOf2 += tempPos;
                }
            }
            mPointOf2 /= (selecteds.Count - 1);
            float z = selecteds[lowestYIndex].GetAngle(mPointOf2);
            transform.rotation = Quaternion.Euler(0f, 0f, z);
        }
    }
}
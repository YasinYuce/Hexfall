using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace YasinYuce.HexagonYasin
{
    public static class ExplosionSystem
    {
        private static GameManager mGameManager;
        private static Score mScore;
        private static ParticleManager mParticleManager;
        private static GridManager mGridManager;

        private static List<ExplodeType> mExplodeTypes;
        public static bool IsAnyExplosion { get { return mExplodeTypes.Exists(x => x.IsAnyExplosion == true); } }

        private static int mGridYLength;

        public static void Initialize(GameManager gameManager, Score score, ParticleManager particleManager, GridManager gridManager)
        {
            mGameManager = gameManager;
            mScore = score;
            mParticleManager = particleManager;
            mGridManager = gridManager;
        }

        public static void StartGame(List<ExplosionTypes> types, int gridYLength)
        {
            mGridYLength = gridYLength;
            mExplodeTypes = new List<ExplodeType>();
            for (int i = 0; i < types.Count; i++)
            {
                switch (types[i])
                {
                    case ExplosionTypes.TriangleExplosion:
                        mExplodeTypes.Add(new TriangleExplode(mGridManager));
                        break;
                    default:
                        break;
                }
            }
        }

        public static IEnumerator Explode()
        {
            List<IndexGroup> explodingGroups = GiveEveryGroupThatGoingToExplode();
            VisualProcessInExplodingGroups(explodingGroups);
            yield return mGameManager.StartCoroutine(SpawnPieces(DivideExplodingGroupsInToColumns(explodingGroups)));
            yield return new WaitForSeconds(.1f);
            if (IsAnyExplosion)
            {
                mGameManager.StartCoroutine(Explode());
                yield break;
            }

            mGameManager.GameOverChecks();
        }

        private static IEnumerator SpawnPieces(Dictionary<int, IndexGroup> explodedIndexesInColumn)
        {
            List<List<GridPiece>> movingPiecesInColumns = new List<List<GridPiece>>();

            foreach (var item in explodedIndexesInColumn)
            {
                item.Value.Indexes.Sort();
                int insertIndex = ((item.Key + 1) * mGridYLength);
                Vector2 enTepePos = mGridManager.GridMap[insertIndex - 1].transform.position;

                InsertTopOfColumn(item.Value.Indexes.Count, insertIndex, enTepePos);
                ResetExplodedGridPieces(item.Value);
                movingPiecesInColumns.Add(GiveNeedsToMovesInColumn(item.Value.Indexes[0], insertIndex));
            }
            yield return mGameManager.StartCoroutine(StartMoveOnRows(movingPiecesInColumns));
        }

        private static IEnumerator StartMoveOnColumns(List<GridPiece> needToMove)
        {
            List<Coroutine> lastMovedPieces = new List<Coroutine>();
            for (int i = 0; i < needToMove.Count; i++)
            {
                lastMovedPieces.Add(needToMove[i].StartCoroutine(needToMove[i].MoveToPosRoutine(mGridManager.GivePosition(mGridManager.GridIndexOfaPiece(needToMove[i])))));
                yield return new WaitForSeconds(0.08f);
            }
            for (int i = 0; i < lastMovedPieces.Count; i++)
            {
                yield return lastMovedPieces[i];
            }
        }

        private static IEnumerator StartMoveOnRows(List<List<GridPiece>> allNeedsToMovesInRows)
        {
            List<Coroutine> lastMovedColumns = new List<Coroutine>();
            for (int i = 0; i < allNeedsToMovesInRows.Count; i++)
            {
                lastMovedColumns.Add(mGameManager.StartCoroutine(StartMoveOnColumns(allNeedsToMovesInRows[i])));
            }
            for (int i = 0; i < lastMovedColumns.Count; i++)
            {
                yield return lastMovedColumns[i];
            }
        }

        private static void InsertTopOfColumn(int count, int insertIndex, Vector2 enTepePos)
        {
            for (int i = 0; i < count; i++)
            {
                GridPiece temp = mGridManager.GridPiece();
                temp.transform.position = new Vector2(enTepePos.x, enTepePos.y + (mGridManager.yPlusPos * i) + 5f);
                mGridManager.GridMap.Insert(insertIndex + i, temp);
                mGridManager.SetColor(temp, true);
            }
        }

        private static Dictionary<int, IndexGroup> DivideExplodingGroupsInToColumns(List<IndexGroup> explodingGroups)
        {
            Dictionary<int, IndexGroup> explodedIndexesInColumn = new Dictionary<int, IndexGroup>();
            for (int i = 0; i < explodingGroups.Count; i++)
            {
                for (int b = 0; b < explodingGroups[i].Indexes.Count; b++)
                {
                    int columnIndex = (explodingGroups[i].Indexes[b]).GetColumn(mGridYLength);
                    if (explodedIndexesInColumn.ContainsKey(columnIndex))
                    {
                        explodedIndexesInColumn[columnIndex].Indexes.Add(explodingGroups[i].Indexes[b]);
                    }
                    else
                    {
                        explodedIndexesInColumn.Add(columnIndex, new IndexGroup(explodingGroups[i].Indexes[b]));
                    }
                }
            }
            return explodedIndexesInColumn;
        }

        private static List<GridPiece> GiveNeedsToMovesInColumn(int lowestIndexInColumn, int insertIndex)
        {
            List<GridPiece> needToMove = new List<GridPiece>();
            for (int b = lowestIndexInColumn; b < insertIndex; b++)
            {
                needToMove.Add(mGridManager.GridMap[b]);
            }
            return needToMove;
        }

        private static void VisualProcessInExplodingGroups(List<IndexGroup> explodingGroups)
        {
            for (int i = 0; i < explodingGroups.Count; i++)
            {
                for (int b = 0; b < explodingGroups[i].Indexes.Count; b++)
                {
                    GridPiece tempPiece = mGridManager.GridMap[explodingGroups[i].Indexes[b]];
                    mParticleManager.ShowParticle(tempPiece);
                }
                mScore.AddScode(explodingGroups[i].Indexes.Count, (Vector3)(explodingGroups[i].MiddlePoint(mGridManager)) + Vector3.back);
            }
        }

        private static void ResetExplodedGridPieces(IndexGroup item)
        {
            for (int i = item.Indexes.Count - 1; i >= 0; i--)
            {
                GridPiece temp = mGridManager.GridMap[item.Indexes[i]];
                mGridManager.GridMap.RemoveAt(item.Indexes[i]);
                temp.Reset();
            }
        }

        private static List<IndexGroup> GiveEveryGroupThatGoingToExplode()
        {
            List<IndexGroup> explodingGroups = new List<IndexGroup>();
            if (mExplodeTypes.Count > 0)
                explodingGroups.AddRange(mExplodeTypes[0].GiveEveryGroupThatGoingToExplode());
            for (int i = 1; i < mExplodeTypes.Count; i++)
            {
                List<IndexGroup> temp = mExplodeTypes[i].GiveEveryGroupThatGoingToExplode();
                for (int x = 0; x < temp.Count; x++)
                    if (explodingGroups.Exists(y => y.IsTrianglesHaveAOneOrMoreSameElement(temp[x])))
                    {
                        IndexGroup group = explodingGroups[explodingGroups.FindIndex(y => y.IsTrianglesHaveAOneOrMoreSameElement(temp[x]))];
                        group = group.MergeGroups(temp[x]);
                    }
                    else
                        explodingGroups.Add(temp[x]);
            }
            return explodingGroups;
        }
    }
}
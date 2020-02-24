using System;
using UnityEngine;
using System.Collections.Generic;

namespace YasinYuce.HexagonYasin
{
    public class GridManager : MonoBehaviour
    {
        [HideInInspector] public List<GridPiece> GridMap;

        public List<IndexGroup> EverySelectableTriangle;

        public GameObject HexagonalPiecePrefab, BombPiecePrefab;
        private Pool<GridPiece> HexagonPool;
        private Pool<GridPiece> BombPool;

        private int CreatedBombPieceCount;
        private int mGridXLength, mGridYLength, mBombLimit;
        [HideInInspector] public float yPlusPos, oneSideScale;
        private float OffsetX, xPlusPos, OffsetY;

        private Color[] mColors;

        private GameManager mGameManager;
        private SelectorManager mSelectorManager;
        private Stats mStats;
        private ParticleManager mParticleManager;

        public void Initialize(GameManager gameManager, SelectorManager selectorManager, Stats stats, ParticleManager particleManager)
        {
            mGameManager = gameManager;
            mSelectorManager = selectorManager;
            mStats = stats;
            mParticleManager = particleManager;
            HexagonPool = new Pool<GridPiece>(new PrefabFactory<GridPiece>(HexagonalPiecePrefab, transform));
            BombPool = new Pool<GridPiece>(new PrefabFactory<GridPiece>(BombPiecePrefab, transform));
        }

        public void StartGame(int gridXLength, int gridYLength, int bombLimit, Color[] colors)
        {
            mGridXLength = gridXLength;
            mGridYLength = gridYLength;
            mBombLimit = bombLimit;
            mColors = colors;
            EverySelectableTriangle = new List<IndexGroup>();
            CreatedBombPieceCount = 0;
            CreateGrid();
        }

        private void CreateGrid()
        {
            oneSideScale = mGridYLength < mGridXLength + 2 ? GetOneSideScale(mGridXLength) : GetOneSideScale(mGridYLength);
            SetPrefabScales();

            xPlusPos = oneSideScale / 2f + oneSideScale / 4f;
            yPlusPos = 2f * oneSideScale / 4f * Mathf.Sqrt(3f);

            OffsetY = (mGridYLength * yPlusPos + (yPlusPos / 2f)) * oneSideScale / 2f;
            OffsetX = ((mGridXLength * (3f * oneSideScale / 4f) + oneSideScale / 4f) - oneSideScale) / 2f;

            Vector3 position, rotation = HexagonalPiecePrefab.transform.rotation.eulerAngles;
            GridMap = new List<GridPiece>();
            for (int i = 0; i < mGridXLength; i++)
            {
                for (int b = 0; b < mGridYLength; b++)
                {
                    position = GivePosition((mGridYLength * i) + b);
                    GridPiece gridPiece = GridPiece();
                    GridMap.Add(gridPiece);
                    gridPiece.transform.localPosition = position;
                    gridPiece.transform.rotation = Quaternion.Euler(rotation);
                    SetColor(gridPiece, false);
                }
            }
        }

        private float GetOneSideScale(float xOryLength)
        {
            return 4.8f / (Mathf.Clamp(xOryLength, 7, 100) * 3f * 1f / 4f + 1f / 4f);
        }

        private void SetPrefabScales()
        {
            HexagonalPiecePrefab.transform.localScale = new Vector3(oneSideScale, oneSideScale, 1f);
            BombPiecePrefab.transform.localScale = new Vector3(oneSideScale * 5f / 6f, oneSideScale * 5f / 6f, 1f);
        }

        public GridPiece GridPiece()
        {
            GridPiece piece = null;
            Action action = null;
            if ((CreatedBombPieceCount + 1) <= ((float)mStats.Score.CurrentScore / mBombLimit))
            {
                CreatedBombPieceCount++;
                piece = BombPool.Allocate();
                action = () => { BombPool.Release(piece); piece.Release -= action; };
            }

            if (!piece)
            {
                piece = HexagonPool.Allocate();
                action = () => { HexagonPool.Release(piece); piece.Release -= action; };
            }

            piece.Release += action;
            piece.Initialize(this, mSelectorManager, mStats.MoveCount);
            piece.gameObject.SetActive(true);
            return piece;
        }

        public void ResetAll(bool showParticle)
        {
            for (int x = 0; x < GridMap.Count; x++)
            {
                if (showParticle)
                    mParticleManager.ShowParticle(GridMap[x]);
                GridMap[x].Reset();
            }
        }

        public void SetColor(GridPiece gridPiece, bool random)
        {
            IColored cPiece = gridPiece.GetComponent<IColored>();
            if (cPiece == null)
                return;

            int columnIndex = (GridMap.Count - 1).GetColumn(mGridYLength);
            int rowIndex = (GridMap.Count - 1).GetRow(mGridYLength);
            if (random || (columnIndex == 0) || ((columnIndex % 2 != 0) && (rowIndex == 0)))
            {
                int cI = UnityEngine.Random.Range(0, mColors.Length);
                cPiece.ChangeColor(cI, mColors[cI]);
                return;
            }

            List<IndexGroup> groupHolder = SelectableTriangles(gridPiece);
            int randomColorIndex = UnityEngine.Random.Range(0, mColors.Length);
            cPiece.ColorIndex = randomColorIndex;
            if (groupHolder.CheckColorCount(this, 1) == true)
            {
                for (int i = 0; i < mColors.Length - 1; i++)
                {
                    randomColorIndex++;
                    if (randomColorIndex >= mColors.Length) { randomColorIndex -= mColors.Length; }
                    cPiece.ColorIndex = randomColorIndex;
                    if (groupHolder.CheckColorCount(this, 1) == false)
                        break;
                }
            }
            cPiece.ChangeColor(randomColorIndex, mColors[randomColorIndex]);
            return;
        }

        public bool IsAnyOtherMoveExist()
        {
            if (EverySelectableTriangle.CheckColorCount(this, 2))
            {
                List<IndexGroup> twoSameColored = EverySelectableTriangle.GiveCountOfColoredGroups(this, 2, false);

                for (int i = 0; i < twoSameColored.Count; i++)
                {
                    int differentColorIndex = twoSameColored[i].GridIndexOfADifferentColored(this);
                    int sameColorIndex = twoSameColored[i].ColorIndexOfASameColoreds(this);

                    IndexGroup surroundinsOfDifferentColoreds = SelectableTriangles(GridMap[differentColorIndex]).MergeGroups();
                    int sameColorCount = 0;

                    for (int b = 0; b < surroundinsOfDifferentColoreds.Indexes.Count; b++)
                    {
                        if (sameColorIndex == GridMap[surroundinsOfDifferentColoreds.Indexes[b]].GetComponent<IColored>().ColorIndex)
                        {
                            sameColorCount++;
                        }
                    }

                    if (sameColorCount > 2)
                    {
                        return true;
                    }
                }
            }
            ResetAll(false);
            mGameManager.GameOver();
            return false;
        }

        public Vector2 GivePosition(int index)
        {
            Vector2 position = Vector2.zero;
            int columnIndex = index.GetColumn(mGridYLength), rowIndex = index.GetRow(mGridYLength);
            position.x = columnIndex * xPlusPos - OffsetX;
            position.y = columnIndex % 2 == 0 ? rowIndex * yPlusPos : (rowIndex * yPlusPos - yPlusPos / 2f);
            position.y -= OffsetY;
            return position;
        }

        public int GridIndexOfaPiece(GridPiece piece)
        {
            return GridMap.FindIndex(x => x == piece);
        }

        public List<IndexGroup> SelectableTriangles(GridPiece tempMapPiece)
        {
            List<IndexGroup> groupHolder = new List<IndexGroup>();
            for (int i = 0; i < 6; i++)
            {
                float angle = (45f + 60f * i);
                int xMultiplier = ((angle / 90f > 1) && (angle / 90f < 4)) ? -1 : +1;
                int yMultiplier = (angle / 90f > 2) ? -1 : +1;

                float xOfVector = Mathf.Sin(angle) * xMultiplier / 2f;
                float yOfVector = Mathf.Cos(angle) * yMultiplier / 2f;

                IndexGroup group = GiveClosestIndexes(tempMapPiece, tempMapPiece.transform.position + new Vector3(xOfVector, yOfVector, 0f), 2);
                if (!group.Contains(groupHolder))
                {
                    groupHolder.Add(group);
                    if (!group.Contains(EverySelectableTriangle))
                    {
                        EverySelectableTriangle.Add(group);
                    }
                }
            }
            return groupHolder;
        }

        public IndexGroup GiveClosestIndexes(GridPiece ClickedPiece, Vector3 Pos, int countToGive)
        {
            IndexGroup selectedIndexex = new IndexGroup();

            List<GridPiece> excludedPieces = new List<GridPiece>();
            excludedPieces.Add(ClickedPiece);

            Vector2 mPoint = (new List<Vector2>(new Vector2[] { Pos, ClickedPiece.transform.position })).GiveMiddePointOfVectors();

            for (int i = 0; i < countToGive; i++)
            {
                int closestIndex = 0;
                float closestPoint = 100f;
                for (int b = 0; b < GridMap.Count; b++)
                {
                    if (!excludedPieces.Contains(GridMap[b]))
                    {
                        if (Vector2.Distance(GridMap[b].transform.position, mPoint) < closestPoint)
                        {
                            closestIndex = b;
                            closestPoint = Vector2.Distance(GridMap[b].transform.position, mPoint);
                        }
                    }
                }
                selectedIndexex.Indexes.Add(closestIndex);
                excludedPieces.Add(GridMap[closestIndex]);

                List<Vector2> tempArray = new List<Vector2>(new Vector2[] { Pos, ClickedPiece.transform.position });
                for (int c = 0; c < excludedPieces.Count; c++)
                {
                    tempArray.Add(excludedPieces[c].transform.position);
                }
                mPoint = tempArray.GiveMiddePointOfVectors();
            }

            selectedIndexex.Indexes.Add(GridIndexOfaPiece(ClickedPiece));
            return selectedIndexex;
        }
    }
}
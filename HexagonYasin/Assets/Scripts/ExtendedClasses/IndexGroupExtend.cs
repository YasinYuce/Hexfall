using UnityEngine;
using System.Collections.Generic;
namespace YasinYuce.HexagonYasin
{
    public static class IndexGroupExtend
    {
        public static int ColorIndexOfASameColoreds(this IndexGroup triangle, GridManager gridManager)
        {
            IndexGroup colorsIndexes = new IndexGroup();

            for (int k = 0; k < triangle.Indexes.Count; k++)
            {
                int colorIndex = gridManager.GridMap[triangle.Indexes[k]].GetComponent<IColored>().ColorIndex;
                if (!colorsIndexes.Indexes.Contains(colorIndex))
                {
                    colorsIndexes.Indexes.Add(colorIndex);
                }
                else
                {
                    return colorIndex;
                }
            }
            return -2;
        }

        public static int GridIndexOfADifferentColored(this IndexGroup group, GridManager gridManager)
        {
            IndexGroup colorsIndexes = new IndexGroup(), _group = new IndexGroup(group);

            for (int k = 0; k < group.Indexes.Count; k++)
            {
                int colorIndex = gridManager.GridMap[group.Indexes[k]].GetComponent<IColored>().ColorIndex;
                if (!colorsIndexes.Indexes.Contains(colorIndex))
                {
                    colorsIndexes.Indexes.Add(colorIndex);
                }
                else
                {
                    _group.Indexes.Remove(group.Indexes[k]);
                    for (int i = 0; i < 2; i++)
                    {
                        if (gridManager.GridMap[_group.Indexes[i]].GetComponent<IColored>().ColorIndex == colorIndex)
                        {
                            _group.Indexes.RemoveAt(i);
                            return _group.Indexes[0];
                        }
                    }
                }
            }
            return -2;
        }

        public static bool IsTrianglesHaveASameElement(this IndexGroup firstTriange, IndexGroup secondTriangle)
        {
            for (int i = 0; i < firstTriange.Indexes.Count; i++)
            {
                if (!secondTriangle.Indexes.Contains(firstTriange.Indexes[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsTrianglesHaveAOneOrMoreSameElement(this IndexGroup firstTriange, IndexGroup secondTriangle)
        {
            for (int i = 0; i < firstTriange.Indexes.Count; i++)
            {
                if (secondTriangle.Indexes.Contains(firstTriange.Indexes[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public static IndexGroup MergeGroups(this List<IndexGroup> triangleHolder)
        {
            IndexGroup biggerGroup = new IndexGroup(triangleHolder[0]);
            for (int i = 1; i < triangleHolder.Count; i++)
            {
                biggerGroup = MergeGroups(triangleHolder[i], biggerGroup);
            }
            return biggerGroup;
        }

        public static IndexGroup MergeGroups(this IndexGroup firstTriangle, IndexGroup secondTriangle)
        {
            IndexGroup biggerGroup = new IndexGroup(firstTriangle);
            for (int i = 0; i < secondTriangle.Indexes.Count; i++)
            {
                int index = secondTriangle.Indexes[i];
                if (!biggerGroup.Indexes.Contains(index))
                {
                    biggerGroup.Indexes.Add(index);
                }
            }
            return biggerGroup;
        }


        public static bool Contains(this IndexGroup group, List<IndexGroup> groupHolder)
        {
            for (int k = 0; k < groupHolder.Count; k++)
            {
                if (group.IsTrianglesHaveASameElement(groupHolder[k]))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckColorCount(this List<IndexGroup> triangles, GridManager gridManager, int colorCount)
        {
            for (int i = 0; i < triangles.Count; i++)
            {
                IndexGroup colorsIndexes = new IndexGroup();

                for (int k = 0; k < triangles[i].Indexes.Count; k++)
                {
                    int colorIndex = gridManager.GridMap[triangles[i].Indexes[k]].GetComponent<IColored>().ColorIndex;
                    if (!colorsIndexes.Indexes.Contains(colorIndex))
                    {
                        colorsIndexes.Indexes.Add(colorIndex);
                    }
                }
                if (colorsIndexes.Indexes.Count == colorCount)
                {
                    return true;
                }
            }
            return false;
        }

        public static List<Transform> GiveGroup(this IndexGroup group, GridManager gridManager)
        {
            List<Transform> selecteds = new List<Transform>();
            for (int i = 0; i < group.Indexes.Count; i++)
            {
                Transform tempPieceTransform = gridManager.GridMap[group.Indexes[i]].transform;
                selecteds.Add(tempPieceTransform);
            }
            return selecteds;
        }

        public static Vector2 MiddlePoint(this IndexGroup selecteds, GridManager gridManager)
        {
            List<Vector2> positions = new List<Vector2>();
            for (int i = 0; i < selecteds.Indexes.Count; i++)
            {
                positions.Add(gridManager.GridMap[selecteds.Indexes[i]].transform.position);
            }
            return positions.GiveMiddePointOfVectors();
        }

        public static List<IndexGroup> GiveCountOfColoredGroups(this List<IndexGroup> groupHolder, GridManager gridManager, int colorCount, bool tryMerge)
        {
            List<IndexGroup> givenAmountOfGroups = new List<IndexGroup>();

            for (int i = 0; i < groupHolder.Count; i++)
            {

                IndexGroup colorsIndexes = new IndexGroup();
                for (int k = 0; k < groupHolder[i].Indexes.Count; k++)
                {
                    int colorIndex = gridManager.GridMap[groupHolder[i].Indexes[k]].GetComponent<IColored>().ColorIndex;
                    if (!colorsIndexes.Indexes.Contains(colorIndex))
                    {
                        colorsIndexes.Indexes.Add(colorIndex);
                    }
                }

                if (colorsIndexes.Indexes.Count == colorCount)
                {
                    if (tryMerge)
                    {
                        if (!groupHolder[i].Contains(givenAmountOfGroups))
                        {
                            bool oneChangedToABigger = false;
                            for (int b = 0; b < givenAmountOfGroups.Count; b++)
                            {
                                if (givenAmountOfGroups[b].IsTrianglesHaveAOneOrMoreSameElement(groupHolder[i]))
                                {
                                    oneChangedToABigger = true;
                                    givenAmountOfGroups[b] = givenAmountOfGroups[b].MergeGroups(groupHolder[i]);
                                    break;
                                }
                            }
                            if (!oneChangedToABigger)
                            {
                                givenAmountOfGroups.Add(groupHolder[i]);
                            }
                        }
                    }
                    else
                    {
                        givenAmountOfGroups.Add(groupHolder[i]);
                    }
                    continue;
                }

            }
            return givenAmountOfGroups;
        }
    }
}
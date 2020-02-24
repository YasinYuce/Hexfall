using System.Collections.Generic;

namespace YasinYuce.HexagonYasin
{
    public class TriangleExplode : ExplodeType
    {
        public override bool IsAnyExplosion
        {
            get
            {
                return mGridManager.EverySelectableTriangle.CheckColorCount(mGridManager, 1);
            }
        }

        public override List<IndexGroup> GiveEveryGroupThatGoingToExplode()
        {
            return mGridManager.EverySelectableTriangle.GiveCountOfColoredGroups(mGridManager, 1, true);
        }


        public TriangleExplode(GridManager gridManager) : base(gridManager)
        {
        }
    }
}
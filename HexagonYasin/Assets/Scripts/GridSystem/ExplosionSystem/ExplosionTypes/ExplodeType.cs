using System.Collections.Generic;

namespace YasinYuce.HexagonYasin
{
    public abstract class ExplodeType
    {
        public GridManager mGridManager;

        public virtual bool IsAnyExplosion  { get { throw new System.NotImplementedException(); } }

        public virtual List<IndexGroup> GiveEveryGroupThatGoingToExplode() { throw new System.NotImplementedException(); }

        public ExplodeType(GridManager gridManager)
        {
            mGridManager = gridManager;
        }
    }
}
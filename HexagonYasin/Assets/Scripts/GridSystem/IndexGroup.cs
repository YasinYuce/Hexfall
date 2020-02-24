using System.Collections.Generic;

namespace YasinYuce.HexagonYasin
{
    public class IndexGroup
    {
        public List<int> Indexes;

        public IndexGroup()
        {
            Indexes = new List<int>();
        }

        public IndexGroup(IndexGroup group)
        {
            Indexes = new List<int>(group.Indexes);
        }

        public IndexGroup(int groupElement)
        {
            Indexes = new List<int>();
            Indexes.Add(groupElement);
        }
    }
}
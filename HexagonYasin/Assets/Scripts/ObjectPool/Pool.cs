using System.Collections.Generic;

namespace YasinYuce
{
    public class Pool<T> where T : IResettable
    {
        public List<T> members = new List<T>();
        public HashSet<T> unavailable = new HashSet<T>();
        IFactory<T> factory;

        public Pool(IFactory<T> factory)
        {
            this.factory = factory;
        }

        public T Allocate()
        {
            for (int i = 0; i < members.Count; i++)
            {
                if (!unavailable.Contains(members[i]))
                {
                    unavailable.Add(members[i]);
                    return members[i];
                }
            }
            T newMember = factory.Create();
            members.Add(newMember);
            unavailable.Add(newMember);
            return newMember;
        }

        public void Release(T member)
        {
            unavailable.Remove(member);
        }
    }
}
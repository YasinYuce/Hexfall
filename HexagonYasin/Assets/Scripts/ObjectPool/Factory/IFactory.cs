using UnityEngine;

namespace YasinYuce
{
    public interface IFactory<T>
    {
        T Create();
    }
}
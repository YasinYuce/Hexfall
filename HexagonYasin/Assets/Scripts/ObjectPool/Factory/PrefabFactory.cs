using UnityEngine;

namespace YasinYuce
{
    public class PrefabFactory<T> : IFactory<T> where T : MonoBehaviour
    {
        private GameObject prefab;
        private Transform parent;
        private string name;
        private int index = 0;

        public PrefabFactory(GameObject prefab, Transform parent) : this(prefab, parent, prefab.name) { }

        public PrefabFactory(GameObject prefab, Transform parent, string name)
        {
            this.prefab = prefab;
            this.parent = parent;
            this.name = name;
        }

        public T Create()
        {
            GameObject tempGameObject = GameObject.Instantiate(prefab, parent) as GameObject;
            tempGameObject.name = name + index.ToString();
            T objectOfType = tempGameObject.GetComponent<T>();
            index++;
            return objectOfType;
        }
    }
}
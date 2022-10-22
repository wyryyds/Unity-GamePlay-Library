using UnityEngine;

namespace HAIPoolTool
{
    [System.Serializable]
    public class PoolItem
    {
        public string tag;
        public GameObject objToPool;
        public int countToPool;

        public PoolItem(string tag,GameObject go,int count)
        {
            this.tag = tag;
            objToPool = go;
            countToPool = count;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HAIPoolTool;

public class Test01 : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(getObj());
        PoolManager.Instance.GetPoolObjects("Tile", 15);
        GameObject t = new GameObject();
        PoolManager.Instance.AddToPool("t", t, 100);
        PoolManager.Instance.GetPoolObjects("t", 20);
    }

    IEnumerator getObj()
    {
        int count = 100;
        while (count-- > 0)
        {
            PoolManager.Instance.GetPoolObject("Obs");
            yield return new WaitForSeconds(1f);
        }
    }
}

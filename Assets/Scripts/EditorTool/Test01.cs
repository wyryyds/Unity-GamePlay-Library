using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HAIPoolTool;

public class Test01 : MonoBehaviour
{
    bool flag = false;
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
        int count = 20;
        while (count-- > 0)
        {
            var obs2=PoolManager.Instance.GetPoolObject("Obs");
            obs2.transform.SetParent(transform);

            yield return new WaitForSeconds(1f);
        }
         flag= true;
    }

    private void Update()
    {
        if(flag)
        {
            PoolManager.Instance.RecycleObjectsToPool("Obs");
        }
    }
}

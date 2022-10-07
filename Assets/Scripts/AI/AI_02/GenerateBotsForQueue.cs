using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBotsForQueue : MonoBehaviour
{
    public GameObject botPrefab;
    public int botCount;
    public GameObject target;
    public float minX;
    public float maxX;
    public float minZ;
    public float MaxZ;
    /// <summary>
    /// ¸ß¶È
    /// </summary>
    public float Yvalue;

    private void Start()
    {
        Vector3 spawnPosition;
        GameObject bot;
        for (int i = 0; i < botCount; i++)
        {
            spawnPosition = new Vector3(Random.Range(minX, maxX), Yvalue, Random.Range(minZ, MaxZ));
            bot = Instantiate(botPrefab, spawnPosition, Quaternion.identity);
            bot.GetComponent<Steering_Arrive>().Target = target;
        }
    }

    public void M_DrawGizmos()
    {
        Vector3 centerPos = new Vector3(minX + (maxX - minX) / 2f, Yvalue, minZ + (minZ + MaxZ) / 2f);
        Vector3 size = new Vector3(maxX - minX, 5f, MaxZ - minZ);
        Gizmos.DrawCube(centerPos, size);
    }

}

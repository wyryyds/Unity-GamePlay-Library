using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour
{
    /// <summary>
    ///操控力的权重
    /// </summary>
    public float Weight = 1;
    /// <summary>
    /// 提供操控力
    /// </summary>
    /// <returns></returns>
    public virtual Vector3 Force()
    {
        return Vector3.zero;
    }
}

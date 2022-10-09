using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering_Wander : Steering
{
    /// <summary>
    /// 徘徊半径
    /// </summary>
    public float wanderRadius;
    /// <summary>
    /// 徘徊距离
    /// </summary>
    public float wanderDistance;
    /// <summary>
    /// 每秒加到目标的随即位移的最大值。
    /// </summary>
    public float wanderJitter;

    public bool isPlaner;

    private Vector3 desiredVelocity;
    private Vehicle vehicle;
    private float maxSpeed;
    private Vector3 circleTarget;
    private Vector3 wanderTarget;

    private void Start()
    {
        vehicle = GetComponent<Vehicle>();
        maxSpeed = vehicle.MaxSpeed;
        isPlaner = vehicle.IsPlaner;
        circleTarget = new Vector3(wanderRadius * 0.707f, 0, wanderRadius * 0.707f);
    }

    public override Vector3 Force()
    {
        Vector3 randomDisplacement = new Vector3((Random.value - 0.5f) * 2 * wanderJitter,
                                                 (Random.value - 0.5f) * 2 * wanderJitter,
                                                 (Random.value - 0.5f) * 2 * wanderJitter);
        if (isPlaner) randomDisplacement.y = 0;
        //将随机位移加到初始点上，得到新的变量。
        circleTarget += randomDisplacement;
        //新位置可能不在圆周上，进行投影。
        circleTarget = wanderDistance * circleTarget.normalized;
        //计算的值是相对于AI的前进方向，需要转换为世界坐标
        wanderTarget = vehicle.Velocity.normalized * wanderDistance + circleTarget + transform.position;

        desiredVelocity = (wanderTarget - transform.position).normalized * maxSpeed;
        return (desiredVelocity - vehicle.Velocity);
    }
   
}

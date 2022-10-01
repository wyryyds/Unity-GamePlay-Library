using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    /// <summary>
    /// 质量
    /// </summary>
    public float Mass = 1f;

    /// <summary>
    /// 行为列表
    /// </summary>
    public Steering[] Steerings;

    /// <summary>
    /// 最大速度
    /// </summary>
    [Range(5f,20f)]public float MaxSpeed;

    /// <summary>
    /// 最大力
    /// </summary>
    [Range(20f,100f)]public float MaxForce;

    /// <summary>
    /// 计算操纵力(三维向量)
    /// </summary>
    private Vector3 steeringForce;

    /// <summary>
    /// 速度(三维向量)
    /// </summary>
    public Vector3 Velocity;

    /// <summary>
    /// 加速度(三维向量)
    /// </summary>
    public Vector3 acceleration;

    private float timer;

    /// <summary>
    /// 更新操作力的时间间隔，由于一帧的时间非常短，我们并不需要在那么短的时间内去实时更新我们的操纵力，同时避免过小的更新导致的浮点数误差。
    /// </summary>
    public float ComputeInterval = 0.2f;

    /// <summary>
    /// 缓存最大速度的平方,避免反复计算平方的开销
    /// </summary>
    protected float sqrMaxspeed;

    /// <summary>
    /// 是否只在地面上
    /// </summary>
    public bool IsPlaner;

    /// <summary>
    /// 控制转向的速度
    /// </summary>
    public float damping=0.9f;
    protected virtual void Start()
    {
        steeringForce = Vector3.zero;
        Steerings = GetComponents<Steering>();
        timer = 0;
        sqrMaxspeed = MaxSpeed * MaxSpeed;
    }
    public void Update()
    {
        timer += Time.deltaTime;
        steeringForce = Vector3.zero;

        if(timer>ComputeInterval)
        {
            foreach(var _steering in Steerings)
            {
                if(_steering.enabled)
                steeringForce += _steering.Force() * _steering.Weight;
            }
            //避免超过最大力，限制大小。
            steeringForce = Vector3.ClampMagnitude(steeringForce, MaxForce);
            //物理计算
            acceleration = steeringForce / Mass;
            timer = 0;
        }
    }

}

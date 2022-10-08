using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering_Pursuit : Steering
{
    public GameObject target;
    private Vector3 desireVelocity;
    private Vehicle vehicle;
    private float maxSpeed;

    private void Start()
    {
        vehicle = GetComponent<Vehicle>();
        maxSpeed = vehicle.MaxSpeed;
    }

    public override Vector3 Force()
    {
        Vector3 toTarget = target.transform.position - transform.position;
        //计算追逐者与猎物之间的夹角
        float relativeDirection = Vector3.Dot(transform.forward, target.transform.forward);
        //如果夹角大于0并且追逐者基本上面对逃避者(点积-1为完全面对)，那么直接向逃避者方向前进。
        if(Vector3.Dot(toTarget,transform.forward)>0&&relativeDirection<-0.95f)
        {
            desireVelocity = (target.transform.position - transform.position).normalized * maxSpeed;
            return desireVelocity - vehicle.Velocity;
        }
        //计算预期时间，正比于追逐者与逃避者的距离，反比于追逐者与逃避者的速度和
        float lookaheadTime = toTarget.magnitude / (maxSpeed + target.GetComponent<Vehicle>().Velocity.magnitude);

        desireVelocity = (target.transform.position + target.GetComponent<Vehicle>().Velocity * lookaheadTime - transform.position).normalized * maxSpeed;
        return desireVelocity - vehicle.Velocity;
    }
}

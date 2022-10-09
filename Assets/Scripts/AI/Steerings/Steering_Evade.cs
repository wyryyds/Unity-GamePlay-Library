using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering_Evade : Steering
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

        float lookAheadTime = toTarget.magnitude / (maxSpeed + target.GetComponent<Vehicle>().Velocity.magnitude);

        //Ô¤ÆÚËÙ¶È
        desireVelocity = (transform.position - (target.transform.position + target.GetComponent<Vehicle>().Velocity * lookAheadTime).normalized * maxSpeed);

        return desireVelocity - vehicle.Velocity;

    }

}

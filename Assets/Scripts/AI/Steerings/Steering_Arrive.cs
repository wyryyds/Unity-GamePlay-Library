using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering_Arrive : Steering
{
    public bool isPlaner;
    public float ArrivalDistance=0.3f;

    public float SlowDownDistance;
    public GameObject Target;

    private Vector3 desiredVelocity;
    private Vehicle vehicle;
    private float maxSpeed;

    private void Start()
    {
        vehicle = GetComponent<Vehicle>();
        isPlaner = vehicle.IsPlaner;
        maxSpeed = vehicle.MaxSpeed;
    }

    public override Vector3 Force()
    {
        Vector3 toTarget = Target.transform.position - transform.position;

        if (isPlaner) toTarget.y = 0;

        float distance = toTarget.magnitude;

        desiredVelocity = distance > SlowDownDistance ? toTarget.normalized * maxSpeed : toTarget - vehicle.Velocity;

        return desiredVelocity - vehicle.Velocity;
    }
}

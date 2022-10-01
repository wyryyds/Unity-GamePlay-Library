using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILocomotion : Vehicle
{
    private Rigidbody _rigidbody;
    private CharacterController controller;
    /// <summary>
    /// 移动的距离
    /// </summary>
    private Vector3 moveDistance;

     protected override void Start()
    {
        controller = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody>();

        moveDistance = Vector3.zero;
        base.Start();
    }

    private void FixedUpdate()
    {
        Velocity += acceleration * Time.fixedDeltaTime;

        //限制最大速度
        Velocity = Velocity.sqrMagnitude > sqrMaxspeed ? Velocity.normalized * MaxSpeed : Velocity;
        if (acceleration.sqrMagnitude == 0) Velocity *= 0.9f;
        moveDistance = Velocity * Time.fixedDeltaTime;
        //是否只在平面上运动，是就把y轴置0。
        if(IsPlaner)
        {
            Velocity.y = 0;
            moveDistance.y = 0;
        }
        //如果有角色控制器，采取角色控制器来移动
        if(controller)
        {
            controller.SimpleMove(Velocity);
        }
        //没有角色控制器，没有刚体或者是静态刚体
        else if(_rigidbody==null||_rigidbody.isKinematic)
        {
            transform.position += moveDistance;
        }
        //刚体动力学方式运动
        else
        {
            _rigidbody.MovePosition(_rigidbody.position + moveDistance);
        }

        //更新朝向
        if(Velocity.sqrMagnitude>1e-5)
        {
            Vector3 newForward = Vector3.Slerp(transform.forward, Velocity, damping);
            if (IsPlaner) newForward.y = 0;

            transform.forward = newForward;
        }
        //TODO 动画等等.....
    }
}

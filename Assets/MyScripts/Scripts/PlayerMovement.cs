using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("移动速度")]
    public float speed=12f;

    [Header("重力加速度")]
    public float gravity = -9.81f;

    [Header("地面检测")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask GroundMask;

    [Header("跳跃高度")]
    public float jumpHeight = 3f;

    private CharacterController controller;

    private Vector3 velocity;
    private bool isGround;
   
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        movement();
    }
    void movement()
    {
        //x跟z轴移动：
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z; //根据角色的朝向进行基于x轴与z轴的移动   

        //Vector3 move =new  Vector3(x, 0f, z); //错误的赋值。
        /*这里赋值的的 vector3 是面对整个世界坐标系的，无论角色面朝任何方向，都只会在世界坐标系的x轴跟z轴上移动。*/

        controller.Move(move * speed * Time.deltaTime);


        //考虑重力的y轴移动：
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime );

        isGround = Physics.CheckSphere(groundCheck.position, groundDistance, GroundMask);

        if (isGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //跳跃
        if (Input.GetButtonDown("Jump") && isGround)
        {
            velocity.y = Mathf.Sqrt(-2 * jumpHeight * gravity); //v=sqrt（2gh）；
        }
    }
}

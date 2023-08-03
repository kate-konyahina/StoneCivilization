using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] JoystickMovement joystick;
    [SerializeField] float speed;
    private Rigidbody2D rb;
    private Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    { if(joystick.joystickVec != Vector2.zero)
        {
            rb.velocity = new Vector2(joystick.joystickVec.x * speed, joystick.joystickVec.y * speed);
            animator.SetFloat("X", joystick.joystickVec.x);
            animator.SetFloat("Y", joystick.joystickVec.y);
        }
        else
        {
            rb.velocity = Vector2.zero;
            animator.SetFloat("X", 0);
            animator.SetFloat("Y", 0);
        }
        
    }
}

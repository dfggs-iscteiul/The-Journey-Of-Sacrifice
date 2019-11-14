using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rigidbody;
    Vector3 change;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (change != Vector3.zero)
        {
            move();
        }
        else
        {
            animator.SetBool("moving", false);
        }
        
       
    }

    void move()
    {
        rigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
        animator.SetFloat("x", change.x);
        animator.SetFloat("y", change.y);
        animator.SetBool("moving", true);
    }
}

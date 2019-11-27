using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk,
    attack,
    interact
}


public class HeroMovement : MonoBehaviour

{
    public PlayerState currentState;
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
        if (Input.GetButtonDown("Attack") & currentState!=PlayerState.attack)
        {
            StartCoroutine(AttackCo());

        }
        else if (change != Vector3.zero & currentState==PlayerState.walk)
        {
            move();
        }
        
        else
        {
            animator.SetBool("moving", false);
        }
        
       
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.33f);
        currentState = PlayerState.walk;
    }

    void move()
        {
            rigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
            animator.SetFloat("x", change.x);
            animator.SetFloat("y", change.y);
            animator.SetBool("moving", true);
        }

    
}

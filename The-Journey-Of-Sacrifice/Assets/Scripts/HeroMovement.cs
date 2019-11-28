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

    public int actualDamage;
    public float multiplier = 1f;
    public int maxHealth;
    public int health;

    public GameObject targetGO;

    bool wasAttacked = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        targetGO = GameObject.FindWithTag("enemy");
        health = maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        Debug.Log("1 - "+currentState);
        Debug.Log(Vector3.Distance(targetGO.GetComponent<Enemy>().transform.position, transform.position));
        Debug.Log(targetGO.GetComponent<Enemy>().attackRadius);
        Debug.Log(targetGO.GetComponent<Enemy>().enemyState);

        if (currentState != PlayerState.attack && Vector3.Distance(targetGO.GetComponent<Enemy>().transform.position, transform.position) <= targetGO.GetComponent<Enemy>().attackRadius && targetGO.GetComponent<Enemy>().enemyState == Enemy.EnemyState.attack && !wasAttacked)
        {
            TakeDamage(targetGO.GetComponent<Enemy>().damage);
            wasAttacked = true;
        }
        else if (Input.GetButtonDown("Attack") & currentState!=PlayerState.attack)
        {
            StartCoroutine(AttackCo());
            wasAttacked = false;
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
        yield return null;
        currentState = PlayerState.attack;
        generateDamage();
        animator.SetBool("attacking", false);
        currentState = PlayerState.walk;
    }

    void move()
        {
            rigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
            animator.SetFloat("x", change.x);
            animator.SetFloat("y", change.y);
            animator.SetBool("moving", true);
        }

    private void generateDamage()
    {
        float p = 0f;
        {
                Random r = new Random();
                float p1 = Random.Range(-1f,1f) * 100f / 100f;
                float p2 = Random.Range(-1f, 1f) * 100f / 100f;
                p = p1 * p1 + p2 * p2;
                if (p < 1)
                {
                    actualDamage=(int)((50 + 8 * p1 * Mathf.Sqrt(-2 * Mathf.Log(p) / p))*multiplier);
                }
        }
    }

   private void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }


}

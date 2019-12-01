using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public Animator transitionAnim;
    public string sceneToLoad;

    public int actualDamage;
    public float multiplier = 1f;
    public int maxHealth;
    public int health;

    public GameObject targetGO;
    public GameObject[] targetsGO;

    bool wasAttacked = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        targetsGO = GameObject.FindGameObjectsWithTag("enemy");

        if (targetsGO.Length == 1)
            targetGO = targetsGO[0];
        else
        {
            targetGO = targetsGO[0];
            for (int i = 1; i < targetsGO.Length; i++)
            {
                if (Vector3.Distance(transform.position, targetsGO[i].transform.position) < Vector3.Distance(transform.position, targetGO.transform.position))
                {
                    targetGO = targetsGO[i];
                }
            }
        }
        
            if (targetGO.GetComponent<Parcas>() != null && targetGO != null && currentState != PlayerState.attack && Vector3.Distance(targetGO.GetComponent<Parcas>().transform.position, transform.position) <= targetGO.GetComponent<Parcas>().attackRadius && targetGO.GetComponent<Parcas>().enemyState == Parcas.EnemyState.attack && !wasAttacked)
            {
                TakeDamage(targetGO.GetComponent<Parcas>().baseAttack);
                wasAttacked = true;
            } 
       
            else if (targetGO != null && currentState != PlayerState.attack && Vector3.Distance(targetGO.GetComponent<Enemy>().transform.position, transform.position) <= targetGO.GetComponent<Enemy>().attackRadius && targetGO.GetComponent<Enemy>().enemyState == Enemy.EnemyState.attack && !wasAttacked)
            {
                TakeDamage(targetGO.GetComponent<Enemy>().baseAttack);
                wasAttacked = true;
            }
   
        else if (targetGO!= null && Input.GetButtonDown("Attack") & currentState!=PlayerState.attack)
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
            StartCoroutine(LoadScene());
        }
    }

    IEnumerator LoadScene()
    {
        transitionAnim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneToLoad);
        yield return new WaitForSeconds(3.5f);
        transitionAnim.SetTrigger("FadeIn");
    }


}

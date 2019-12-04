using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class HeroMovement1 : MonoBehaviour

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

    bool wasAttacked = false;

    public GameObject fire;
    public GameObject water;
    public GameObject rock;
    public GameObject leaf;

    public int specialAttack;

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
        
        if (change != Vector3.zero & currentState==PlayerState.walk)
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
        yield return null;
        currentState = PlayerState.attack;
        animator.SetBool("attacking", true);
        animator.SetBool("special", true);
        generateDamage();
        animator.SetBool("attacking", false);
        animator.SetBool("special", true);
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
            int special = (int)Mathf.Floor(1 + 4 * Random.Range(0f, 1f));
                if (special == 1) 
            {
                if (specialAttack == 1)
                {

                    Instantiate(leaf, transform.position, Quaternion.identity);
                    actualDamage += 50;

                }
                else if (specialAttack == 2)
                {
                    Instantiate(fire, transform.position, Quaternion.identity);
                    actualDamage += 50;

                }
                else if (specialAttack == 3)
                {
                    Instantiate(water, transform.position, Quaternion.identity);
                    actualDamage += 50;

                }
                else if (specialAttack == 4)
                {
                    Instantiate(rock, transform.position, Quaternion.identity);
                    actualDamage += 50;

                }
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

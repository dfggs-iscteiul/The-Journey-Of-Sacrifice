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
        targetsGO = GameObject.FindGameObjectsWithTag("enemy");

        if (targetsGO.Length == 1)
        {
             targetGO = targetsGO[0];

            if (targetGO.GetComponent<Parcas>() != null && targetGO != null && currentState != PlayerState.attack && Vector3.Distance(targetGO.GetComponent<Parcas>().transform.position, transform.position) <= targetGO.GetComponent<Parcas>().attackRadius && targetGO.GetComponent<Parcas>().enemyState == Parcas.EnemyState.attack && !wasAttacked)
            {
                TakeDamage(targetGO.GetComponent<Parcas>().baseAttack);
                wasAttacked = true;
            }

            else if (targetGO.GetComponent<Parcas>() == null && targetGO != null && currentState != PlayerState.attack && Vector3.Distance(targetGO.GetComponent<Enemy>().transform.position, transform.position) <= targetGO.GetComponent<Enemy>().attackRadius && targetGO.GetComponent<Enemy>().enemyState == Enemy.EnemyState.attack && !wasAttacked)
            {
                TakeDamage(targetGO.GetComponent<Enemy>().baseAttack);
                wasAttacked = true;
            }

            else if (targetGO != null && Input.GetButtonDown("Attack") & currentState != PlayerState.attack)
            {
                animator.SetBool("attacking", true);
                StartCoroutine(AttackCo());
                wasAttacked = false;
            }
            else if (change != Vector3.zero & currentState == PlayerState.walk)
            {
                move();
            }
            else
            {
                animator.SetBool("moving", false);
            }
        }
           
        else if(targetsGO.Length == 0)
        {
            targetGO = null;

            if (change != Vector3.zero & currentState == PlayerState.walk)
            {
                move();
            }
            else
            {
                animator.SetBool("moving", false);
            }
        }

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

            if (targetGO.GetComponent<Parcas>() != null && targetGO != null && currentState != PlayerState.attack && Vector3.Distance(targetGO.GetComponent<Parcas>().transform.position, transform.position) <= targetGO.GetComponent<Parcas>().attackRadius && targetGO.GetComponent<Parcas>().enemyState == Parcas.EnemyState.attack && !wasAttacked)
            {
                TakeDamage(targetGO.GetComponent<Parcas>().baseAttack);
                wasAttacked = true;
            }

            else if (targetGO.GetComponent<Parcas>() == null && targetGO != null && currentState != PlayerState.attack && Vector3.Distance(targetGO.GetComponent<Enemy>().transform.position, transform.position) <= targetGO.GetComponent<Enemy>().attackRadius && targetGO.GetComponent<Enemy>().enemyState == Enemy.EnemyState.attack && !wasAttacked)
            {
                TakeDamage(targetGO.GetComponent<Enemy>().baseAttack);
                wasAttacked = true;
            }

            else if (targetGO != null && Input.GetButtonDown("Attack") & currentState != PlayerState.attack)
            {
                animator.SetBool("attacking", true);
                StartCoroutine(AttackCo());
                wasAttacked = false;
            }
            else if (change != Vector3.zero & currentState == PlayerState.walk)
            {
                move();
            }
            else
            {
                animator.SetBool("moving", false);
            }
        }

        
       
    }

    private IEnumerator AttackCo()
    {
        yield return null;
        currentState = PlayerState.attack;
        generateDamage();
        yield return null;
        animator.SetBool("attacking", false);
        animator.SetBool("special", false); ;
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
                animator.SetBool("special", true);
                
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
        var loading = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        yield return loading;
        int sp = GameObject.FindWithTag("Player").GetComponent<HeroMovement>().specialAttack;
        int lf = GameObject.FindWithTag("Player").GetComponent<HeroMovement>().maxHealth;
        float mt = GameObject.FindWithTag("Player").GetComponent<HeroMovement>().multiplier;
        int dm = GameObject.FindWithTag("Player").GetComponent<HeroMovement>().actualDamage;


        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));

        GameObject.FindWithTag("Player").GetComponent<HeroMovement>().sceneToLoad = "sceneToLoad";
        GameObject.FindWithTag("Player").GetComponent<HeroMovement>().specialAttack = sp;
        GameObject.FindWithTag("Player").GetComponent<HeroMovement>().maxHealth = lf;
        GameObject.FindWithTag("Player").GetComponent<HeroMovement>().multiplier = mt;
        GameObject.FindWithTag("Player").GetComponent<HeroMovement>().actualDamage = dm;

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
        transitionAnim.SetTrigger("FadeIn");
    }

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }


}

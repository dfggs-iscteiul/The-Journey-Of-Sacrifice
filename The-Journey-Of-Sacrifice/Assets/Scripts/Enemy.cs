using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour{

    public enum EnemyState
    {
        idle,
        walk,
        attack,
        dead
    }

    public EnemyState enemyState;
    public int maxHealth;
    public float health;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;

    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;

    public GameObject targetGO;

    public Animator transitionAnim;
    public string sceneToLoad;

    private bool load = false;

    public bool sp = false;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        enemyState = EnemyState.idle;
        target = GameObject.FindWithTag("Player").transform;
        targetGO = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        targetGO = GameObject.FindWithTag("Player");
        target = GameObject.FindWithTag("Player").transform;
        checkDistance();
        if (health <= 0 && !load)
        {
            StartCoroutine(LoadScene());
            load = true;
        }
    }

    void checkDistance()
    {
        if(Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            enemyState = EnemyState.walk;
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed*Time.deltaTime);

        }
        else if (Vector3.Distance(target.position, transform.position) <= attackRadius && targetGO.GetComponent<Animator>().GetBool("attacking"))
        {  
            TakeDamage((int)targetGO.GetComponent<HeroMovement>().actualDamage);
        }
        else if (sp == true)
        {
            TakeDamage((int)targetGO.GetComponent<HeroMovement>().actualDamage);
            sp = false;
        }
        else if (Vector3.Distance(target.position, transform.position) <= attackRadius)
        {
            enemyState = EnemyState.attack;
        }
        else
        {
            enemyState = EnemyState.idle;
        }
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
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
        Debug.Log(sp);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));

        GameObject.FindWithTag("Player").GetComponent<HeroMovement>().sceneToLoad = sceneToLoad;
        GameObject.FindWithTag("Player").GetComponent<HeroMovement>().specialAttack = sp;
        GameObject.FindWithTag("Player").GetComponent<HeroMovement>().maxHealth = lf;
        GameObject.FindWithTag("Player").GetComponent<HeroMovement>().multiplier = mt;
        GameObject.FindWithTag("Player").GetComponent<HeroMovement>().actualDamage = dm;

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
        transitionAnim.SetTrigger("FadeIn");
    }
}

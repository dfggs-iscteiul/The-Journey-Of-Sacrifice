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
        if(health<=0)
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
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
        transitionAnim.SetTrigger("FadeIn");
    }
}

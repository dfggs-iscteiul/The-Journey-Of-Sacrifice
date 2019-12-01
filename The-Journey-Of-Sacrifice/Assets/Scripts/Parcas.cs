using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Parcas : MonoBehaviour{

    public enum EnemyState
    {
        idle,
        walk,
        attack,
        dead
    }

    public string name;

    public float maxY;
    public float minY;

    public EnemyState enemyState;
    public int maxHealth;
    public float health;
    public int baseAttack;
    public float moveSpeed;

    public Transform target;
    public float chaseRadius;
    public float attackRadius;

    public GameObject targetGO;

    public Animator transitionAnim;

    public GameObject minion;

    private int time;

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
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius && targetGO.transform.position.y < maxY && targetGO.transform.position.y > minY)
        {
            enemyState = EnemyState.walk;
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

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

        if (name == "Laquesis")
        {
            if (time == 0 && GameObject.FindGameObjectsWithTag("enemy").Length < 6 && Vector3.Distance(transform.position,target.position) <= chaseRadius) { 
            float A = 1 / Mathf.Sqrt(2 * 2 - 1);
            float B = 2 - Mathf.Log(4);
            float Q = 2 + 1 / A;
            float T = 4.5f;
            float D = 1 + Mathf.Log(T);

            float p1 = Random.Range(0.0f, 1.0f);
            float p2 = Random.Range(0.0f, 1.0f);
            float v = A * Mathf.Log(p1 / (1 - p1));
            float y = 2 * Mathf.Exp(v);
            float z = p1 * p1 * p2;
            float w = B + Q * v - y;

                if (w + D - T * z >= 0 || w >= Mathf.Log(z))
                {
                    time = Mathf.RoundToInt(Mathf.Floor(0 + 1 * y))*30;
                    Instantiate(minion,transform.position,Quaternion.identity);
                }
            }
            else if(Vector3.Distance(transform.position, target.position) <= chaseRadius && time>0)
            {
                time--;
            }
        }
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        if(health<=0)
        {
            if (name == "Minion")
            {
                Destroy(transform.root.gameObject);
            }
            else 
            { 
                GameObject.Find(name).SetActive(false);
                if (GameObject.FindWithTag("enemy") == null)
                {
                    StartCoroutine(LoadScene());
                }
            }
        }
    }

    IEnumerator LoadScene()
    {
        transitionAnim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.5f);
        var loading = SceneManager.LoadSceneAsync("ParcasDefeated", LoadSceneMode.Additive);
        yield return loading;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("ParcasDefeated"));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
        transitionAnim.SetTrigger("FadeIn");
    }
}

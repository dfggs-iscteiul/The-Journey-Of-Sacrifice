using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour{

    public enum EnemyState
    {
        idle,
        walk,
        attack,
        dead
    }

    public EnemyState enemyState;
    public FloatValue maxHealth;
    public float health;
    public int extraHealth;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;

    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;

    public GameObject targetGO;

    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth.initialValue;
        enemyState = EnemyState.idle;
        target = GameObject.FindWithTag("Player").transform;
        targetGO = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
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
            this.gameObject.SetActive(false);
        }
    }
}

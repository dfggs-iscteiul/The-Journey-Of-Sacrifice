using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{

    public float speed;
    private GameObject targetGO;
    private Rigidbody2D myRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        targetGO = GameObject.FindWithTag("Player").GetComponent<HeroMovement>().targetGO;
    }

    // Update is called once per frame
    void Update()
    {
        targetGO = GameObject.FindWithTag("Player").GetComponent<HeroMovement>().targetGO;
        myRigidBody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (targetGO.GetComponent<Enemy>()!=null && Vector3.Distance(transform.position, targetGO.transform.position) <= targetGO.GetComponent<Enemy>().attackRadius)
        {
            targetGO.GetComponent<Enemy>().sp = true;
            Destroy(gameObject);
        }
        else if (targetGO.GetComponent<Parcas>() != null && Vector3.Distance(transform.position, targetGO.transform.position) <= targetGO.GetComponent<Parcas>().attackRadius)
        {
            targetGO.GetComponent<Parcas>().sp = true;
            Destroy(gameObject);
        }
        else
        {
            Vector2 direction = targetGO.transform.position - transform.position;
            myRigidBody.velocity = direction.normalized * speed;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    }

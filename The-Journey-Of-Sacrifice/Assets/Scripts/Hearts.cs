using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearts : MonoBehaviour
{

    public GameObject heart;
    private GameObject heart1;
    private GameObject heart2;
    private GameObject heart3;
    private GameObject heart4;
    private GameObject heart5;

    private Camera camera;

    private GameObject hero;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;

        hero = GameObject.FindGameObjectWithTag("Player");
        hero.GetComponent<HeroMovement>().health = hero.GetComponent<HeroMovement>().maxHealth;

        heart1 = Instantiate(heart, Camera.main.ScreenToWorldPoint(new Vector3(50, camera.pixelHeight - 50),0), Quaternion.identity);
        heart1.SetActive(true);
        heart2 = Instantiate(heart, Camera.main.ScreenToWorldPoint(new Vector3(100, camera.pixelHeight - 50),0), Quaternion.identity);
        heart2.SetActive(true);
        heart3 = Instantiate(heart, Camera.main.ScreenToWorldPoint(new Vector3(150, camera.pixelHeight - 50),0), Quaternion.identity);
        heart3.SetActive(true);
        heart4 = Instantiate(heart, Camera.main.ScreenToWorldPoint(new Vector3(200, camera.pixelHeight - 50),0), Quaternion.identity);
        heart4.SetActive(true);
        heart5 = Instantiate(heart, Camera.main.ScreenToWorldPoint(new Vector3(250, camera.pixelHeight - 50),0), Quaternion.identity);
        heart5.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        camera = Camera.main;

        hero = GameObject.FindGameObjectWithTag("Player");

        int currentHealth = hero.GetComponent<HeroMovement>().health;

        int max = hero.GetComponent<HeroMovement>().maxHealth;
        int section = hero.GetComponent<HeroMovement>().maxHealth / 5;

        heart1.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(50, camera.pixelHeight - 50),0);
        heart2.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(100, camera.pixelHeight - 50),0);
        heart3.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(150, camera.pixelHeight - 50),0);
        heart4.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(200, camera.pixelHeight - 50),0);
        heart5.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(250, camera.pixelHeight - 50),0);

        if (currentHealth<=max-section && currentHealth > max - (2 * section))
        {
            heart5.SetActive(false);
        }
        else if (currentHealth <= max - (2 * section) && currentHealth > max - (3 * section))
        {
            heart4.SetActive(false);
        }
        else if (currentHealth <= max - (3 * section) && currentHealth > max - (4 * section))
        {
            heart3.SetActive(false);
        }
        else if (currentHealth <= max - (4 * section) && currentHealth > 0)
        {
            heart2.SetActive(false);
        }
        else if (currentHealth <= 0)
        {
            heart1.SetActive(false);
            hero.GetComponent<HeroMovement>().health = hero.GetComponent<HeroMovement>().maxHealth;
        }

    }
}

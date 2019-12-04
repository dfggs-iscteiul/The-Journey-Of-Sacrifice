using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Parcas_CS5 : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;

    public GameObject Achlys;
    public GameObject Erebus;
    public GameObject Thanatos;
    public GameObject Artmis;
    public GameObject Hero;


    public Animator transitionAnim;
    public string sceneToLoad;
    private string name = "";


    public bool releaseSpaceBar = false;

    public GameObject totem;

    IEnumerator Type()
    {
        yield return new WaitForSeconds(1f);
        foreach(char letter in sentences[index].ToCharArray()){
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        releaseSpaceBar = true;
    }
    IEnumerator LoadScene()
    {
        transitionAnim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.5f);        
        var loading = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        yield return loading;

        int sp = GameObject.Find("Hero").GetComponent<HeroMovement>().specialAttack;
        int lf = GameObject.Find("Hero").GetComponent<HeroMovement>().maxHealth;
        float mt = GameObject.Find("Hero").GetComponent<HeroMovement>().multiplier;
        int dm = GameObject.Find("Hero").GetComponent<HeroMovement>().actualDamage;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));

        GameObject.Find("Hero").GetComponent<HeroMovement>().sceneToLoad = "Parcas-HeroDeath4";
        GameObject.Find("Hero").GetComponent<HeroMovement>().specialAttack = sp;
        GameObject.Find("Hero").GetComponent<HeroMovement>().maxHealth = lf;
        GameObject.Find("Hero").GetComponent<HeroMovement>().multiplier = mt;
        GameObject.Find("Hero").GetComponent<HeroMovement>().actualDamage = dm;

        Achlys.GetComponent<Enemy>().maxHealth = Achlys.GetComponent<Enemy>().maxHealth + 300;
        Achlys.GetComponent<Enemy>().baseAttack = 300;
        Achlys.GetComponent<Enemy>().sceneToLoad = "Parcas-ForthBossDefeated";


        Artmis.GetComponent<Enemy>().maxHealth = Achlys.GetComponent<Enemy>().maxHealth + 300; ;
        Artmis.GetComponent<Enemy>().baseAttack = 300;
        Artmis.GetComponent<Enemy>().sceneToLoad = "Parcas-ForthBossDefeated";


        Erebus.GetComponent<Enemy>().maxHealth = Achlys.GetComponent<Enemy>().maxHealth + 300; ;
        Erebus.GetComponent<Enemy>().baseAttack = 300;
        Erebus.GetComponent<Enemy>().sceneToLoad = "Parcas-ForthBossDefeated";


        Thanatos.GetComponent<Enemy>().maxHealth = Achlys.GetComponent<Enemy>().maxHealth + 300; ;
        Thanatos.GetComponent<Enemy>().baseAttack = 300;
        Thanatos.GetComponent<Enemy>().sceneToLoad = "Parcas-ForthBossDefeated";

        GameObject.FindGameObjectWithTag("totem").transform.position = new Vector3(-0.64f, 1.36f);

        if (name == "ACHLYS")
        {
            Instantiate(Achlys,new Vector3(0,1.36f,0),Quaternion.identity);
        }
        else if (name == "EREBUS")
        {
            Instantiate(Erebus, new Vector3(0, 1.36f, 0), Quaternion.identity);
        }
        else if (name == "ARTMIS")
        {
            Instantiate(Artmis, new Vector3(0, 1.36f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(Thanatos, new Vector3(0, 1.36f, 0), Quaternion.identity);
        }

        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(-0.49f, -11.97f);

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
        transitionAnim.SetTrigger("FadeIn");

    }

    public void NextSentence()
    {
        releaseSpaceBar = false;
        if (index < 2)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
 
        }
        else if(index==2) {
            textDisplay.text = "";
            float choice = Mathf.Round((1 + 4 * Random.Range(0f,1f))*100f/100f);
            if (choice == 1)
            {
                name = "ACHLYS";

            }
            else if (choice == 2)
            {
                name = "ARTMIS";
            }
            else if (choice == 3)
            {
                name = "EREBUS";
            }
            else
            {
                name = "THANATOS";
            }
            sentences[3]="ALL: " + name + "!";
            StartCoroutine(Type());
            index++;
        }
        else
        {
            textDisplay.text = "";
            StartCoroutine(LoadScene());
          
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Hero = GameObject.FindGameObjectWithTag("Player");
        if (Hero != null)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(-15, 0);
        }
        StartCoroutine(Type());

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") & releaseSpaceBar)
        {
            StopCoroutine(Type());
            NextSentence();
        }
    }
}

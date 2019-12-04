using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Parcas_CS6: MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;

    public GameObject Atropos;
    public GameObject Laquesis;
    public GameObject Cloto;
    public GameObject Hero;



    public Animator transitionAnim;
    public string sceneToLoad;
    private string name = "";


    public bool releaseSpaceBar = false;

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

        GameObject.Find("Hero").GetComponent<HeroMovement>().sceneToLoad = "Parcas-HeroDeath5";
        GameObject.Find("Hero").GetComponent<HeroMovement>().specialAttack = sp;
        GameObject.Find("Hero").GetComponent<HeroMovement>().maxHealth = lf;
        GameObject.Find("Hero").GetComponent<HeroMovement>().multiplier = mt;
        GameObject.Find("Hero").GetComponent<HeroMovement>().actualDamage = dm;

        Instantiate(Atropos, new Vector3(-0.42f,-9.07f), Quaternion.identity);
        Instantiate(Cloto, new Vector3(-13.72f, 10.18f), Quaternion.identity);
        Instantiate(Laquesis, new Vector3(12.22f, 10.70f), Quaternion.identity);

        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(-0.0f, -12.13f);

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
        transitionAnim.SetTrigger("FadeIn");
    }

    public void NextSentence()
    {
        releaseSpaceBar = false;
        if (index < sentences.Length-1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
 
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

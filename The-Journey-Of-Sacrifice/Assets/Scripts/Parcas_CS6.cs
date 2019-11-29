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
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));

        GameObject.Find("Hero").GetComponent<HeroMovement>().sceneToLoad = "Parcas-HeroDeath3";

        
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Parcas_CS2 : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;

    public Animator transitionAnim;
    public string sceneToLoad;

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
        SceneManager.LoadScene(sceneToLoad);
        yield return new WaitForSeconds(3.5f);
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
            string name = "";
            if (choice == 1)
            {
                name = "ACHLYS";
            }
            else if (choice == 2)
            {
                name = "EREBUS";
            }
            else if (choice == 3)
            {
                name = "HECATE";
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

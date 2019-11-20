using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Parcas_CS1 : MonoBehaviour
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
        if (index < sentences.Length - 1)
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

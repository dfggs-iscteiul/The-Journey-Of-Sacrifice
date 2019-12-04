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

    public GameObject hero;

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
        Instantiate(hero, new Vector3(-0.46f,-1.06f), Quaternion.identity);
        int special = (int)Mathf.Floor(1 + 4 * Random.Range(0f, 1f));
        if (special == 1)
        {
            hero.GetComponent<HeroMovement>().specialAttack = 1;
        }
        if (special == 2)
        {
            hero.GetComponent<HeroMovement>().specialAttack = 2;
        }
        if (special == 3)
        {
           hero.GetComponent<HeroMovement>().specialAttack = 3;
        }
        if (special == 4)
        {
            hero.GetComponent<HeroMovement>().specialAttack = 4;
        }

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(0));
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
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(-15, 0);
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

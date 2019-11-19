using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransiion : MonoBehaviour
{
    public Animator transitionAnim;
    public string sceneToLoad;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            StartCoroutine(LoadScene());
        }
    } 

    IEnumerator LoadScene()
    {
        transitionAnim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneToLoad);
        yield return new WaitForSeconds(3.5f);
        transitionAnim.SetTrigger("FadeIn");
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Totem : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    private string[] sentences = new string[1];
    public float typingSpeed;
    private bool done = false;


    void OnTriggerStay2D (Collider2D other)
    {

        if (other.CompareTag("Player") && !other.isTrigger)
        {        

            GameObject player = GameObject.FindWithTag("Player");
            int power = Mathf.RoundToInt(Mathf.Floor(Mathf.Log(Random.Range(0f, 1f)) / Mathf.Log(1 - (0.65f))));
            if (!done)
            {
                switch (power)
                {
                    case 1:
                        player.GetComponent<HeroMovement>().maxHealth = player.GetComponent<HeroMovement>().maxHealth + 300;
                        sentences[0] = "The mythical beings gave you a boost of 300HP";
                        done = true;
                        StartCoroutine(Type());
                        break;
                    case 2:
                        player.GetComponent<HeroMovement>().maxHealth = player.GetComponent<HeroMovement>().maxHealth + 400;
                        sentences[0] = "The mythical beings gave you a boost of 400HP";
                        done = true;
                        StartCoroutine(Type());
                        break;
                    case 3:
                        player.GetComponent<HeroMovement>().maxHealth = player.GetComponent<HeroMovement>().maxHealth + 500;
                        sentences[0] = "The mythical beings gave you a boost of 500HP";
                        done = true;
                        StartCoroutine(Type());
                        break;
                    case 4:
                        player.GetComponent<HeroMovement>().multiplier = 2f;
                        sentences[0] = "The mythical beings multiplied your attack by 2x";
                        done = true;
                        StartCoroutine(Type());
                        break;
                    case 5:
                        player.GetComponent<HeroMovement>().multiplier = 3f;
                        sentences[0] = "The mythical beings multiplied your attack by 3x";
                        done = true;
                        StartCoroutine(Type());
                        break;
                    case 6:
                        player.GetComponent<HeroMovement>().multiplier = 4f;
                        sentences[0] = "The mythical beings multiplied your attack by 4x";
                        done = true;
                        StartCoroutine(Type());
                        break;
                    case 7:
                        player.GetComponent<HeroMovement>().maxHealth = player.GetComponent<HeroMovement>().maxHealth + 500;
                        player.GetComponent<HeroMovement>().multiplier = 5f;
                        sentences[0] = "The mythical beings gave you a boost of 500HP and multiplied your attack by 5x";
                        done = true;
                        StartCoroutine(Type());
                        break;
                }
            }
        }
    }

    IEnumerator Type()
    {
        yield return new WaitForSeconds(1f);
        foreach (char letter in sentences[0].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
            yield return new WaitForSeconds(2f);
            textDisplay.text = "";
            StopCoroutine(Type());
    }

}

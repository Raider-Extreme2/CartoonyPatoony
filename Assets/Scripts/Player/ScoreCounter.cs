using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] int score;
    [SerializeField] GameObject colectableUI;

    [Header("SoundFX")]
    [SerializeField] AudioSource playerSpeaker;
    [SerializeField] AudioClip coinNoise;

    private void Start()
    {
        text.text = "x0";
        colectableUI.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Coin")
        {
            other.gameObject.SetActive(false);
            playerSpeaker.PlayOneShot(coinNoise);
            score++;
            text.text = "x" + score.ToString();
            StartCoroutine(ShowScore());
        }
    }
    IEnumerator ShowScore() 
    {
        colectableUI.SetActive(true);
        yield return new WaitForSeconds(3);
        colectableUI.SetActive(false);
    }
}

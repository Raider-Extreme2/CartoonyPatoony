using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLifeManager : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] Animator playerAnimator;

    [Header("Life")]
    public bool playerIsAlive;
    public int healt;


    void Awake()
    {
        playerIsAlive = true;
    }

    void Update()
    {
        if (playerIsAlive)
        {
            if (healt <= 0)
            {
                StartCoroutine(PlayerDeath());
            }
        }
    }

    IEnumerator PlayerDeath()
    {
        playerIsAlive = false;
        playerAnimator.SetBool("Dead", true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("IslandMap");
    }
}

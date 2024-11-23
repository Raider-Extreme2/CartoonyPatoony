using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeCombat : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] Animator playerAnimator;

    [Header("Hammer")]
    public bool holdingHammer;
    public GameObject hammerObject;

    [Header("MeleeCombat")]
    public List<HammerAttackSO> combo;
    [SerializeField] float lastClickedTime;
    [SerializeField] float lastComboEnd;
    [SerializeField] int comboCounter;
    //[SerializeField] Collider hammerHead;

    [Header("HP Check")]
    [SerializeField] PlayerLifeManager playerLifeManager;

    [Header("Gun Check")]
    [SerializeField] PlayerRangedCombat playerRangedCombat;

    [Header("ParticleFX")]
    [SerializeField] ParticleSystem[] listaDeParticulas;

    private void Awake()
    {
        for (int i = 0; i < listaDeParticulas.Length; i++)
        {
            listaDeParticulas[i].gameObject.SetActive(false);
        }
    }

    void Start()
    {
        holdingHammer = false;
        hammerObject.SetActive(false);
    }

    void Update()
    {
        if (!playerLifeManager.playerIsAlive) return;
        if (playerRangedCombat.holdingGuns) return;
        EquipHammer();
        ExitAttack();

        if (Input.GetButtonDown("Fire1") && holdingHammer)
        {
            playerAnimator.SetTrigger("AttackT");
            HammerAttack();
        }


    }

    private void EquipHammer()
    {
        //Equip the Hammer
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("apertei E");
            if (!holdingHammer)
            {
                holdingHammer = true;
                playerAnimator.SetBool("HoldingHammer", true);
                hammerObject.SetActive(true);
                for (int i = 0; i < listaDeParticulas.Length; i++)
                {listaDeParticulas[i].gameObject.SetActive(false);}
                StartCoroutine(WaitAnimation());
            }
        }
        //Unequip the Hammer
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("apertei R");
            if (holdingHammer)
            {
                holdingHammer = false;
                playerAnimator.SetBool("HoldingHammer", false);
                playerAnimator.SetBool("PlayedAlready", false);//<--------------------------
                hammerObject.SetActive(false);
                StopAllCoroutines();
            }
        }
    }
    IEnumerator WaitAnimation()
    {
        yield return new WaitForSeconds(1f);
        playerAnimator.SetBool("PlayedAlready", true);//<--------------------------
    }

    void HammerAttack()
    {
        if (Time.time - lastComboEnd > 1f && comboCounter <= combo.Count)
        {
            CancelInvoke("EndCombo");
        }
        //delay between attacks
        if (Time.time - lastClickedTime >= 0.75f)
        {
            //hammerHead.enabled = true;
            playerAnimator.runtimeAnimatorController = combo[comboCounter].animOverrider;
            playerAnimator.Play("HammerAttackDefault", 1, 0);
            //visual effects happen here
            PlayPaticleFX();
            comboCounter++;
            lastClickedTime = Time.time;

            if (comboCounter > combo.Count)
            {
                comboCounter = 0;
            }
        }
    }

    void ExitAttack()
    {
        if (playerAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0.9f && playerAnimator.GetCurrentAnimatorStateInfo(1).IsTag("HAttack"))
        {
            Invoke("EndCombo", 0.1f);
            //hammerHead.enabled = false;
        }
    }

    void EndCombo()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
        playerAnimator.ResetTrigger("AttackT");
    }

    public void PlayPaticleFX()
    {
        for (int j = 0; j < listaDeParticulas.Length; j++)
        {
            listaDeParticulas[j].gameObject.SetActive(true);

            if (listaDeParticulas[j].isStopped)
            {
                listaDeParticulas[j].Play();
            }
        }
    }

}

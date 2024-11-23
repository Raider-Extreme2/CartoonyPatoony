using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class PlayerRangedCombat : MonoBehaviour
{

    [Header("Animator")]
    [SerializeField] Animator playerAnimator;

    [Header("Guns Ref")]
    [SerializeField] GameObject GunR;
    [SerializeField] GameObject GunL;

    [Header("Ranged Attack")]
    public bool holdingGuns;
    public bool aiming;

    [Header("HP Check")]
    [SerializeField] PlayerLifeManager playerLifeManager;

    [Header("HammerCheck")]
    [SerializeField] PlayerMeleeCombat playerMeleeCombat;

    [Header("Target Aquisition")]
    [SerializeField] Transform targetTransform;
    [SerializeField] LayerMask targetLayerMask;

    [Header("AimCamera")]
    [SerializeField] CinemachineVirtualCamera aimCamera;

    [Header("Crosshair")]
    [SerializeField] GameObject crosshair;

    [Header("Damage")]
    [SerializeField] float ShootingTime;
    [SerializeField] float cooldown;

    [Header("Damage")]
    public int damage;

    [Header("Decal")]
    [SerializeField] Transform decalFX;

    [Header("SoundFX")]
    [SerializeField] AudioSource playerSpeaker;
    [SerializeField] AudioClip gunNoise;

    //teste
    public GameObject teste01;

    void Start()
    {
        holdingGuns = false;
        GunR.SetActive(false);
        GunL.SetActive(false);
        aimCamera.gameObject.SetActive(false);
        crosshair.SetActive(false);
    }
    void Update()
    {
        if (!playerLifeManager.playerIsAlive) return;
        if (playerMeleeCombat.holdingHammer) return;

        Vector3 mouseWorldPos = Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, targetLayerMask))
        {
            targetTransform.position = hit.point;
            mouseWorldPos = hit.point;
            hitTransform = hit.transform;
        }

        //Fire the gun
        if (Input.GetButtonDown("Fire1") && holdingGuns && Time.time - cooldown >= ShootingTime)
        {
            playerAnimator.SetTrigger("PistolAttack");
            playerSpeaker.PlayOneShot(gunNoise);
            cooldown = Time.time;
            //the atack happens here
            if (hitTransform != null)
            {
                if (hitTransform.GetComponent<EnemyController>() != null)
                {
                    var enemy = hitTransform.gameObject.GetComponent<EnemyController>();
                    enemy.vida -= damage;
                    GameObject teste02 = Instantiate(teste01, hitTransform.transform.position, hitTransform.transform.rotation);
                }
                else
                {
                    Instantiate(decalFX, targetTransform.transform.position, Quaternion.identity);
                }
            }

        }
        else
        {
            playerAnimator.ResetTrigger("PistolAttack");
        }

        //equip the gun
        if (Input.GetKey(KeyCode.Z) && !holdingGuns)
        {
            playerAnimator.SetBool("HoldingPistols", true);
            StartCoroutine(GunToggleEquip());
        }
        //unequip the gun
        if (Input.GetKey(KeyCode.X) && holdingGuns)
        {
            playerAnimator.SetBool("HoldingPistols", false);
            StartCoroutine(GunToggleUnequip());
        }

        //make gun object visible and invisible
        if (holdingGuns)
        {
            GunR.SetActive(true);
            GunL.SetActive(true);
        }
        else
        {
            GunR.SetActive(false);
            GunL.SetActive(false);
        }

        //toggle aim
        if (holdingGuns && Input.GetButton("Fire2"))
        {
            aiming = true;
            aimCamera.gameObject.SetActive(true);
            crosshair.SetActive(true);
            Vector3 worldAimTarget = mouseWorldPos;
            worldAimTarget.y = transform.position.y;
            Vector3 aimLookDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimLookDirection, Time.deltaTime * 20f);
        }
        else
        {
            aiming = false;
            aimCamera.gameObject.SetActive(false);
            crosshair.SetActive(false);
        }
    }
    IEnumerator GunToggleEquip() 
    {
        yield return new WaitForSeconds(0.5f);

        if (!holdingGuns)
        {
            holdingGuns = true;
        }
    }

    IEnumerator GunToggleUnequip()
    {
        yield return new WaitForSeconds(0.75f);

        if (holdingGuns)
        {
            holdingGuns = false;
        }
    }
}

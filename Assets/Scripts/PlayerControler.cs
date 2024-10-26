using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControler : MonoBehaviour
{
    [Header("Movment Variables")]
    [SerializeField] CharacterController CC_player;
    [SerializeField] float playerSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float runningSpeed;
    [SerializeField] float gravityForce;
    [SerializeField] Vector3 playerVelocity;
    [SerializeField] bool playerGrounded;
    [SerializeField] float jumpHeight;
    [SerializeField] int airJumpCount;
    [SerializeField] float rotationTime;
     float rotationSpeed;

    [Header("Camera")]
    [SerializeField] Transform Camera;

    [Header("Trap Layers")]
    [SerializeField] LayerMask SideSmasher;
    [SerializeField] LayerMask TopSmasher;

    [Header("Animation")]
    [SerializeField] Animator playerAnimator;

    [Header("Hammer")]
    [SerializeField] bool holdHammer;
    public GameObject marteloObject;

    [Header("MeleeCombat")]
    public List<HammerAttackSO> combo;
    [SerializeField] float lastClickedTime;
    [SerializeField] float lastComboEnd;
    [SerializeField] int comboCounter;
    [SerializeField] CapsuleCollider hammerHead;

    [Header("Life")]
    public int healt;
    bool playerIsAlive;

    private void Start()
    {
        playerSpeed = walkSpeed;
        CC_player = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        holdHammer = false;
        marteloObject.SetActive(false);
        playerIsAlive = true;
    }
    void Update()
    {
        if (!playerIsAlive) return;
        Movment();
        EquipHammer();
        ExitAttack();

        if (healt <= 0)
        {
            StartCoroutine(PlayerDeath());
        }

        if (Input.GetButtonDown("Fire1") && holdHammer)
        {
            playerAnimator.SetTrigger("AttackT");
            HammerAttack();
        }

        playerGrounded = CC_player.isGrounded;
        if (playerGrounded && playerVelocity.y <= 0)
        {
            playerVelocity.y = 0;
            airJumpCount = 1;
        }

        //Jump
        if (Input.GetButtonDown("Jump") && playerGrounded)
        {
            playerAnimator.SetTrigger("Jumping");
            playerVelocity.y += (jumpHeight * gravityForce);
        }
        //Air Jump
        if (Input.GetButtonDown("Jump") && !playerGrounded && airJumpCount > 0)
        {
            playerAnimator.SetTrigger("AirJumping");
            airJumpCount--;
            playerVelocity.y += (jumpHeight * gravityForce);
        }

        //ground check
        if (playerGrounded)
        {
            print("is grounded boss");
            playerAnimator.ResetTrigger("AirJumping");
            playerAnimator.SetBool("Falling", false);
        }
        else
        {
            print("nope");
            playerAnimator.SetBool("Falling",true);
            playerAnimator.ResetTrigger("Jumping");
        }
    }
    private void Movment()
    {
       //Move
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0 , Input.GetAxisRaw("Vertical")).normalized;
        //CC_player.Move(move * playerSpeed * Time.deltaTime);

        //Run
        if (Input.GetButton("Fire3"))
        {
            playerSpeed = runningSpeed;
            playerAnimator.SetTrigger("RunningT");
            playerAnimator.SetBool("RunningB", true);
        }
        if (Input.GetButtonUp("Fire3"))
        {
            playerSpeed = walkSpeed;
            playerAnimator.ResetTrigger("RunningT");
            playerAnimator.SetBool("RunningB", false);
        }

        //Turn
        if (move.magnitude >= 0.1f)
        {
            //New Way
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + Camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, rotationTime);
            transform.rotation = Quaternion.Euler(0f,angle,0f);

            Vector3 masterMovment = Quaternion.Euler(0f,targetAngle,0f) * Vector3.forward;
            CC_player.Move(masterMovment * playerSpeed * Time.deltaTime);

            playerAnimator.SetTrigger("WalkingT");
            playerAnimator.SetBool("WalkingB", true);

            //Old Way
            //gameObject.transform.forward = move;
            //Quaternion rotateThisWay = Quaternion.LookRotation(move, Vector3.up);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateThisWay, rotationSpeed * Time.deltaTime);
        }
        else
        {
            playerAnimator.ResetTrigger("WalkingT");
            playerAnimator.SetBool("WalkingB", false);
        }

        //gravity
        if (playerGrounded)
        {
            playerVelocity.y += gravityForce;
        }
        else
        {
            playerVelocity.y += gravityForce * Time.deltaTime;
        }
        CC_player.Move(playerVelocity * Time.deltaTime);

        //Grow back
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(Grow());
        }
    }

    private void EquipHammer() 
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("apertei E");
            if (holdHammer == false)
            {
                holdHammer = true;
                playerAnimator.SetBool("HoldingHammer", true);
                marteloObject.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("apertei R");
            if (holdHammer)
            {
                holdHammer = false;
                playerAnimator.SetBool("HoldingHammer", false);
                marteloObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SideSmasher")
        {
            transform.localScale = new Vector3(0.2f, 1f, 0.2f);
        }
        if (other.tag == "TopSmasher")
        {
            transform.localScale = new Vector3(1f, 0.2f, 1f);
        }
    }

    IEnumerator Grow() 
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
        yield return new WaitForSeconds(0.5f);
    }

    void HammerAttack() 
    {
        if (Time.time - lastComboEnd > 1f && comboCounter <= combo.Count)
        {
            CancelInvoke("EndCombo");
        }
        if (Time.time - lastClickedTime >= 0.2f)
        {
            hammerHead.enabled = true;
            playerAnimator.runtimeAnimatorController = combo[comboCounter].animOverrider;
            playerAnimator.Play("HammerAttackDefault", 1, 0);
            //visual effects happen here
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
            hammerHead.enabled = false;
        }
    }

    void EndCombo() 
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
        playerAnimator.ResetTrigger("AttackT");
    }

    IEnumerator PlayerDeath()
    {
        playerIsAlive = false;
        playerAnimator.SetBool("Dead",true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("IslandMap");
    }
}

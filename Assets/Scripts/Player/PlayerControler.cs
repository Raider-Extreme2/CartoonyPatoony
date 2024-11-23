using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [Header("Movment Variables")]

    [Header("Walk and Run")]
    [SerializeField] CharacterController CC_player;
    [SerializeField] float playerSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float runningSpeed;

    [Header("Jump and Gravity")]
    [SerializeField] Vector3 playerVelocity;
    [SerializeField] bool playerGrounded;
    [SerializeField] float gravityForce;
    [SerializeField] float jumpHeight;
    [SerializeField] int airJumpCount;
    [SerializeField] float JumpMultiplyer;

    [Header("Turn Speed and Rate")]
    [SerializeField] float rotationTime;
     float rotationSpeed;

    [Header("Camera")]
    [SerializeField] Transform Camera;

    [Header("Trap Layers")]
    [SerializeField] LayerMask SideSmasher;
    [SerializeField] LayerMask TopSmasher;

    [Header("Animation")]
    [SerializeField] Animator playerAnimator;

    [Header("HP Check")]
    [SerializeField] PlayerLifeManager playerLifeManager;

    [Header("Gun Check")]
    [SerializeField] PlayerRangedCombat playerRangedCombat;



    private void Start()
    {
        playerSpeed = walkSpeed;
        CC_player = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        if (!playerLifeManager.playerIsAlive) return;
        Movment();

        //Air Jump Reset
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
            playerVelocity.y += (jumpHeight * gravityForce * JumpMultiplyer);
        }
        //Air Jump
        if (Input.GetButtonDown("Jump") && !playerGrounded && airJumpCount > 0)
        {
            playerAnimator.SetTrigger("AirJumping");
            airJumpCount--;
            playerVelocity.y += (jumpHeight * gravityForce);
        }

        //Fall Animation
        if (playerGrounded)
        {
            //print("is grounded boss");
            playerAnimator.ResetTrigger("AirJumping");
            playerAnimator.SetBool("Falling", false);
        }
        else
        {
            //print("nope");
            playerAnimator.SetBool("Falling",true);
            playerAnimator.ResetTrigger("Jumping");
        }
    }
    private void Movment()
    {
        //Move
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0 , Input.GetAxisRaw("Vertical")).normalized;

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
            if (!playerRangedCombat.aiming)
            {
                //Rotation Control
                float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + Camera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, rotationTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 masterMovment = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                CC_player.Move(masterMovment * playerSpeed * Time.deltaTime);


                playerAnimator.SetTrigger("WalkingT");
                playerAnimator.SetBool("WalkingB", true);
            }
        }
        else
        {
            playerAnimator.ResetTrigger("WalkingT");
            playerAnimator.SetBool("WalkingB", false);
        }

        //Gravity
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(Grow());
        }
    }

   

    private void OnTriggerEnter(Collider other)
    {
        //Become Thin
        if (other.tag == "SideSmasher")
        {
            transform.localScale = new Vector3(0.2f, 1f, 0.2f);
        }
        //Become Short
        if (other.tag == "TopSmasher")
        {
            transform.localScale = new Vector3(1f, 0.2f, 1f);
        }
    }

    IEnumerator Grow() 
    {
        //Makes Player Normal Size
        transform.localScale = new Vector3(1f, 1f, 1f);
        yield return new WaitForSeconds(0.5f);
    }

    
}

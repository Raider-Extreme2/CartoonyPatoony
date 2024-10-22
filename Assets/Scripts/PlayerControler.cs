using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [Header("Movment Variables")]
    [SerializeField] CharacterController CC_player;
    [SerializeField] float playerSpeed;
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

    private void Start()
    {
        CC_player = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        Movment();

        playerGrounded = CC_player.isGrounded;
        if (playerGrounded && playerVelocity.y <= 0)
        {
            playerVelocity.y = 0;
            airJumpCount = 1;
        }

        //Jump
        if (Input.GetButtonDown("Jump") && playerGrounded)
        {
            playerVelocity.y += (jumpHeight * gravityForce);
        }
        if (Input.GetButtonDown("Jump") && !playerGrounded && airJumpCount > 0)
        {
            airJumpCount--;
            playerVelocity.y += (jumpHeight * gravityForce);
        }

        //debug log
        if (playerGrounded)
        {
            print("is grounded boss");
        }
        else
        {
            print("nope");
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
        }
        if (Input.GetButtonUp("Fire3"))
        {
            playerSpeed = 5f;
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

            //Old Way
            //gameObject.transform.forward = move;
            //Quaternion rotateThisWay = Quaternion.LookRotation(move, Vector3.up);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateThisWay, rotationSpeed * Time.deltaTime);
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
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Grow());
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
}

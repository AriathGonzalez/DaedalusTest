using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class <c>UserControlledPlayer</c> Controls player movement which includes sprinting, crouching, and jumping.
/// <summary>
public class UserControlledPlayer : MonoBehaviour
{
    //Visible/Adjustable variables.
    [Header("Movement")]
    public float speed = 5f;
    public float groundDrag;
    public float jumpForce = 5f;
    public float jumpCooldown;
    public float airMultiplier;
    public float maxStam = 5;
    //Hidden variables. (If no access modifier is present, it is enforced as private.)
    float normalSpeed;
    float fastSpeed;
    float slowSpeed;
    bool readyToJump;
    bool isTired;
    bool isSprinting;
    [HideInInspector]
    //May be needed for UI, left public for that reason.
    public float curStam;
    

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    [HideInInspector]
    //Needed for 'Interactable' and 'PuzzleManager' scripts.
    public bool consoleCheck;
    [Header("Ground Check")]
    //Visible/Adjustable variables.
    public float playerHeight;
    public LayerMask whatIsGround;
    //Hidden variables.
    bool grounded;
    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    void Start()
    {

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        isTired = false;
        curStam = maxStam;
        normalSpeed = speed;
        fastSpeed = normalSpeed * 2f;
        slowSpeed = normalSpeed * .5f;

        //Player has full battery
 


    }

    void Update()
    {

        //TODO: calculate whether player is certain distance from enemy to reduce health.

        //if player is interacting with a console.
        if (consoleCheck) 
        {
            return;
        }
        //Enforce cooldown if player runs out of stam.
        if (curStam <= 0f) {
            StartCoroutine(Cooldown());
        }
        //Detect if player is on ground.
        //TODO 2: Player height range may not be accurate, need to change.
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();

        //If player is grounded apply drag, otherwise in air, no drag.
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
        
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    /// <summary>
    /// Handles taking in user input and storing that input.
    /// </summary>
    void MyInput()
    {
        //Get keyboard inputs.
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //If player wants to, and is able to jump, do so and enforce cooldown.
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    /// <summary>
    /// Applies movement force to player based on input from MyInput()
    /// </summary>
    void MovePlayer()
    {
        //Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //If player is grounded, player can crouch and sprint.
        if(grounded)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                isSprinting = false;
                speed = slowSpeed;
                //TODO 1: Add smoothing to crouched and uncrouched positions.
                //Adjusts camera down.
                this.transform.GetChild(0).transform.localPosition = new Vector3(0f, 0f, 0f);
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftShift) && !isTired && curStam > 0f)
                {
                    speed = fastSpeed;
                    isSprinting = true;
                    //Reduce stamina while sprinting.
                    curStam -= 1 * Time.deltaTime;
                }
                else
                {
                    speed = normalSpeed;
                    isSprinting = false;
                }
                //TODO 1
                //Adjusts to normal camera position.
                this.transform.GetChild(0).transform.localPosition = new Vector3(0f, .6f, 0f);
            }
            if (curStam < maxStam && !isSprinting)
            {
                //Stamina regeneration.
                curStam += (1 * Time.deltaTime) / 2;
                if (curStam > maxStam)
                {
                    //One check to keep curStam from being over maxStam.
                    curStam = maxStam;
                }
            }
            //Apply force to player rigidbody.
            rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        }    
        //If airborne, do not check crouch or sprint controls.
        //Force normal speed? keeping old speed if sprinting does not feel right for this game.
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * normalSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    /// <summary>
    /// Limits speed of player. Needed due to player's speed increasing past our desired speed,
    /// due to accelration.
    /// </summary>
    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //If our velocity increases past our desired speed,
        if(flatVel.magnitude > speed)
        {
            //Limit it.
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    /// <summary>
    /// Applies jump force to player based on input from MyInput()
    /// </summary>
    void Jump()
    {
        //Reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //Add a force to our rigidbody
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    /// <summary>
    /// Enforces our jump cooldown based on a timer.
    /// </summary>
    void ResetJump()
    {
        readyToJump = true;
    }

    /// <summary>
    /// Enforces our sprint cooldown based on a timer.
    /// </summary>
    IEnumerator Cooldown()
    {
        isTired = true;
        yield return new WaitForSeconds(5f);
        isTired = false;
    }

    //TODO: Battery deplete function






}

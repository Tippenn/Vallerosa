using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    private Rigidbody rb;

    #region Movement Variables

    public bool playerCanMove = true;
    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;
    public bool isWalking = false;

    #region Sprint

    public KeyCode sprintKey = KeyCode.LeftShift;
    public float sprintSpeed = 7f;
    public float sprintFOV = 80f;
    public float sprintFOVStepTime = 10f;

    // Sprint Bar
    public bool isSprinting = false;

    #endregion

    #region Jump

    public bool enableJump = true;
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpPower = 5f;

    // Internal Variables
    private bool isGrounded = false;

    #endregion

    #region Crouch

    public bool enableCrouch = true;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public float crouchHeight = .75f;
    public float speedReduction = .5f;

    // Internal Variables
    public bool isCrouched = false;
    private Vector3 originalScale;

    #endregion

    #region Slide
    public float slideHeight = .75f;
    public float speedIncrease = 1.5f;

    //Internal Variables
    public bool isSliding = false;
    private bool canSlide = false;
    private float canSlideThreshold = 1f;
    private float canSlideTimer = 0f;
    private float slideTimer = 0f;
    private float maxSlideTimer = 1f;

    #endregion

    #endregion
    private void Awake()
    {
        originalScale = transform.localScale;
        Instance = this;
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        #region Sprint
        if (isSprinting)
        {   
            canSlideTimer += Time.deltaTime;

            // Check For Slide
            if (canSlideTimer > canSlideThreshold)
            {
                canSlide = true;
            }
        }
        else
        {
            canSlideTimer = 0f;
        }

        #endregion

        #region Jump

        // Gets input and calls jump method
        if (enableJump && Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        #endregion

        #region Crouch & Slide

        //sliding
        if (enableCrouch && isSprinting && canSlide)
        {

            if (Input.GetKeyDown(crouchKey))
            {
                isSliding = false;
                Slide();
            }
            else if (Input.GetKeyUp(crouchKey) && isSliding)
            {
                isSliding = true;
                Slide();
            }
        }
        //crouching
        else if (enableCrouch && !isSprinting)
        {

            if (Input.GetKeyDown(crouchKey))
            {
                isCrouched = false;
                Crouch();
            }
            else if (Input.GetKeyUp(crouchKey))
            {
                isCrouched = true;
                Crouch();
            }
        }
        #endregion

        CheckGround();
    }

    private void FixedUpdate()
    {
        if (playerCanMove)
        {
            // Calculate how fast we should be moving
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // Checks if player is walking and isGrounded
            // Will allow head bob
            if (targetVelocity.x != 0 || targetVelocity.z != 0 && isGrounded)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }

            // All movement calculations while sprint is active

            if (Input.GetKey(sprintKey))
            {
                targetVelocity = transform.TransformDirection(targetVelocity) * sprintSpeed;

                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                // Player is only moving when valocity change != 0
                // Makes sure fov change only happens during movement
                if (velocityChange.x != 0 || velocityChange.z != 0)
                {
                    isSprinting = true;

                    if (isCrouched)
                    {
                        Crouch();
                    }

                }

                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }
            // All movement calculations while walking
            else
            {
                isSprinting = false;

                targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;

                // Apply a force that attempts to reach our target velocity
                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }

        }
    }

    private void Jump()
    {
        // Adds force to the player rigidbody to jump
        if (isGrounded)
        {
            rb.AddForce(0f, jumpPower, 0f, ForceMode.Impulse);
            isGrounded = false;
        }

        // When crouched and using toggle system, will uncrouch for a jump
        if (isCrouched)
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        // Stands player up to full height
        // Brings walkSpeed back up to original speed
        if (isCrouched)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            walkSpeed /= speedReduction;

            isCrouched = false;
        }
        // Crouches player down to set height
        // Reduces walkSpeed
        else
        {
            transform.localScale = new Vector3(originalScale.x, crouchHeight, originalScale.z);
            walkSpeed *= speedReduction;

            isCrouched = true;
        }
    }

    private void Slide()
    {
        // Stands player up to full height
        // Brings walkSpeed back up to original speed
        if (isSliding)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            sprintSpeed /= speedIncrease;
            PlayerHeadBob.Instance.enableHeadBob = true;
            isSliding = false;
            
        }
        // Crouches player down to set height
        // Reduces walkSpeed
        else
        {
            transform.localScale = new Vector3(originalScale.x, slideHeight, originalScale.z);
            sprintSpeed *= speedIncrease;
            PlayerHeadBob.Instance.enableHeadBob = false;
            isSliding = true;
            Invoke(nameof(ResetSliding), maxSlideTimer);
        }
    }

    private void ResetSliding()
    {
        if(isSliding)
        {
            Slide();
        }      
    }

    private void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = .75f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}

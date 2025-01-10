using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    private Rigidbody rb;

    #region Movement Variables

    #region Walking
    [Header("Walking")]
    public bool playerCanMove = true;
    public float walkSpeed = 5f;
    public float maxVelocityChange = 10f;
    public bool isWalking = false;
    #endregion

    #region Dash
    [Header("Dash")]
    public KeyCode dashKey = KeyCode.LeftShift;
    public float dashForce = 20f;
    public float dashSpeedIncrease = 2f;
    public float dashSpeedAmp = 1.2f;
    public float dashFOV = 80f;
    public float dashFOVStepTime = 10f;
    public float maxDashDuration = 2f;
    public Vector3 dashDirection;

    // Sprint Bar
    public bool isDashing = false;
    public bool canDash = true;
    public bool justDash = false;
    #endregion

    #region Jump
    [Header("Jump")]
    public bool enableJump = true;
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpPower = 5f;
    public float maxJumpAmount = 2f;

    // Internal Variables
    public bool isGrounded = false;
    public float jumpAmount = 0f;
    public bool waitBuffer;
    public float waitBufferDuration = 0.2f;

    #endregion

    #region GroundSlam
    [Header("GroundSlam")]
    public bool enableCrouch = true;
    public KeyCode groundSlamKey = KeyCode.LeftControl;
    public float groundSlamLinger = 0.5f;
    public float groundSlamForce = 15f;
    public float groundSlamAmp = 1.6f;
    public float groundSlamRadius = 5f;
    public float groundSlamDamage = 20f;
    public LayerMask enemyLayers;

    // Internal Variables
    public bool isGroundSlam = false;
    public bool justGroundSlam = false;
    private Vector3 originalScale;

    #endregion

    #region Slide
    [Header("Slide")]
    public float slideHeight = .75f;
    public float slideSpeed = 8f;
    public float slideAmp = 2f;
    public bool justSlide = false;
    public KeyCode slideKey = KeyCode.LeftControl;
    public Vector3 slideDirection;

    //Internal Variables
    public bool isSliding = false;
    private bool canSlide = true;

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
        CheckWalkingSFX();
        
        if (LevelManager.instance)
        {
            if (LevelManager.instance.isPaused)
            {
                return;
            }
        }

        

        #region Sprint

        #endregion

        #region Jump

        // Gets input and calls jump method
        if (enableJump && Input.GetKeyDown(jumpKey) && jumpAmount > 0f)
        {
            Jump();
        }

        //recovery
        if(isGrounded)
        {
            jumpAmount = 2f;
        }

        #endregion

        #region Slide

        //sliding
        if (isGrounded)
        {
            if (Input.GetKeyDown(slideKey))
            {
                Debug.Log("isSliding");
                isSliding = false;
                Slide();
            }
            else if (Input.GetKeyUp(slideKey) && isSliding)
            {
                isSliding = true;
                Slide();
            }
        }
        #endregion

        #region GroundSlam
        if(!isGrounded && !isGroundSlam)
        {
            if (Input.GetKeyDown(groundSlamKey))
            {
                GroundSlam();
            }
        }

        if (isGroundSlam)
        {
            if (isGrounded)
            {
               GroundSlamShockwave();
            }
        }
        #endregion

        #region Dash
        if (canDash)
        {
            if (Input.GetKeyDown(dashKey))
            {
                Dash();
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

            if (isSliding)
            {
                targetVelocity = slideDirection * slideSpeed;
            }
            else if (isGroundSlam)
            {
                targetVelocity = new Vector3(0f, -1f, 0f) * groundSlamForce;
            }
            else if (isDashing)
            {
                targetVelocity = dashDirection * dashForce;
            }
            else{
                targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;
            }

            if(justSlide)
            {
                targetVelocity = targetVelocity * slideAmp;
            }
            else if (justDash)
            {
                targetVelocity = targetVelocity * dashSpeedAmp;
            }
            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            if (isGroundSlam)
            {
                velocityChange.y = Mathf.Clamp(velocityChange.y, -maxVelocityChange, maxVelocityChange);
            }
            else
            {
                velocityChange.y = 0f;
            }
            

            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }

    private void Jump()
    {
        float totalJumpPower = jumpPower;
        waitBuffer = true;
        isGrounded = false;
        if (justGroundSlam)
            totalJumpPower = totalJumpPower * groundSlamAmp;
        if (isDashing)
        {
            justDash = true;
            isDashing = false;
        }
        else if (isSliding)
        {
            Slide();
            justSlide = true;
        }

        if(jumpAmount == 1)
        {
            rb.AddForce(0f, totalJumpPower*1.5f, 0f, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(0f, totalJumpPower, 0f, ForceMode.Impulse);
        }
        jumpAmount--;
        Invoke("ResetGroundCheck", waitBufferDuration);
    }

    public void ResetGroundCheck()
    {
        waitBuffer = false;
    }
    #region Slide
    private void Slide()
    {
        // Stands player up to full height
        // Brings walkSpeed back up to original speed
        if (isSliding)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            PlayerHeadBob.Instance.enableHeadBob = true;
            isSliding = false;
            
        }
        // Crouches player down to set height
        // Reduces walkSpeed
        else
        {
            transform.localScale = new Vector3(originalScale.x, slideHeight, originalScale.z);
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (targetVelocity.x != 0 || targetVelocity.z != 0)
            {
                slideDirection = transform.TransformDirection(targetVelocity);
            }
            else
            {
                slideDirection = transform.TransformDirection(Vector3.forward);
            }
            
            PlayerHeadBob.Instance.enableHeadBob = false;
            isSliding = true;
        }
    }

    public void ResetSlide()
    {

    }
    #endregion

    #region ground slam
    public void GroundSlam()
    {
        PlayerHeadBob.Instance.enableHeadBob = false;
        isGroundSlam = true;
    }

    public void GroundSlamShockwave()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, groundSlamRadius, enemyLayers);

        // Apply damage to all enemies hit by the sphere
        foreach (Collider unit in hitEnemies)
        {
            Debug.Log("adaUnit");
            IDamageable damageable = unit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Debug.Log("Ground Pounded");
                damageable.TakeDamage(groundSlamDamage);
            }
        }
        isGroundSlam = false;
        justGroundSlam = true;
        Invoke(nameof(ResetGroundSlam), groundSlamLinger);
    }

    public void ResetGroundSlam()
    {
        justGroundSlam = false;
    }
    #endregion

    #region Dash
    public void Dash()
    {
        if (isDashing) return;
        
        isDashing = true;
        rb.useGravity = false;
        PlayerHeadBob.Instance.enableHeadBob = false;
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (targetVelocity.x != 0 || targetVelocity.z != 0)
        {
            dashDirection = transform.TransformDirection(targetVelocity);
        }
        else
        {
            dashDirection = transform.TransformDirection(Vector3.forward);
        }

        Invoke("ResetDash",maxDashDuration);
    }

    private void ResetDash()
    {
        if(isDashing)
        {
            isDashing = false;
        }
        playerCanMove = true;
        rb.useGravity = true;
        PlayerHeadBob.Instance.enableHeadBob = true;
    }

    #endregion

    private void CheckGround()
    {
        if (waitBuffer) return;
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = 1f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;
            canSlide = true;
            justSlide = false;
            justDash = false;
            jumpAmount = maxJumpAmount;
        }
        else
        {
            isGrounded = false;
            canSlide = false;
        }
    }

    public void Teleport(Vector3 position)
    {
        transform.position = position;
    }

    public void CheckWalkingSFX()
    {
        if (!LevelManager.instance.isPaused)
        {
            if (isWalking == true)
            {
                AudioManager.Instance.turnOn(AudioManager.Instance.walking);
            }
            else
            {
                AudioManager.Instance.turnOff(AudioManager.Instance.walking);
            }
        }
        else
        {
            AudioManager.Instance.turnOff(AudioManager.Instance.walking);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, groundSlamRadius);
    }
}

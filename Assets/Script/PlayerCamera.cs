using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance;

    public Camera playerCamera;

    public float fov = 60f;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 50f;
    public float sprintFOV = 80f;
    public float sprintFOVStepTime = 10f;

    //internal variable
    private float yaw = 0.0f;
    private float pitch = 0.0f;

    //crosshair
    public Sprite crosshairImage;
    [SerializeField]private Image crosshairObject;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        crosshairObject.sprite = crosshairImage;
    }

    void Update()
    {
        if (PlayerStats.Instance.isDead || PlayerStats.Instance.isWin)
        {
            return;
        }
        #region Camera
        yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;

        pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");

        // Clamp pitch between lookAngle
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        transform.localEulerAngles = new Vector3(0, yaw, 0);
        playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        #endregion

        #region Sprinting
        if(PlayerMovement.Instance.isSliding)
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, sprintFOVStepTime * Time.deltaTime);
        }
        else
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, fov, sprintFOVStepTime * Time.deltaTime);
        }
        #endregion

        #region Walking
        #endregion
    }
}

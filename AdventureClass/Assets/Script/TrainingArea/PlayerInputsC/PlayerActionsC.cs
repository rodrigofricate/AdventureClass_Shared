using Assets.Script.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerActionsC : MonoBehaviour
{
    [SerializeField] float rotationSpeeed;
    [SerializeField] float moveSpeed;
    Animator animator;
    //Camera
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    CinemachineFramingTransposer virtualCamBody;
    [SerializeField] float[] distancePosCam;
    int cameraPosSelector = 0;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        virtualCamBody = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        virtualCamBody.m_CameraDistance = distancePosCam[cameraPosSelector];
    }

    // Update is called once per frame
    void Update()
    {
        InputMagnitude();
        ChangeCameraPosition();
    }
    void RotatePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.Normalize();
        right.Normalize();

        Vector3 rotationDirection = (forward * verticalInput) + (right * horizontalInput);
        Quaternion rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotationDirection), rotationSpeeed);
        transform.rotation = new Quaternion(0, rotation.y, 0, rotation.w);
    }
    void InputMagnitude()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        float speed = new Vector2(verticalInput, horizontalInput).sqrMagnitude;
        float allowRotation = 0.1f;
        if (speed > allowRotation)
        {
            animator.SetFloat("InputMagnetude", speed);
            Walk();
            RotatePlayer();
        }
        else if (speed < allowRotation)
        {
            animator.SetFloat("InputMagnetude", speed);
        }
    }
    void Walk()
    {
        float walkSpeedForward = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float walkSpeedRight = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.Normalize();
        right.Normalize();
        Vector3 walkDirection = forward * walkSpeedForward + right * walkSpeedRight;
        transform.position += walkDirection;
    }

    void ChangeCameraPosition()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cameraPosSelector++;
            if (cameraPosSelector >= distancePosCam.Length)
            {
                cameraPosSelector = 0;
            }
            virtualCamBody.m_CameraDistance = distancePosCam[cameraPosSelector];
        }

    }
}

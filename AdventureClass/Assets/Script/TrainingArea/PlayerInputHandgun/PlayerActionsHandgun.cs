using Assets.Script.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerActionsHandgun : MonoBehaviour
{
    #region VarAntigas
    [SerializeField] float rotationSpeeed;
    [SerializeField] float moveSpeed;
    Animator animator;
    //Camera
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    CinemachineFramingTransposer virtualCamBody;
    [SerializeField] float[] distancePosCam;
    int cameraPosSelector = 0;
    #endregion

    [SerializeField] CharacterController characterController;
    InputManager input;
    // Start is called before the first frame update
    void Start()
    {
        // characterController = GetComponent<CharacterController>();
        input = GameManager.Instance.GetInputManager();

        #region Código Antigo
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        virtualCamBody = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
      //  virtualCamBody.m_CameraDistance = distancePosCam[cameraPosSelector];
        #endregion
    }

    // Update is called once per frame
    void Update()
    {

        //InputMagnitude();
        //ChangeCameraPosition();

        MoveChar();
    }
    #region Código Antigo
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
    #endregion


    void MoveChar()
    {
        float _horizontal = input.GetHorizontalAxis();
        float _vertical = input.GetVerticalAxis();

        animator.SetFloat("XAxis", _horizontal, 0.1f, Time.deltaTime);
        animator.SetFloat("YAxis", _vertical, 0.1f, Time.deltaTime);

     
        //Rotation
        float mouseXAxis = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up, mouseXAxis * Time.deltaTime * 800);
        if(_vertical != 0)
        {        
           characterController.SimpleMove(transform.forward  * moveSpeed * _vertical);
        }
        if(_horizontal != 0)
        {
            characterController.SimpleMove(transform.right * moveSpeed * _horizontal);
        }


    }
}

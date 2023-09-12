using Assets.Script.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerActionsHandgun : MonoBehaviour
{
    #region VarAntigas
    [SerializeField] float rotationSpeeed;
    [SerializeField] float verticalSpeed;
    float verticalAxisLimit = 2.0f;
    [SerializeField] float moveSpeed;
    Animator animator;
    //Camera
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    CinemachineComposer composer;
    [SerializeField] float[] distancePosCam;
    int cameraPosSelector = 0;
    #endregion

    CharacterController characterController;
    InputManager input;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        input = GameManager.Instance.GetInputManager();
        composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
      
        #region Código Antigo
         animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
       
        MoveChar();
    }

    #region Código Antigo



    void ChangeCameraPosition()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cameraPosSelector++;
            if (cameraPosSelector >= distancePosCam.Length)
            {
                cameraPosSelector = 0;
            }
        }

    }
    #endregion


    void MoveChar()
    {
        float _horizontal = input.GetHorizontalAxis();
        float _vertical = input.GetVerticalAxis();

        animator.SetFloat("XAxis", _horizontal, 0.1f, Time.deltaTime);
        animator.SetFloat("YAxis", _vertical, 0.1f, Time.deltaTime);

        //Rotação vertical
        float mouseYAxis = Input.GetAxis("Mouse Y");
        float verticalRotation = mouseYAxis * verticalSpeed * Time.deltaTime;
        Debug.Log(mouseYAxis);
        VerticalCameraRotation(verticalRotation);


        //Rotation Horizontal
        float mouseXAxis = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up, mouseXAxis * Time.deltaTime * 800);
        if (_vertical != 0)
        {
            characterController.SimpleMove(transform.forward * moveSpeed * _vertical);
        }
        if (_horizontal != 0)
        {
            characterController.SimpleMove(transform.right * moveSpeed * _horizontal);
        }


    }
    void VerticalCameraRotation(float verticalRotation)
    {
        float composeAligne = composer.m_TrackedObjectOffset.y + verticalRotation;
        if(Mathf.Abs(composeAligne) <= verticalAxisLimit)
        {
            composer.m_TrackedObjectOffset.y += verticalRotation;
        }
        
    }
}

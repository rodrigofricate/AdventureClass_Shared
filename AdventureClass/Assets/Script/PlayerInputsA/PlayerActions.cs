using Assets.Script.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    Animator animator;
    [SerializeField] Transform rigthHandPivot;
    [SerializeField] EnumPlayerActionState playerState = EnumPlayerActionState.Grounded;
    [SerializeField] Transform target;
    Transform hangedTransformAdjust;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {


        HangedActions();
        Revive();
        Jump();
        Lift();
       // RotatePlayer();
    }
    private void FixedUpdate()
    {

        if (playerState != EnumPlayerActionState.Dead && !animator.GetCurrentAnimatorStateInfo(0).IsName("GettingUp") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Lifting"))
        {
            Walk();
            Strafe();

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            playerState = EnumPlayerActionState.Grounded;
        }

    }

    void Walk()
    {
        if (playerState == EnumPlayerActionState.Grounded || playerState == EnumPlayerActionState.Jumping)
        {
            float moveDirection = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
            float dumpTime = 0.1f;
            if (moveDirection != 0)
            {
                animator.SetFloat("YAxis", Input.GetAxis("Vertical"), dumpTime, Time.deltaTime);
            }
            else
            {
                animator.SetFloat("YAxis", 0);
            }
            transform.position += transform.forward * moveDirection;
        }
    }
    void Strafe()
    {
        if (playerState == EnumPlayerActionState.Grounded)
        {
            float moveDirection = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            float dumpTime = 0.1f;

            if (moveDirection != 0)
            {
                animator.SetFloat("XAxis", Input.GetAxis("Horizontal"), dumpTime, Time.deltaTime);
            }
            else
            {
                animator.SetFloat("XAxis", 0);
            }
            transform.position += transform.right * moveDirection;

        }
    }

    void Revive()
    {
        if (playerState == EnumPlayerActionState.Dead)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerState = EnumPlayerActionState.Grounded;
                animator.SetTrigger("GettingUp");
            }
        }

    }
    void Jump()
    {

        if (playerState == EnumPlayerActionState.Grounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerState = EnumPlayerActionState.Jumping;
                animator.SetTrigger("Jump");
            }
        }
    }
    void HangedActions()
    {
        HangedSlide();
        DropDown();
        Climb();
    }
    void HangedSlide()
    {
        if (playerState == EnumPlayerActionState.Hanged)
        {
            transform.position = hangedTransformAdjust.transform.position;
            float moveDirection = Input.GetAxis("Horizontal") * moveSpeed / 2.0f * Time.deltaTime;

            if (Input.GetAxis("Horizontal") < 0)
            {
                animator.SetBool("HangLeft", true);
                hangedTransformAdjust.position += hangedTransformAdjust.right * moveDirection;
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                animator.SetBool("HangRight", true);
                hangedTransformAdjust.position += hangedTransformAdjust.right * moveDirection;

            }
            else
            {
                animator.SetBool("HangLeft", false);
                animator.SetBool("HangRight", false);

            }
        }

    }
    void DropDown()
    {
        if (playerState == EnumPlayerActionState.Hanged)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerState = EnumPlayerActionState.DropDown;
                GetComponent<Rigidbody>().isKinematic = false;
                animator.SetTrigger("DropDown");
            }
        }

    }
    void Climb()
    {
        if (playerState == EnumPlayerActionState.Hanged)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                playerState = EnumPlayerActionState.Climb;
                transform.position += new Vector3(0, 0.3f, 0);
                animator.SetTrigger("Climb");
            }
        }
    }
    void Lift()
    {
        if (playerState == EnumPlayerActionState.Grounded && !animator.GetCurrentAnimatorStateInfo(0).IsName("Lifting"))
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                float maximumDistance = 1;
                float distance = Vector3.Distance(transform.position, target.position);

                if (distance <= maximumDistance)
                {
                    animator.SetTrigger("Lifting");
                }

            }

        }

    }
    //IK
    private void OnAnimatorIK(int layerIndex)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= 1)
        {
            animator.SetLookAtWeight(0.6f);
            animator.SetLookAtPosition(target.position);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Lifting"))
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, animator.GetFloat("IK_Value"));
            animator.SetIKPosition(AvatarIKGoal.RightHand, target.position);
        }
        if (animator.GetFloat("IK_Value") >= 0.478f)
        {
            target.parent = rigthHandPivot;
            target.localPosition = new Vector3(0.03f, 0.12f, 0.075f);
        }
    }

    //Public
    public void Death()
    {
        if (playerState != EnumPlayerActionState.Dead)
        {
            playerState = EnumPlayerActionState.Dead;
            animator.SetTrigger("Death");
        }

    }
    public void StartHang(Transform hangerTranform)
    {
        if (playerState == EnumPlayerActionState.Jumping)
        {
            playerState = EnumPlayerActionState.Hanged;
            hangedTransformAdjust = hangerTranform;
            Quaternion rot = transform.rotation;
            rot.y = hangerTranform.rotation.z;
            transform.rotation = rot;
            GetComponent<Rigidbody>().isKinematic = true;
            animator.SetTrigger("HangIdle");
        }
    }

    //Use with animations
    public void DisableKinematic()
    {
        GetComponent<Rigidbody>().isKinematic = false;

    }

    public void RotatePlayer()
    {
        float rotateYAxis = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
      transform.Rotate(0, rotateYAxis, 0);
    }
  
}

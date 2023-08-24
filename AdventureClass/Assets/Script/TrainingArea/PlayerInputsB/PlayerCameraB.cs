using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraB : MonoBehaviour
{

    [SerializeField] Transform[] cameraTarget;
    [SerializeField] int cameraPos;

    [SerializeField] float cameraSensitive;
    [SerializeField] float maxYAxisAngle;
    float YRotationAxis = 0;
    float XRotationAxis = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CameraRotation();
        ChangeCameraPosition();
    }
    private void FixedUpdate()
    {
      CameraFollow();
    }

    void CameraRotation()
    {
        YRotationAxis += Input.GetAxis("Mouse X") * cameraSensitive * Time.deltaTime;
        XRotationAxis += Input.GetAxis("Mouse Y") * cameraSensitive * Time.deltaTime;
        XRotationAxis = Mathf.Clamp(XRotationAxis, -maxYAxisAngle, maxYAxisAngle);
        Quaternion rotation = Quaternion.Euler(-XRotationAxis, YRotationAxis, 0);
        transform.rotation = rotation;



    }
    void CameraFollow()
    {
        Vector3 smoothSpeed = Vector3.zero;
        float smoothTime = 0f;
       transform.position = Vector3.SmoothDamp(transform.position, cameraTarget[cameraPos].position, ref smoothSpeed, smoothTime);
    }
    void ChangeCameraPosition()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cameraPos++;
            if (cameraPos >= cameraTarget.Length)
            {
                cameraPos = 0;
            }
            Vector3 smoothVector = Vector3.zero;
            float smoothTime = 0.3f;
            gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, cameraTarget[cameraPos].position, ref smoothVector, smoothTime);
        }
    }
}

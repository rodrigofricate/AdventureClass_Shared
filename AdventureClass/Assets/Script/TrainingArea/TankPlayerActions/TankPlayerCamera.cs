using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TankPlayerCamera : MonoBehaviour
{
    [SerializeField] Transform cameraController;
    [SerializeField] Transform headTarget;
    [SerializeField] Transform[] cameraPosition;
    [SerializeField] Transform player;
    int cameraPositionSelector = 0;
    RaycastHit hitInfo;
    [SerializeField] float cameraRotationSpeed;
    float cameraYAxisValue =0;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameObject.transform.position = cameraPosition[cameraPositionSelector].position;
        gameObject.transform.LookAt(headTarget.transform.position);
    }
    private void Update()
    {
       if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeCameraPosition();
        }
        RotateCameraAndPlayer();
    }
    private void LateUpdate()
    {
      gameObject.transform.LookAt(headTarget.transform.position);

        Vector3 smoothVector = Vector3.zero;
        float smoothTime = 0.05f;
        if (!Physics.Linecast(headTarget.position, cameraPosition[cameraPositionSelector].position))
        {
            gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, cameraPosition[cameraPositionSelector].position, ref smoothVector, smoothTime);
            Debug.DrawLine(headTarget.position, cameraPosition[cameraPositionSelector].position);
        }
        else if (Physics.Linecast(headTarget.position, cameraPosition[cameraPositionSelector].position, out hitInfo))
        {
            gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, hitInfo.point, ref smoothVector, smoothTime);

        }
    

    }

    void ChangeCameraPosition()
    {
        cameraPositionSelector++;
        if (cameraPositionSelector >= cameraPosition.Length)
        {
            cameraPositionSelector = 0;
        }
        Vector3 smoothVector = Vector3.zero;
        float smoothTime = 0.05f;
        gameObject.transform.position = Vector3.SmoothDamp(gameObject.transform.position, cameraPosition[cameraPositionSelector].position, ref smoothVector, smoothTime);
    }
    void RotateCameraAndPlayer()
    {
        cameraYAxisValue = Input.GetAxis("Mouse X") * cameraRotationSpeed * Time.deltaTime;
        player.Rotate(0, cameraYAxisValue, 0);

     
    }
}

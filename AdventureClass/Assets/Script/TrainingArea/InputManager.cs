using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    float verticalAxis;
    float horizontalAxis;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        verticalAxis = Input.GetAxis("Vertical");
        horizontalAxis = Input.GetAxis("Horizontal");


    }

    public float GetVerticalAxis() { return verticalAxis; }
    public float GetHorizontalAxis() {  return horizontalAxis; }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    InputManager inputManager;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null){ Instance = this; }
        inputManager = GetComponent<InputManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public InputManager GetInputManager() {  return inputManager; }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsExemple : MonoBehaviour
{
    Action exempleAction;
    // Start is called before the first frame update
    void Start()
    {
        exempleAction = () => { Debug.Log("Sou a action!"); };
        exempleAction();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

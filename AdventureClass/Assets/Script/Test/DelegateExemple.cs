using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateExemple : MonoBehaviour
{
    delegate void FirstDelegate(string text);
    FirstDelegate myDelegate;
    void Start()
    {
        myDelegate += HelloWorld;
        myDelegate.Invoke("Hello delegate");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HelloWorld(string text)
    {
        Debug.Log(text);
    }

}

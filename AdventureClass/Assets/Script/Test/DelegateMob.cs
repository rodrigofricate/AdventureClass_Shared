using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateMob : MonoBehaviour
{
   
    void Start()
    {
        DelegateBoss.BossOrder += RecieveOrder;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RecieveOrder(string order)
    {
        Debug.Log("O chefe disse: " + order);
    }
}

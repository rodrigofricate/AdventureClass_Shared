using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateBoss : MonoBehaviour
{
   public delegate void  Boss(string order);
    public static event Boss BossOrder;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(BossOrder != null)
            {
                BossOrder("Matem eles");
            }
            else
            {
                Debug.Log("Sem ordem");
            }
        }
    }
}

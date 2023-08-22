using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hanger : MonoBehaviour
{
    [SerializeField] Transform hangTranform;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
                other.gameObject.GetComponent<PlayerActions>().StartHang(hangTranform);
            
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decal : MonoBehaviour
{
   float lifeSpan = 0f;
    private void FixedUpdate()
    {
        Vanish();
    }
    void Vanish()
    {
        lifeSpan += Time.fixedDeltaTime;
        if (lifeSpan > 5f )
        {
            Destroy( this.gameObject );
        }
    }
}

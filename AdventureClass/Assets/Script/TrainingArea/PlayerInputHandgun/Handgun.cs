using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Handgun : MonoBehaviour
{
    [SerializeField] GameObject decal;
    [SerializeField] Image crosshair;
    [SerializeField] float fireImpact;
    float weaponRange = 100;


    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        CheckTarget();
    }

    void Shot(RaycastHit target)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (target.transform.CompareTag("Enemy"))
            {
                // Destroy(target.transform.gameObject);
                target.rigidbody.AddForce(-target.normal * fireImpact, ForceMode.Impulse);
            }
            else
            {
                DecalTarget(target);
            }

        }
    }
    void CheckTarget()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, weaponRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                crosshair.color = Color.red;
            }
            else
            {
                crosshair.color = Color.white;
            }
            Shot(hit);


        }

    }
    void DecalTarget(RaycastHit target)
    {
        if (!target.transform.CompareTag("Enemy"))
        {
            GameObject[] decalsOnWorld = GameObject.FindGameObjectsWithTag("Decal");
            if (decalsOnWorld.Length > 4) { Destroy(decalsOnWorld[0]); }
            GameObject _decal = Instantiate(decal);
            _decal.transform.position = target.point;
            _decal.transform.forward = target.normal * -1;
            _decal.transform.position -= transform.forward * 0.001f;
        }
    }

}

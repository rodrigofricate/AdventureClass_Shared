using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Handgun : MonoBehaviour
{
    [SerializeField] GameObject decal;
    [SerializeField] GameObject decalPaticles;
    [SerializeField] GameObject handgunPointerParticles;
    [SerializeField] GameObject ammo;
    [SerializeField] Transform ammoExitPointer;
    [SerializeField] Transform handgunPointer;
    [SerializeField] Image crosshair;
    [SerializeField] float fireImpact;
    float weaponRange = 100;
   
   

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
      
        CrossHair(ray);
        Shot(ray);
    }

    void Shot(Ray ray)
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, weaponRange))
            {
                GameObject _shotFX = Instantiate(handgunPointerParticles, handgunPointer.position, handgunPointer.rotation);
                Destroy(_shotFX, 1.5f);
                DropUsedAmmo();
                if (hit.transform.CompareTag("Enemy"))
                {
                    if (hit.transform.gameObject.TryGetComponent(out Rigidbody _rb))
                    {
                        //_rb.AddForce(-hit.normal * fireImpact, ForceMode.Impulse);
                        // Destroy(hit.transform.gameObject, 2.0f);
                        Debug.Log(hit.collider.name);
                    }

                }
                else
                {
                    DecalTarget(hit);

                }
            }
        }
    }
    void CrossHair(Ray ray)
    {
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
            GameObject _decalFX = Instantiate(decalPaticles, _decal.transform.transform.position, _decal.transform.rotation);
            Destroy(_decalFX, 0.5f);
        }
    }
    void DropUsedAmmo()
    {
        GameObject _usedAmmo = Instantiate(ammo);
      
        _usedAmmo.transform.position = ammoExitPointer.transform.position;

        
        _usedAmmo.GetComponent<Rigidbody>().AddForce(Vector3.up/5.0f,ForceMode.Impulse);
        Quaternion rot = ammoExitPointer.transform.rotation;
        rot.eulerAngles += new Vector3(0, 90, 0);
        _usedAmmo.transform.rotation = rot;
        Destroy(_usedAmmo, 3.0f);

    }
}

using UnityEngine;
using Photon.Pun;

public class Gun : MonoBehaviourPun
{
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 50f;

    //public int maxAmmo = 10;
    //private int currentAmmo;
    //public float reloadTime = 1f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    //private void Start()
    //{
    //    currentAmmo = maxAmmo;
    //}


    void Update()
    {
        if (!photonView.IsMine) return;

        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    
    public void Shoot()
    {
        PhotonView PV = GetComponent<PhotonView>();
        if (!PV)
        {
            Debug.Log("PhotonView not found!");
        }
        else
        {
            photonView.RPC("ShootRPC", RpcTarget.All);
        }
    }

    [PunRPC]
    public void ShootRPC()
    {
        muzzleFlash.Play();
        RaycastHit hit;

        if (fpsCam != null)
        {
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);

                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if (photonView.IsMine)
                {
                    if (enemy != null)
                    {
                        enemy.photonView.RPC("TakeDamage", RpcTarget.All, damage);
                    }

                    if (hit.rigidbody != null)
                    {
                        hit.rigidbody.AddForce(-hit.normal * impactForce);
                    }
                    GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impactGo, 1.5f);
                }
            }
        }
    }

}

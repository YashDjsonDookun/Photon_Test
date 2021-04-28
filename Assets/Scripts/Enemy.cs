using UnityEngine;
using Photon.Pun;
public class Enemy : MonoBehaviourPunCallbacks
{
    public float health = 50f;

    [PunRPC]
    public void TakeDamage(float amount)
    {
        health -= amount;

        if(health  <= 0f)
        {
            photonView.RPC("Die", RpcTarget.All);
        }
    }

    [PunRPC]
    void Die()
    {
        Destroy(gameObject);
    }
}

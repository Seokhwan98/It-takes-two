using Fusion;
using Fusion.Addons.KCC;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<NetworkObject>() == null)
        { 
            Debug.Log($"PlayerRespawner: OnTriggerEnter with {other.name}");
            return;
        }

        if (other.GetComponentInParent<NetworkObject>().HasStateAuthority)
        {
            var cc = other.GetComponentInParent<KCC>();
            RespawnPlayer(cc);
            Debug.Log("1");
        }
    }
    
    private void RespawnPlayer(KCC cc)
    {
        cc.SetPosition(respawnPoint.position);
        cc.SetLookRotation(respawnPoint.rotation);
    }
}

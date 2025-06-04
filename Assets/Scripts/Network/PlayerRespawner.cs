using Fusion;
using Fusion.Addons.KCC;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<NetworkObject>() == null)
            return;

        if (other.GetComponentInParent<NetworkObject>().HasStateAuthority)
        {
            var cc = other.GetComponentInParent<KCC>();
            RespawnPlayer(cc);
        }
    }

    private void RespawnPlayer(KCC cc)
    {
        if (cc.HasStateAuthority)
        {
            var respawnPoint = transform;
            
            cc.TeleportRPC(respawnPoint.position, respawnPoint.rotation.eulerAngles.y,
                respawnPoint.rotation.eulerAngles.x);
        }
    }
}

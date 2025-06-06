using UnityEngine;
using Fusion;
using Fusion.Addons.KCC;

public class RoomStarter : NetworkBehaviour {
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    
    [SerializeField] private Transform _spawnPointA;
    [SerializeField] private Transform _spawnPointB;
    
    public override void Spawned()
    {
        InterfaceManager.Instance.MouseEnable();
        Debug.Log("11");
    }

    public void AutoHostOrClientSpawn()
    {
        Debug.Log("22");
        bool isHost = Runner.SessionInfo.PlayerCount == 1;
        Vector3 SpawnPoint = isHost ? _spawnPointA.position : _spawnPointB.position;
        
        foreach (PlayerRef playerRef in Runner.ActivePlayers)
        {
            if (Runner.SessionInfo.PlayerCount == 2 && playerRef == Runner.LocalPlayer)
                continue;
            
            Debug.Log("33");
            var playerObject = Runner.Spawn(_playerPrefab, SpawnPoint, inputAuthority: playerRef);
            Debug.Log("44");
            RPC_ComponentDisabled(playerObject);
            Debug.Log("66");
        }
    }
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_ComponentDisabled(NetworkObject playerObject)
    {
        Debug.Log("55");
        playerObject.GetComponent<PlayerMovement>().enabled = false;
        playerObject.GetComponent<KCC>().enabled = false;
    }
}

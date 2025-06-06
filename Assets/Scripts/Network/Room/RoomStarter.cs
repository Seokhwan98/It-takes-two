using UnityEngine;
using Fusion;
using Fusion.Addons.KCC;
using TMPro;

public class RoomStarter : NetworkBehaviour {
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    
    [SerializeField] private Transform _spawnPointA;
    [SerializeField] private Transform _spawnPointB;
    
    public override void Spawned()
    {
        InterfaceManager.Instance.MouseEnable();
    }

    public void AutoHostOrClientSpawn()
    {
        bool isHost = Runner.SessionInfo.PlayerCount == 1;
        Vector3 SpawnPoint = isHost ? _spawnPointA.position : _spawnPointB.position;
        
        var playerObject = Runner.Spawn(_playerPrefab, SpawnPoint, inputAuthority: Runner.LocalPlayer
        );
        
        /*
         * , 
            onBeforeSpawned: (runner, obj) =>
            {
                obj.GetComponent<PlayerNetworkApplier>()?.ApplyCustomization();
            }
         */
        
        playerObject.GetComponent<PlayerMovement>().enabled = false;
        playerObject.GetComponent<KCC>().enabled = false;
    }
}
using UnityEngine;
using Fusion;
using Fusion.Addons.KCC;

public class RoomSpawner : NetworkBehaviour {
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    
    [SerializeField] private Transform _spawnPointA;
    [SerializeField] private Transform _spawnPointB;
    
    public void AutoHostOrClientSpawn()
    {
        bool isHost = Runner.SessionInfo.PlayerCount == 1;
        Vector3 SpawnPoint = isHost ? _spawnPointA.position : _spawnPointB.position;
        
        var playerObject = Runner.Spawn(_playerPrefab, SpawnPoint, inputAuthority: Runner.LocalPlayer);
        
        playerObject.GetComponent<PlayerMovement>().enabled = false;
        playerObject.GetComponent<KCC>().enabled = false;
    }
}
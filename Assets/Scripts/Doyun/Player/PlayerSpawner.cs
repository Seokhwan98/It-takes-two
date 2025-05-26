using UnityEngine;
using Fusion;
using Fusion.Addons.KCC;

public class PlayerSpawner : NetworkBehaviour {
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    
    [SerializeField] private Transform _spawnPointA;
    [SerializeField] private Transform _spawnPointB;

    public override void Spawned()
    {
        if (!Runner.IsRunning) return;
        
        if (Runner.GameMode == GameMode.Shared)
        {
            SharedSpawn();
        }
    }
    
    private void SharedSpawn()
    {
        Runner.Spawn(_playerPrefab, Vector3.up * 3, inputAuthority: Runner.LocalPlayer);
    }
    
    public void AutoHostOrClientSpawn()
    {
        Debug.Log("AutoHostOrClientSpawn", gameObject);
        bool isHost = Runner.SessionInfo.PlayerCount == 1;
        Vector3 SpawnPoint = isHost ? _spawnPointA.position : _spawnPointB.position;
        
        var playerObject = Runner.Spawn(_playerPrefab, SpawnPoint, inputAuthority: Runner.LocalPlayer);
        
        playerObject.GetComponent<PlayerMovement>().enabled = false;
        playerObject.GetComponent<KCC>().enabled = false;
    }
}
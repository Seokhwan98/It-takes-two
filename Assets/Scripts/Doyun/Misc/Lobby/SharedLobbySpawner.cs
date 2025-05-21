using UnityEngine;
using Fusion;

public class SharedLobbySpawner : NetworkBehaviour {
    
    [SerializeField] private NetworkPrefabRef _playerPrefab;

    public override void Spawned()
    {
        Runner.Spawn(_playerPrefab, Vector3.up * 3, inputAuthority: Runner.LocalPlayer);
        Debug.Log("1");
    }
}

using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Random = UnityEngine.Random;

public class HostLobbySpawner : NetworkBehaviour
{
    [SerializeField] private NetworkPrefabRef _character;

    private Dictionary<PlayerRef, NetworkObject> _playerAvatars = new();
    
    public override void Spawned()
    {
        if (Object.HasStateAuthority == false) return;
        
        Vector3 pos = Vector3.zero;
        foreach (var playerRef in Runner.ActivePlayers)
        {
            pos = Random.insideUnitSphere * 3;
            pos.y = 2;
            _playerAvatars.Add(playerRef, Runner.Spawn(_character, pos, inputAuthority: playerRef));
        }
    }
}

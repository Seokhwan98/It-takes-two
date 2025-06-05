using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class GameStarter : NetworkBehaviour {
    [SerializeField] private NetworkPrefabRef _playerPrefab;

    private Dictionary<PlayerRef, NetworkObject> _playerAvatars;

    public override void Spawned()
    {
        InterfaceManager.Instance.MouseDisable();
        
        if (Object.HasStateAuthority == false) return;
        _playerAvatars = new Dictionary<PlayerRef, NetworkObject>();
    
        Vector3 pos = Vector3.zero;
        foreach (var playerRef in Runner.ActivePlayers)
        {
            pos = Random.insideUnitSphere * 3;
            pos.y = 2;
            var netObj = Runner.Spawn(_playerPrefab, pos, Quaternion.identity, playerRef);
            Runner.SetPlayerObject(playerRef, netObj);
            _playerAvatars[playerRef] = netObj;
            
            foreach (var kvp in _playerAvatars)
            {
                var playerMovement = kvp.Value.GetComponent<PlayerMovement>();
                playerMovement?.TryBindOtherCamera();
                playerMovement?.TryBindOtherInteractionUIUpdater();
            }
        }
    }
}
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
    
        int i = 0;
        foreach (var playerRef in Runner.ActivePlayers)
        {
            var spawnPos = (i % 2 == 0) ? Constant.spawnPoint1 : Constant.spawnPoint2;
            var netObj = Runner.Spawn(_playerPrefab, spawnPos, Constant.spawnRotation, playerRef);
            Runner.SetPlayerObject(playerRef, netObj);
            _playerAvatars[playerRef] = netObj;

            i++;
            
            foreach (var kvp in _playerAvatars)
            {
                var playerMovement = kvp.Value.GetComponent<PlayerMovement>();
                playerMovement?.TryBindOtherCamera();
            }
        }
    }
}
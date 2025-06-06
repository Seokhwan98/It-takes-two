using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Addons.KCC;

public class GameStarter : NetworkBehaviour {
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [SerializeField] private List<KCCProcessorInjectData> _injectProcessors;
    
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

                var kcc = kvp.Value.GetComponent<KCC>();
                foreach (var processorInjectData in _injectProcessors)
                {
                    TryInjectKCCProcessor(kcc, processorInjectData);
                }
            }
        }
    }

    private void TryInjectKCCProcessor(KCC kcc, KCCProcessorInjectData processorInjectData)
    {
        KCCProcessor processor = processorInjectData.Processor;
        KCCProcessorInjectData.EInjectTarget target = processorInjectData.InjectTarget;
        
        if(kcc.Object.InputAuthority.PlayerId == 1 && target.HasFlag(KCCProcessorInjectData.EInjectTarget.Host))
        {
            kcc.AddLocalProcessor(processor);
        }
        else if(kcc.Object.InputAuthority.PlayerId == 2 && target.HasFlag(KCCProcessorInjectData.EInjectTarget.Client))
        {
            kcc.AddLocalProcessor(processor);
        }
    }
}
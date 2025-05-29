using Fusion.Addons.KCC;
using UnityEngine;

public class ApplyExternalDeltaProcess : KCCProcessor, IAfterMoveStep
{
    private readonly float DefaultPriority = -5000;
    public override float GetPriority(KCC kcc) => DefaultPriority;
    
    public void Execute(AfterMoveStep stage, KCC kcc, KCCData data)
    {
        var playerData = kcc.GetComponent<PlayerMovement>().PlayerData;
        
        data.ExternalDelta = Vector3.zero;
        if (playerData.ExternalDelta.magnitude > 0.001f)
        {
            data.ExternalDelta += playerData.ExternalDelta;
        }
        
        playerData.ExternalDelta = default;
    }
}

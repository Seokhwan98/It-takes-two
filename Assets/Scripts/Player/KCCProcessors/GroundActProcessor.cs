using Fusion.Addons.KCC;
using UnityEngine;

public class GroundActProcessor : KCCProcessor, IAfterMoveStep
{
    private readonly float DefaultPriority = -1500;
    public override float GetPriority(KCC kcc) => DefaultPriority;
    
    public void Execute(AfterMoveStep stage, KCC kcc, KCCData data)
    {
        var playerData = kcc.GetComponent<PlayerMovement>().PlayerData;

        if (!data.IsGrounded) return;

        playerData.ResetAirJump();
    }
}

using System;
using Fusion.Addons.KCC;
using UnityEngine;

public class AirJumpProcessor : KCCProcessor, ISetDynamicVelocity
{
    [SerializeField] private Vector3 _baseJumpImpulse = 10f * Vector3.up;
    
    private readonly float DefaultPriority = 500;
    public override float GetPriority(KCC kcc) => DefaultPriority;
    
    public void Execute(ISetDynamicVelocity stage, KCC kcc, KCCData data)
    {
        var playerData = kcc.GetComponent<PlayerMovement>().PlayerData;
        
        if (playerData.AirJump > 0)
        {
            if (playerData.JumpTrigger.TryShot())
            {
                ApplyAirJump(data, playerData);
            }
            SuppressOtherJumpProcessors(kcc);
        }

        SuppressOtherSameTypeProcessors(kcc);
    }

    private void ApplyAirJump(KCCData data, PlayerData playerData)
    {
        Vector3 jumpImpulse = JumpImpulseHelper.GetJumpImpulse(_baseJumpImpulse, playerData.PlayerScale);
        data.DynamicVelocity = Vector3.zero;
        data.JumpImpulse = jumpImpulse;
        playerData.ApplyAirJump();
    }
    
    private void SuppressOtherSameTypeProcessors(KCC kcc)
    {
        kcc.SuppressProcessors<WallProcessor>();
        
    }

    private void SuppressOtherJumpProcessors(KCC kcc)
    {
        kcc.SuppressProcessors<NormalJumpProcessor>();
        kcc.SuppressProcessors<AirJumpProcessor>();
    }
}

using System;
using Fusion.Addons.KCC;
using UnityEngine;

public class AirJumpProcessor : KCCProcessor, ISetDynamicVelocity
{
    [SerializeField] private Vector3 _baseJumpImpulse = 10f * Vector3.up;
    
    private readonly float DefaultPriority = 2000;
    public override float GetPriority(KCC kcc) => DefaultPriority;
    
    public void Execute(ISetDynamicVelocity stage, KCC kcc, KCCData data)
    {
        var playerData = kcc.GetComponent<PlayerMovement>().PlayerData;

        Action applyAirJump = () => ApplyAirJump(data, playerData);
        
        playerData.JumpTrigger.OnShot += applyAirJump;
        
        if (playerData.AirJump > 0)
        {
            playerData.JumpTrigger.TryShot();
            SuppressOtherJumpProcessors(kcc);
        }
        
        playerData.JumpTrigger.OnShot -= applyAirJump;

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
        kcc.SuppressProcessors<WallJumpProcessor>();
        
    }

    private void SuppressOtherJumpProcessors(KCC kcc)
    {
        kcc.SuppressProcessors<NormalJumpProcessor>();
        kcc.SuppressProcessors<AirJumpProcessor>();
    }
}

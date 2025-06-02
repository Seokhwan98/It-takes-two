using System;
using Fusion.Addons.KCC;
using UnityEngine;

public class NormalJumpProcessor : KCCProcessor, ISetDynamicVelocity
{
    [SerializeField] private Vector3 _baseJumpImpulse = 10f * Vector3.up;
    
    private readonly float DefaultPriority = 503;
    public override float GetPriority(KCC kcc) => DefaultPriority;
    
    public void Execute(ISetDynamicVelocity stage, KCC kcc, KCCData data)
    {
        var playerData = kcc.GetComponent<PlayerMovement>().PlayerData;
        
        if (data.IsGrounded)
        {
            if (playerData.JumpTrigger.TryShot())
            {
                ApplyJump(data, playerData);
            }
            SuppressOtherJumpProcessors(kcc);
        }
        
        SuppressOtherSameTypeProcessors(kcc);
    }

    private void ApplyJump(KCCData data, PlayerData playerData)
    {
        Vector3 jumpImpulse = JumpImpulseHelper.GetJumpImpulse(_baseJumpImpulse, playerData.PlayerScale);
        data.DynamicVelocity = Vector3.zero;
        data.JumpImpulse = jumpImpulse;
    }
    
    private void SuppressOtherSameTypeProcessors(KCC kcc)
    {
        kcc.SuppressProcessors<NormalJumpProcessor>();
    }

    private void SuppressOtherJumpProcessors(KCC kcc)
    {
        kcc.SuppressProcessors<AirJumpProcessor>();
        kcc.SuppressProcessors<WallProcessor>();
    }
}

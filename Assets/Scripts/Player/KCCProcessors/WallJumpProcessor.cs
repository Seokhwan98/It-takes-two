using System;
using Fusion.Addons.KCC;
using UnityEngine;

public class WallJumpProcessor : KCCProcessor, ISetDynamicVelocity
{
    [SerializeField] private Vector3 _baseJumpImpulse = 10f * Vector3.up;
    
    private readonly float DefaultPriority = 2001;
    public override float GetPriority(KCC kcc) => DefaultPriority;
    
    public void Execute(ISetDynamicVelocity stage, KCC kcc, KCCData data)
    {
        var playerData = kcc.GetComponent<PlayerMovement>().PlayerData;
        if (!playerData.Wall) return;
        
        Action applyWallJump = () => ApplyWallJump(kcc, data, playerData);
        
        playerData.JumpTrigger.OnShot += applyWallJump;
        
        if (playerData.JumpTrigger.TryShot())
        {
            SuppressOtherJumpProcessors(kcc);
        }
        else
        {
            data.DynamicVelocity *= 0.5f;
        }
        
        playerData.JumpTrigger.OnShot -= applyWallJump;

        SuppressOtherSameTypeProcessors(kcc);
    }

    private void ApplyWallJump(KCC kcc, KCCData data, PlayerData playerData)
    {
        Vector3 jumpImpulse = JumpImpulseHelper.GetJumpImpulse(_baseJumpImpulse, playerData.PlayerScale);
        
        playerData.Wall = false;
        data.DynamicVelocity = Vector3.zero;
        data.JumpImpulse = kcc.transform.rotation * jumpImpulse;
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

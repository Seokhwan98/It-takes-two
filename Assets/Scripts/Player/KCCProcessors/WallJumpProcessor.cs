using System;
using Fusion.Addons.KCC;
using UnityEngine;

public class WallProcessor : KCCProcessor, ISetDynamicVelocity
{
    [SerializeField] private Vector3 _baseJumpImpulse = 10f * Vector3.up;
    [SerializeField] private float _wallSlideMultiplier = 0.5f;
    
    private readonly float DefaultPriority = 501;
    public override float GetPriority(KCC kcc) => DefaultPriority;
    
    public void Execute(ISetDynamicVelocity stage, KCC kcc, KCCData data)
    {
        var playerData = kcc.GetComponent<PlayerMovement>().PlayerData;
        if (!playerData.Wall) return;
        
        if (playerData.JumpTrigger.TryShot())
        {
            ApplyWallJump(data, playerData);
            SuppressOtherJumpProcessors(kcc);
        }
        else
        {
            data.DynamicVelocity *= _wallSlideMultiplier;
        }

        SuppressOtherSameTypeProcessors(kcc);
    }

    private void ApplyWallJump(KCCData data, PlayerData playerData)
    {
        Quaternion rot = Quaternion.LookRotation(playerData.WallNormal, Vector3.up);
        Vector3 jumpImpulse = JumpImpulseHelper.GetJumpImpulse(_baseJumpImpulse, playerData.PlayerScale);
        
        playerData.Wall = false;
        playerData.WallNormal = default;
        data.DynamicVelocity = Vector3.zero;
        data.JumpImpulse = rot * jumpImpulse;
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

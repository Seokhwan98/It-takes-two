using System;
using Fusion.Addons.KCC;
using UnityEngine;

public class NormalJumpProcessor : KCCProcessor, ISetDynamicVelocity
{
    [SerializeField] private Vector3 _jumpImpulse = 10f * Vector3.up;
    
    private readonly float DefaultPriority = 2003;
    public override float GetPriority(KCC kcc) => DefaultPriority;
    
    public void Execute(ISetDynamicVelocity stage, KCC kcc, KCCData data)
    {
        var playerData = kcc.GetComponent<PlayerMovement>().PlayerData;
        
        Action applyJump = () => ApplyJump(data);
        
        playerData.JumpTrigger.OnShot += applyJump;
        
        if (data.IsGrounded)
        {
            Debug.Log("Normal");
            playerData.JumpTrigger.TryShot();
            SuppressOtherJumpProcessors(kcc);
        }
        
        playerData.JumpTrigger.OnShot -= applyJump;
        
        SuppressOtherSameTypeProcessors(kcc);
    }

    private void ApplyJump(KCCData data)
    {
        data.JumpImpulse = _jumpImpulse;
    }
    
    private void SuppressOtherSameTypeProcessors(KCC kcc)
    {
        kcc.SuppressProcessors<NormalJumpProcessor>();
    }

    private void SuppressOtherJumpProcessors(KCC kcc)
    {
        kcc.SuppressProcessors<AirJumpProcessor>();
        kcc.SuppressProcessors<WallJumpProcessor>();
    }
}

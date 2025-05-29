using Fusion.Addons.KCC;
using UnityEngine;

public class WallJumpProcessor : KCCProcessor, ISetDynamicVelocity
{
    [SerializeField] private Vector3 _jumpImpulse = 10f * Vector3.up;
    
    private PlayerData _playerData;
    
    private readonly float DefaultPriority = 2001;
    public override float GetPriority(KCC kcc) => DefaultPriority;
    
    public void Execute(ISetDynamicVelocity stage, KCC kcc, KCCData data)
    {
        _playerData ??= kcc.GetComponent<PlayerMovement>().PlayerData;
        
        if (!_playerData.WallJump) return;
        
        Debug.Log("Wall");

        if (_playerData.TriggerJump)
        {
            ApplyWallJump(kcc, data);
            SuppressOtherJumpProcessors(kcc);
        }
        else
        {
            data.DynamicVelocity *= 0.5f;
        }
        
        SuppressOtherProcessors(kcc);
    }

    private void ApplyWallJump(KCC kcc, KCCData data)
    {
        data.JumpImpulse = kcc.transform.rotation * _jumpImpulse;
        _playerData.TriggerJump = false;
    }

    private void SuppressOtherProcessors(KCC kcc)
    {
        kcc.SuppressProcessors<WallJumpProcessor>();
    }
    
    private void SuppressOtherJumpProcessors(KCC kcc)
    {
        kcc.SuppressProcessors<AirJumpProcessor>();
        kcc.SuppressProcessors<NormalJumpProcessor>();
    }
}

using Fusion.Addons.KCC;
using UnityEngine;

public class NormalJumpProcessor : KCCProcessor, ISetDynamicVelocity
{
    [SerializeField] private Vector3 _jumpImpulse = 10f * Vector3.up;
    
    private readonly float DefaultPriority = 2003;
    public override float GetPriority(KCC kcc) => DefaultPriority;
    
    public void Execute(ISetDynamicVelocity stage, KCC kcc, KCCData data)
    {
        var _playerData = kcc.GetComponent<PlayerMovement>().PlayerData;
        
        // Debug.Log(_playerData.TriggerJump);
        if (!_playerData.TriggerJump) return;
        
        Debug.Log("Normal");
        
        if (data.IsGrounded)
        {
            ApplyJump(data, _playerData);
            SuppressOtherJumpProcessors(kcc);
        }
        
        SuppressOtherProcessors(kcc);
    }

    private void ApplyJump(KCCData data, PlayerData _playerData)
    {
        data.JumpImpulse = _jumpImpulse;
        _playerData.TriggerJump = false;
    }

    private void SuppressOtherProcessors(KCC kcc)
    {
        kcc.SuppressProcessors<NormalJumpProcessor>();
    }
    
    private void SuppressOtherJumpProcessors(KCC kcc)
    {
        kcc.SuppressProcessors<AirJumpProcessor>();
        kcc.SuppressProcessors<WallJumpProcessor>();
    }
}

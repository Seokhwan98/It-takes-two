using Fusion.Addons.KCC;
using UnityEngine;

public class AirJumpProcessor : KCCProcessor, ISetDynamicVelocity
{
    [SerializeField] private Vector3 _jumpImpulse = 10f * Vector3.up;
    
    private PlayerData _playerData;
    
    private readonly float DefaultPriority = 2000;
    public override float GetPriority(KCC kcc) => DefaultPriority;
    
    public void Execute(ISetDynamicVelocity stage, KCC kcc, KCCData data)
    {
        _playerData ??= kcc.GetComponent<PlayerMovement>().PlayerData;
        
        if (!_playerData.TriggerJump) return;

        Debug.Log("Air");
        
        if (_playerData.AirJump > 0)
        {
            ApplyAirJump(data);
            SuppressOtherJumpProcessors(kcc);
        }
        
        SuppressOtherProcessors(kcc);
        _playerData.TriggerJump = false;
    }

    private void ApplyAirJump(KCCData data)
    {
        data.DynamicVelocity = Vector3.zero;
        data.JumpImpulse = _jumpImpulse;
        _playerData.ApplyAirJump();
        _playerData.TriggerJump = false;
    }

    private void SuppressOtherProcessors(KCC kcc)
    {
        kcc.SuppressProcessors<AirJumpProcessor>();
    }
    
    private void SuppressOtherJumpProcessors(KCC kcc)
    {
        kcc.SuppressProcessors<NormalJumpProcessor>();
        kcc.SuppressProcessors<WallJumpProcessor>();
    }
}

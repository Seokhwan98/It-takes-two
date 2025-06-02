using Fusion.Addons.KCC;
using UnityEngine;

public class WallCheckProcessor : KCCProcessor, ISetDynamicVelocity
{
    [SerializeField] private float _castDistance = 1f;
    [SerializeField] private LayerMask _wallLayer;
    
    private readonly float DefaultPriority = 502;
    public override float GetPriority(KCC kcc) => DefaultPriority;

    public void Execute(ISetDynamicVelocity stage, KCC kcc, KCCData data)
    {
        var playerData = kcc.GetComponent<PlayerMovement>().PlayerData;
        
        if (data.IsGrounded)
        {
            playerData.Wall = false;
            ApplyWallAnimation(kcc, false);
            return;
        }

        bool result = Physics.SphereCast(
            kcc.transform.position + Vector3.up,
            0.2f,
            kcc.transform.forward,
            out RaycastHit hit,
            _castDistance,
            _wallLayer
        );

        if (result && data.DynamicVelocity.y < -0.001f && Vector3.Angle(Vector3.up, hit.normal) > data.MaxGroundAngle)
        {
            playerData.Wall = true;
            playerData.WallNormal = hit.normal;
        }
        else
        {
            playerData.Wall = false;
            playerData.WallNormal = default;
        }
        
        ApplyWallAnimation(kcc, playerData.Wall);
    }
    
    private void ApplyWallAnimation(KCC kcc, bool isWall)
    {
        var animatorController = kcc.GetComponent<NetworkAnimatorController>();
        animatorController.RPC_SetBool(Constant.IsWallHash, isWall);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            transform.position + Vector3.up,
            transform.position + Vector3.up + transform.forward * _castDistance
        );
    }
}

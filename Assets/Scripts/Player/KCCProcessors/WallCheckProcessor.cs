using Fusion.Addons.KCC;
using UnityEngine;

public class WallCheckProcessor : KCCProcessor, ISetDynamicVelocity
{
    [SerializeField] private float _wallSlideMultiplier = 0.5f;
    [SerializeField] private float _castDistance = 1f;
    [SerializeField] private LayerMask _wallLayer;
    
    private readonly float DefaultPriority = 2002;
    public override float GetPriority(KCC kcc) => DefaultPriority;

    public void Execute(ISetDynamicVelocity stage, KCC kcc, KCCData data)
    {
        var playerData = kcc.GetComponent<PlayerMovement>().PlayerData;

        if (data.IsGrounded)
        {
            playerData.Wall = false;
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

        if (result && data.DynamicVelocity.y < 0.001f && Vector3.Angle(Vector3.up, hit.normal) > data.MaxGroundAngle)
        {
            playerData.Wall = true;
        }
        else
        {
            playerData.Wall = false;
        }
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

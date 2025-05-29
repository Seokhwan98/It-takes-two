using System;
using System.Collections.Generic;
using Fusion.Addons.KCC;
using UnityEngine;

public class WallCheckProcessor : KCCProcessor, ISetDynamicVelocity
{
    [SerializeField] private float _wallSlideMultiplier = 0.5f;
    [SerializeField] private float _castDistance = 1f;
    [SerializeField] private LayerMask _wallLayer;
    
    private PlayerData _playerData;
    
    private readonly float DefaultPriority = 2002;
    public override float GetPriority(KCC kcc) => DefaultPriority;

    public void Execute(ISetDynamicVelocity stage, KCC kcc, KCCData data)
    {
        _playerData ??= kcc.GetComponent<PlayerMovement>().PlayerData;

        bool result = Physics.SphereCast(
            kcc.transform.position + Vector3.up,
            0.2f,
            kcc.transform.forward,
            out RaycastHit hit,
            0.5f,
            _wallLayer
        );

        if (result && Vector3.Angle(Vector3.up, hit.normal) > data.MaxGroundAngle)
        {
            _playerData.WallJump = true;
        }
        else
        {
            _playerData.WallJump = false;
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

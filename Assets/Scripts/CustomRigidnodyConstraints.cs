using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomRigidnodyConstraints : NetworkBehaviour
{
    [SerializeField] private Transform _coord;
    [SerializeField] private bool X;
    [SerializeField] private bool Y;
    [SerializeField] private bool Z;
    
    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority)
        {
            Matrix4x4 trs = Matrix4x4.TRS(_coord.position, _coord.rotation, Vector3.one);
            Vector4 originPos = new(transform.position.x, transform.position.y, transform.position.z, 1);
            Vector4 posNewCoord = trs.inverse * originPos;
            posNewCoord /= posNewCoord.w;

            if (X) posNewCoord.x = 0;
            if (Y) posNewCoord.y = 0;
            if (Z) posNewCoord.z = 0;
        
            Vector4 newOriginCoord = trs * posNewCoord;
            newOriginCoord /= newOriginCoord.w;
            transform.position = new Vector3(newOriginCoord.x, newOriginCoord.y, newOriginCoord.z);
        }
    }
}

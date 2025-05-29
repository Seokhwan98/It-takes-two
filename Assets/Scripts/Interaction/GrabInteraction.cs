using System;
using Fusion;
using UnityEngine;

public class GrabInteractor : Interactor
{
    [SerializeField] private Transform _grabPoint;
    [SerializeField] private float _distance = 2f;
    [SerializeField] private Color _gizmoColor = Color.red;
    private Grabbable _grabbable;
    
    public Transform GrabPoint => _grabPoint;
    
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out MyNetworkInput data))
        {
            // Grab
            if (data.IsDown(MyNetworkInput.BUTTON_INTERACT))
            {
                if (_grabbable != null) return;
                
                if (Physics.Raycast(_grabPoint.position, _grabPoint.forward, out RaycastHit hit, _distance))
                {
                    var grabbable = hit.collider.GetComponent<Grabbable>();
                    if (grabbable)
                    {
                        grabbable.Interact(this);
                        _grabbable = grabbable;
                    }
                }
            }
            
            if (data.IsDown(MyNetworkInput.BUTTON_END_INTERACT))
            {
                if (_grabbable == null) return;
                
                _grabbable.FinishInteract(this);
                _grabbable = null;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawLine(_grabPoint.position, _grabPoint.position + _distance * _grabPoint.forward);
    }
}

using System.Collections;
using Fusion;
using UnityEngine;

public class GrabInteractor : Interactor
{
    [SerializeField] private NetworkAnimatorController _animatorController;
    
    [SerializeField] private Transform _grabPoint;
    [SerializeField] private float _distance = 2f;
    [SerializeField] private Color _gizmoColor = Color.red;
    
    private PlayerMovement _playerMovement;
    
    public Transform GrabPoint => _grabPoint;
    [Networked] public Grabbable Grabbable { get; set; }
    
    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }
    
    public override void FixedUpdateNetwork()
    {
        if (_playerMovement == null) return;
        
        if (GetInput(out MyNetworkInput data) && HasStateAuthority)
        {
            // Grab
            if (data.IsDown(MyNetworkInput.BUTTON_INTERACT))
            {
                if (Grabbable != null) return;
                
                if (Physics.Raycast(_grabPoint.position, _grabPoint.forward, out RaycastHit hit, _distance))
                {
                    var grabbable = hit.collider.GetComponent<Grabbable>();
                    if (grabbable)
                    {
                        bool result = grabbable.TryInteract(this);
                        if (result)
                        {
                            Grabbable = grabbable;
                        }
                    }
                }
            }
            
            if (data.IsDown(MyNetworkInput.BUTTON_END_INTERACT))
            {
                if (HasStateAuthority)
                {
                    if (Grabbable == null) return;
                
                    Grabbable.FinishInteract(this);
                    Grabbable = null;
                }
            }
        }
        
        _animatorController.RPC_SetBool(Constant.IsGrabbingHash, Grabbable != null);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawLine(_grabPoint.position, _grabPoint.position + _distance * _grabPoint.forward);
    }
}

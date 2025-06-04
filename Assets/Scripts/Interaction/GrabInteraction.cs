using System.Collections;
using Fusion;
using Unity.Cinemachine;
using UnityEngine;

public class GrabInteractor : Interactor
{
    [SerializeField] private NetworkAnimatorController _animatorController;
    
    [SerializeField] private Transform _grabPoint;
    [SerializeField] private float _distance = 2f;
    [SerializeField] private Color _gizmoColor = Color.red;
    
    private PlayerMovement _playerMovement;
    
    public Transform GrabPoint => _grabPoint;
    [Networked, OnChangedRender(nameof(OnChangeGrabbable))] public Grabbable Grabbable { get; set; }
    
    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }
    
    public override void FixedUpdateNetwork()
    {
        if (_playerMovement == null) return;

        Grabbable grabbable = null;
        RaycastHit hit = default;
        if (Grabbable == null && Physics.Raycast(_grabPoint.position, _grabPoint.forward, out hit, _distance))
        {
            grabbable = hit.collider.GetComponent<Grabbable>();
        }
        
        if (GetInput(out MyNetworkInput data) && HasStateAuthority)
        {
            if (grabbable != null)
            {
                if (data.IsDown(MyNetworkInput.BUTTON_INTERACT))
                {
                    bool result = grabbable.TryInteract(this);
                    if (result)
                    {
                        Grabbable = grabbable;
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
        
        if (Grabbable != null)
        {
            _playerMovement.InteractionUIUpdater?.SetActiveEndInteractionUI(true);
            _playerMovement.InteractionUIUpdater?.SetActiveInteractionUI(false);
        }
        else
        {
            _playerMovement.InteractionUIUpdater?.SetActiveEndInteractionUI(false);
            if (grabbable != null)
            {
                _playerMovement.InteractionUIUpdater?.SetActiveInteractionUI(true);
                _playerMovement.InteractionUIUpdater?.SetInteractionUIPositionWorld(hit.point);
            }
            else
            {
                _playerMovement.InteractionUIUpdater?.SetActiveInteractionUI(false);
            }
        }
    }

    private void OnChangeGrabbable()
    {
        _animatorController.RPC_SetBool(Constant.IsGrabbingHash, Grabbable);
        var interactionUIUpdater = _playerMovement.InteractionUIUpdater;
        interactionUIUpdater.SetActiveEndInteractionUI(Grabbable);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawLine(_grabPoint.position, _grabPoint.position + _distance * _grabPoint.forward);
    }
}

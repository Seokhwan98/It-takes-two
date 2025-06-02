using System.Collections;
using UnityEngine;

public class GrabInteractor : Interactor
{
    [SerializeField] private NetworkAnimatorController _animatorController;
    
    [SerializeField] private Transform _grabPoint;
    [SerializeField] private float _distance = 2f;
    [SerializeField] private Color _gizmoColor = Color.red;
    
    private PlayerData _playerData;
    
    public Transform GrabPoint => _grabPoint;

    private IEnumerator Start()
    {
        var playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement == null) yield break;
        
        yield return new WaitUntil(() => playerMovement.PlayerData != null);
        _playerData = playerMovement.PlayerData;
    }
    
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out MyNetworkInput data))
        {
            // Grab
            if (data.IsDown(MyNetworkInput.BUTTON_INTERACT))
            {
                if (_playerData.Grabbable != null) return;
                
                if (Physics.Raycast(_grabPoint.position, _grabPoint.forward, out RaycastHit hit, _distance))
                {
                    var grabbable = hit.collider.GetComponent<Grabbable>();
                    if (grabbable)
                    {
                        Debug.Log(grabbable.gameObject.name);
                        bool result = grabbable.TryInteract(this);
                        if (result)
                        {
                            _playerData.Grabbable = grabbable;
                        }
                    }
                }
            }
            
            if (data.IsDown(MyNetworkInput.BUTTON_END_INTERACT))
            {
                if (_playerData.Grabbable == null) return;
                
                _playerData.Grabbable.FinishInteract(this);
                _playerData.Grabbable = null;
            }
        }
        
        _animatorController.RPC_SetBool(Constant.IsGrabbingHash, _playerData?.Grabbable != null);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawLine(_grabPoint.position, _grabPoint.position + _distance * _grabPoint.forward);
    }
}

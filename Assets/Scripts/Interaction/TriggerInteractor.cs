using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerInteractor: Interactor
{
    [Networked] private ATriggerInteractable TriggerInteractable { get; set; }
    
    private PlayerMovement _playerMovement;
    
    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    public void Update()
    {
        if (TriggerInteractable != null && TriggerInteractable.IsInteractable(this))
        {
            var interactionUIUpdater = _playerMovement.InteractionUIUpdater;
            interactionUIUpdater?.SetActiveTriggerInteractionUI(true);
            interactionUIUpdater?.SetTriggerInteractionUIPositionWorld(TriggerInteractable.transform.position);    
        }
        else
        {
            var interactionUIUpdater = _playerMovement.InteractionUIUpdater;
            interactionUIUpdater?.SetActiveTriggerInteractionUI(false);
        }
    }
    
    public override void FixedUpdateNetwork()
    {
        if (_playerMovement == null) return;
        
        if (HasStateAuthority && GetInput(out MyNetworkInput data))
        {
            if (data.IsDown(MyNetworkInput.BUTTON_INTERACT))
            {
                TriggerInteractable?.TryInteract(this);
            }

            if (data.IsDown(MyNetworkInput.BUTTON_END_INTERACT))
            {
                TriggerInteractable?.FinishInteract(this);
            }
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority) return;
        
        ATriggerInteractable interactable = other.GetComponent<ATriggerInteractable>();
        interactable ??= other.GetComponentInParent<ATriggerInteractable>();
        
        if (interactable == null) return;
        
        if (TriggerInteractable == null)
        {
            TriggerInteractable = interactable;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (!HasStateAuthority) return;
        
        ATriggerInteractable interactable = other.GetComponent<ATriggerInteractable>();
        interactable ??= other.GetComponentInParent<ATriggerInteractable>();
    
        if (interactable == null) return;
        
        if (TriggerInteractable == interactable)
        {
            TriggerInteractable = null;
        }
    }
}

using Fusion;
using UnityEngine;

public class GrabInteractor : Interactor
{
    [SerializeField] private Transform _grabPoint;
    private Grabbable _grabbable;
    
    public Transform GrabPoint => _grabPoint;
    

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            // Grab
            if (data.networkButtons.IsSet(NetworkInputData.GrabButton))
            {
                if (_grabbable != null) return;
                
                if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out RaycastHit hit))
                {
                    var grabbable = hit.collider.GetComponent<Grabbable>();
                    if (grabbable)
                    {
                        grabbable.Interact(this);
                        _grabbable = grabbable;
                    }
                }
            }
            
            if (data.networkButtons.IsSet(NetworkInputData.FreeButton))
            {
                if (_grabbable == null) return;
                
                _grabbable.FinishInteract(this);
                _grabbable = null;
            }
        }
    }
}

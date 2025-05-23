using Fusion;
using UnityEngine;

public class Grabbable : NetworkBehaviour, IInteraction
{
    protected Rigidbody _rb;

    protected GrabInteractor currentInteractor = null;
    [SerializeField] protected Vector3 localOffset = Vector3.zero;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public override void FixedUpdateNetwork()
    {
        if (currentInteractor != null && HasStateAuthority)
        {
            UpdateGrabPosition();
        }
    }

    public virtual void Interact(GrabInteractor interactor)
    {
        if (HasStateAuthority)
        {
            this.currentInteractor = interactor;
            UpdateGrabPosition();
            if (_rb != null)
            {
                _rb.isKinematic = true;
            }
        }
    }

    public virtual void FinishInteract(GrabInteractor interactor)
    {
        if (HasStateAuthority)
        {
            this.currentInteractor = null;
            if (_rb != null)
            {
                _rb.isKinematic = false;
            }
        }
    }

    private void UpdateGrabPosition()
    {
        Matrix4x4 matrix = currentInteractor.GrabPoint.localToWorldMatrix;
        Vector4 offsetTemp = new Vector4(localOffset.x, localOffset.y, localOffset.z, 1);
        Vector4 posTemp = matrix * offsetTemp;
        Vector3 newPosition = new Vector3(posTemp.x, posTemp.y, posTemp.z) / posTemp.w;
        transform.position = newPosition;
    }
}

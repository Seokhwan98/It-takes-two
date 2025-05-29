using UnityEngine;

public class Ball : Grabbable
{
    [SerializeField] private Transform _model;
    
    private Vector3 _lastPosition;
    private bool _roll;

    public void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    public override void Interact(GrabInteractor interactor)
    {
        if (HasStateAuthority)
        {
            base.Interact(interactor);
            _lastPosition = transform.position;
        }
    }
    
    public override void FinishInteract(GrabInteractor interactor)
    {
        if (HasStateAuthority)
        {
            base.FinishInteract(interactor);
            _roll = false;
        }
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        
        if (currentInteractor != null && HasStateAuthority)
        {
            if (!_roll)
            {
                _roll = true;
                return;
            }

            Roll();
        }
        
        _lastPosition =  transform.position;
    }
    
    private void Roll()
    {
        Vector3 velocity = (transform.position - _lastPosition) / Runner.DeltaTime;
            
        if (velocity.magnitude <= 0f) return;
            
        Vector3 roundAxis = Vector3.Cross(Vector3.up, velocity);
        
        float angularSpeed = 360f / _model.lossyScale.x * velocity.magnitude;
        transform.RotateAround(_model.position, roundAxis, angularSpeed * Runner.DeltaTime);
    }
}

using Fusion;
using Fusion.Addons.KCC;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private Transform _grabPoint;
    
    private KCC _KCC;
    private Grabbable _grabbable;
    
    public Transform GrabPoint => _grabPoint;

    private void Awake()
    {
        _KCC = GetComponent<KCC>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            // Movement
            data.direction.Normalize();
            _KCC.SetInputDirection(5f * data.direction);
        }
    }
}

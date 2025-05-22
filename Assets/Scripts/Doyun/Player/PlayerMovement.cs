using Fusion;
using Fusion.Addons.KCC;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour {
    // private NetworkBool _canMove { get; set; } = true;
    [SerializeField] private GameObject freeLookPrefab;
    [SerializeField] private Transform cameraPivot;
    
    private GameObject _camInstance;
    
    private KCC _cc;
    private Vector3 _dir;
    
    // private NetworkButtons _prev;

    [Networked] private TickTimer _moveTimer { get; set; }

    public override void Spawned() {
        _cc = GetComponent<KCC>();

        if (Object.HasInputAuthority) {
            _camInstance = Instantiate(freeLookPrefab);
            var freeLook = _camInstance.GetComponent<CinemachineCamera>();
            
            freeLook.Follow = cameraPivot;
            freeLook.LookAt = cameraPivot;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.IsProxy) return;
        
        // UpdateMoveCD();

        Vector3 camForward = _camInstance.transform.forward;
        Vector3 camRight = _camInstance.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();
        
        if (GetInput<MyNetworkInput>(out var input))
        {
            _dir = Vector3.zero;

            if (input.IsDown(MyNetworkInput.BUTTON_FORWARD)) _dir += camForward;
            else if (input.IsDown(MyNetworkInput.BUTTON_BACKWARD)) _dir -= camForward;

            if (input.IsDown(MyNetworkInput.BUTTON_RIGHT)) _dir += camRight;
            else if (input.IsDown(MyNetworkInput.BUTTON_LEFT)) _dir -= camRight;
            
            _cc.SetInputDirection(_dir.normalized);

            if (_dir.sqrMagnitude > 0.001f)
            {
                Quaternion rot = Quaternion.LookRotation(_dir);
                _cc.SetLookRotation(rot);
            }
            // _prev = input.Buttons;
        }
    }
    
    // public void SetCanMove(float time)
    // {
    //     _moveTimer = TickTimer.CreateFromSeconds(Runner, time);
    // }
    //
    // private void UpdateMoveCD()
    // {
    //     _canMove = _moveTimer.ExpiredOrNotRunning(Runner);
    // }
}
using Fusion;
using Fusion.Addons.KCC;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour {
    private NetworkBool _canMove { get; set; } = true;
    private KCC _cc;
    private Vector3 _dir;
    private NetworkButtons _prev;

    [Networked] private TickTimer _moveTimer { get; set; }

    public override void Spawned()
    {
        _cc = GetComponent<KCC>();
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.IsProxy) return;
        
        // UpdateMoveCD();
        
        if (GetInput<MyNetworkInput>(out var input))
        {
            _dir = Vector3.zero;

            if (input.IsDown(MyNetworkInput.BUTTON_FORWARD)) _dir += Vector3.forward;
            else if (input.IsDown(MyNetworkInput.BUTTON_BACKWARD)) _dir -= Vector3.forward;

            if (input.IsDown(MyNetworkInput.BUTTON_RIGHT)) _dir += Vector3.right;
            else if (input.IsDown(MyNetworkInput.BUTTON_LEFT)) _dir -= Vector3.right;
            
            _cc.SetInputDirection(_dir.normalized);

            if (_dir.sqrMagnitude > 0.001f)
            {
                Quaternion rot = Quaternion.LookRotation(_dir);
                _cc.SetLookRotation(rot);
            }
            _prev = input.Buttons;
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
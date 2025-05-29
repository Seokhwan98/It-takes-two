using Fusion;
using Fusion.Addons.KCC;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour {
    private NetworkBool _canMove { get; set; } = true;
    [SerializeField] private GameObject freeLookPrefab;
    [SerializeField] private Transform cameraPivot;
    
    private GameObject _camInstance;
    
    private KCC _cc;
    private Vector3 _dir;
    
    private PlayerData _playerData;
    public PlayerData PlayerData => _playerData;
    
    [Networked] private TickTimer _moveTimer { get; set; }

    public override void Spawned()
    {
        _cc = GetComponent<KCC>();

        if (Object.HasInputAuthority) {
            _camInstance = Instantiate(freeLookPrefab);
            var freeLook = _camInstance.GetComponent<CinemachineCamera>();
            
            freeLook.Follow = cameraPivot;
            freeLook.LookAt = cameraPivot;
        }
        
        int playerId = Runner.LocalPlayer.PlayerId;
        _playerData = new PlayerData(/*playerId*/1);
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority) return;
        
        UpdateMoveCD();
        
        if (!_canMove) return;
        
        if (GetInput<MyNetworkInput>(out var input))
        {
            Vector3 camForward = input.forward;
            Vector3 camRight = input.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();
            
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

            if (input.IsDown(MyNetworkInput.BUTTON_RUN))
            {
                _playerData.Running = true;
            }
            else
            {
                _playerData.Running = false;
            }
            
            if (input.IsDown(MyNetworkInput.BUTTON_JUMP))
            {
                _playerData.TriggerJump = true;
            }
        }
    }
    
    public void DisableMovementForSeconds(float time)
    {
        _moveTimer = TickTimer.CreateFromSeconds(Runner, time);
    }
    
    public void SetCanMove(bool canMove)
    {
        _canMove = canMove;
    }
    
    private void UpdateMoveCD()
    {
        _canMove = _moveTimer.ExpiredOrNotRunning(Runner);
    }
}
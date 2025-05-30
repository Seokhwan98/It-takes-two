using System;
using Fusion;
using Fusion.Addons.KCC;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour {
    private NetworkBool _canMove { get; set; } = true;
    private NetworkBool _canJump { get; set; } = true;
    [SerializeField] private GameObject freeLookPrefab;
    [SerializeField] private Transform cameraPivot;
    
    private GameObject _camInstance;
    private CinemachineOrbitalFollow _myOrbitalFollow;
    
    private Animator _animator;
    private bool _wasGrounded = true;
    private bool _wasWall = false;
    
    private KCC _cc;
    private Vector3 _dir;
    private float camSpeed = 20f;
    
    private float smoothYaw;
    private float smoothPitch;
    private float smoothSpeed = 15f;
    
    [Networked] 
    public float NetworkYaw { get; set; }
    
    [Networked] 
    public float NetworkPitch { get; set; }
    
    private PlayerData _playerData;
    public PlayerData PlayerData => _playerData;
    
    [Networked] private TickTimer _moveTimer { get; set; }
    [Networked] private TickTimer _jumpTimer { get; set; }
    
    private Action _playJumpTimer; 

    public override void Spawned()
    {
        _cc = GetComponent<KCC>();
        _animator = GetComponent<Animator>();
        
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        
        int playerId = Object.InputAuthority.PlayerId;
        _playerData = new PlayerData(playerId);

        _playJumpTimer = () => _jumpTimer = TickTimer.CreateFromSeconds(Runner, 0.2f);
        _playerData.JumpTrigger.OnShot += _playJumpTimer;
        
        if (Runner.GameMode is GameMode.Shared)
        {
            if (HasStateAuthority) 
            {
                _camInstance = Instantiate(freeLookPrefab);
                var freeLook = _camInstance.GetComponent<CinemachineCamera>();
            
                freeLook.Follow = cameraPivot;
                freeLook.LookAt = cameraPivot;
            }
        }
        
        else if (Runner.GameMode is GameMode.Client or GameMode.Host)
        {
            if (currentScene == "RoomScene") return;
            if(HasInputAuthority)  TryBindMyCamera();
            else TryBindOtherCamera();
        }
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        _playerData.JumpTrigger.OnShot -= _playJumpTimer;
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority) return;
        
        _playerData.ReleaseAllTrigger();
        
        UpdateMoveCD();
        UpdateJumpCD();
        
        if (!_canMove) return;
        
        if (GetInput<MyNetworkInput>(out var input))
        {
            Vector3 camForward = _camInstance.transform.forward;
            Vector3 camRight = _camInstance.transform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();
            
            _dir = Vector3.zero;

            if (input.IsDown(MyNetworkInput.BUTTON_FORWARD)) _dir += camForward;
            else if (input.IsDown(MyNetworkInput.BUTTON_BACKWARD)) _dir -= camForward;
            if (input.IsDown(MyNetworkInput.BUTTON_RIGHT)) _dir += camRight;
            else if (input.IsDown(MyNetworkInput.BUTTON_LEFT)) _dir -= camRight;
            
            if (HasStateAuthority)
            {
                NetworkYaw -= input.LookYaw * camSpeed;
                NetworkPitch += input.LookPitch * camSpeed;
                
                NetworkYaw = Mathf.Clamp(NetworkYaw, -10f, 45f);
            }

            _cc.SetInputDirection(_dir.normalized);

            if (_dir.sqrMagnitude > 0.001f)
            {
                Quaternion rot = Quaternion.LookRotation(_dir);
                _cc.SetLookRotation(rot);
            }
            
            _playerData.Running = input.IsDown(MyNetworkInput.BUTTON_RUN);
            
            Debug.Log(_canJump);
            if (_canJump && input.IsDown(MyNetworkInput.BUTTON_JUMP))
            {
                _playerData.JumpTrigger.Ready();
            }
            
            if (input.IsDown(MyNetworkInput.BUTTON_LEFTCLICK))
            {
                _playerData.SmallerTrigger.Ready();
                _cc.ExecuteStage<ISetScale>();
            }
            else if (input.IsDown(MyNetworkInput.BUTTON_RIGHTCLICK))
            {
                _playerData.BiggerTrigger.Ready();
                _cc.ExecuteStage<ISetScale>();
            }
        }
    }
    
    public override void Render()
    {
        UpdateAnimator();
        
        if (_myOrbitalFollow == null) return;
        smoothYaw = Mathf.Lerp(smoothYaw, NetworkYaw, Time.deltaTime * smoothSpeed);
        smoothPitch = Mathf.Lerp(smoothPitch, NetworkPitch, Time.deltaTime * smoothSpeed);

        _myOrbitalFollow.VerticalAxis.Value = smoothYaw;
        _myOrbitalFollow.HorizontalAxis.Value = smoothPitch;
    }
    
    private void UpdateAnimator()
    {
        Vector3 moveSpeed = _cc.Data.RealVelocity;
        moveSpeed = new Vector3(moveSpeed.x, 0, moveSpeed.z);
        float speed = moveSpeed.magnitude / _cc.Data.KinematicSpeed / 2f;
        
        _animator.SetFloat("speed", speed);

        bool isGrounded = _cc.Data.IsGrounded;
        _animator.SetBool("isGrounded", isGrounded);
        
        bool isWall = _playerData.Wall;
        _animator.SetBool("isWall", isWall);
        
        if ((!isGrounded && _wasGrounded) || (!isWall && _wasWall))
        {
            _animator.SetTrigger("jump");
        }
        _wasGrounded = isGrounded;
        _wasWall = isWall;
        
        bool isGrabbing = _playerData.Grabbable != null;
        _animator.SetBool("isGrabbing", isGrabbing);
    }
    
    public void TryBindMyCamera()
    {
        if (HasInputAuthority)
        {
            var MyChannel = (int)Object.InputAuthority.PlayerId;

            var camSet = CameraHolder.Instance.GetCameraSet(MyChannel);

            if (camSet != null)
            {
                camSet.Camera.Follow = cameraPivot;
                camSet.Camera.LookAt = cameraPivot;

                _camInstance = camSet.Camera.gameObject;

                _myOrbitalFollow = _camInstance.GetComponent<CinemachineOrbitalFollow>();

                var axisController = _camInstance.GetComponent<CinemachineInputAxisController>();
                if (axisController != null)
                {
                    axisController.enabled = false;
                }
            }
        }
    }
    
    public void TryBindOtherCamera()
    {
        if(!HasInputAuthority)
        {
            var MyChannel = (int)Object.InputAuthority.PlayerId == 1 ? 1 : 2;
            var camSet = CameraHolder.Instance.GetCameraSet(MyChannel);
            if (camSet != null)
            {
                camSet.Camera.Follow = cameraPivot;
                camSet.Camera.LookAt = cameraPivot;
                    
                _camInstance = camSet.Camera.gameObject;
                    
                _myOrbitalFollow = _camInstance.GetComponent<CinemachineOrbitalFollow>();
                    
                var axisController = _camInstance.GetComponent<CinemachineInputAxisController>();
                if (axisController != null)
                {
                    axisController.enabled = false;
                }
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
    
    private void UpdateJumpCD()
    {
        _canJump = _jumpTimer.ExpiredOrNotRunning(Runner);
    }
}
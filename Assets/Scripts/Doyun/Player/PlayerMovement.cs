using System;
using Fusion;
using Fusion.Addons.KCC;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour {
    // private NetworkBool _canMove { get; set; } = true;
    [SerializeField] private GameObject freeLookPrefab;
    [SerializeField] private Transform cameraPivot;
    
    private GameObject _camInstance;
    private CinemachineOrbitalFollow _myOrbitalFollow;
    
    private KCC _cc;
    private Vector3 _dir;
    private float speed = 20f;

    private float smoothYaw;
    private float smoothPitch;
    private float smoothSpeed = 15f;
    
    // , OnChangedRender(nameof(OnYawChanged))
    [Networked] 
    public float NetworkYaw { get; set; }
    
    // , OnChangedRender(nameof(OnPitchChanged))
    [Networked] 
    public float NetworkPitch { get; set; }
    
    
    [Networked] private TickTimer _moveTimer { get; set; }

    public override void Spawned() {
        _cc = GetComponent<KCC>();

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
            if(HasInputAuthority)  TryBindMyCamera();
            else TryBindOtherCamera();
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.IsProxy) return;
        // if (!HasInputAuthority) return;
        
        // UpdateMoveCD();
        
        // if (HasInputAuthority)
        // {
        //     if (_myOrbitalFollow != null)
        //     {
        //         NetworkYaw = _myOrbitalFollow.VerticalAxis.Value;
        //         NetworkPitch = _myOrbitalFollow.HorizontalAxis.Value;
        //     }
        // }

        
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
                NetworkYaw -= input.LookYaw * speed;
                NetworkPitch += input.LookPitch * speed;
                
                NetworkYaw = Mathf.Clamp(NetworkYaw, -10f, 45f);
                // NetworkPitch = NetworkPitch % 360f;
            }
            
            _cc.SetInputDirection(_dir.normalized);

            if (_dir.sqrMagnitude > 0.001f)
            {
                Quaternion rot = Quaternion.LookRotation(_dir);
                _cc.SetLookRotation(rot);
            }
        }
    }

    public override void Render()
    {
        // if (HasInputAuthority) return;
        if (_myOrbitalFollow == null) return;
        smoothYaw = Mathf.Lerp(smoothYaw, NetworkYaw, Time.deltaTime * smoothSpeed);
        smoothPitch = Mathf.Lerp(smoothPitch, NetworkPitch, Time.deltaTime * smoothSpeed);

        _myOrbitalFollow.VerticalAxis.Value = smoothYaw;
        _myOrbitalFollow.HorizontalAxis.Value = smoothPitch;
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
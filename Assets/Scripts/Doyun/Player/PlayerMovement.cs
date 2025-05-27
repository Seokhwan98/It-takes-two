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
    private GameObject _otherCamera;
    private CinemachineOrbitalFollow _myOrbitalFollow;
    private CinemachineOrbitalFollow _otherOrbitalFollow;
    
    private KCC _cc;
    private Vector3 _dir;
    
    //
    [Networked, OnChangedRender(nameof(OnYawChanged))] 
    public float NetworkYaw { get; set; }
    
    //
    
    [Networked, OnChangedRender(nameof(OnPitchChanged))] 
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
        
        if (HasInputAuthority)
        {
            if (_myOrbitalFollow != null)
            {
                NetworkYaw = _myOrbitalFollow.VerticalAxis.Value;
                NetworkPitch = _myOrbitalFollow.HorizontalAxis.Value;
            }
        }

        
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
            
            _cc.SetInputDirection(_dir.normalized);

            if (_dir.sqrMagnitude > 0.001f)
            {
                Quaternion rot = Quaternion.LookRotation(_dir);
                _cc.SetLookRotation(rot);
            }
        }
    }

    // public override void Render()
    // {
    //     if (!HasInputAuthority) return;
    //     if (_otherOrbitalFollow == null) return;
    //     _myOrbitalFollow.HorizontalAxis.Value = NetworkYaw;
    //     _myOrbitalFollow.VerticalAxis.Value = NetworkPitch;
    // }

    public void TryBindMyCamera()
    {
        if (HasInputAuthority)
        {
            var MyChannel = (int)Object.InputAuthority.PlayerId;
            var otherChannel = (MyChannel == 1) ? 2 : 1;

            var camSet = CameraHolder.Instance.GetCameraSet(MyChannel);
            var otherCamSet = CameraHolder.Instance.GetCameraSet(otherChannel);

            if (camSet != null)
            {
                camSet.Camera.Follow = cameraPivot;
                camSet.Camera.LookAt = cameraPivot;

                _camInstance = camSet.Camera.gameObject;

                _myOrbitalFollow = _camInstance.GetComponent<CinemachineOrbitalFollow>();
                _otherOrbitalFollow = otherCamSet.Camera.gameObject.GetComponent<CinemachineOrbitalFollow>();

                var axisController = _camInstance.GetComponent<CinemachineInputAxisController>();
                if (axisController != null)
                {
                    axisController.enabled = true;
                }
            }
        }
    }
    
    public void TryBindOtherCamera()
    {
        if(!HasInputAuthority)
        {
            var MyChannel = (int)Object.InputAuthority.PlayerId == 1 ? 1 : 2;
            var otherChannel = (MyChannel == 1) ? 2 : 1;
            var camSet = CameraHolder.Instance.GetCameraSet(MyChannel);
            var otherCamSet = CameraHolder.Instance.GetCameraSet(otherChannel);
            if (camSet != null)
            {
                camSet.Camera.Follow = cameraPivot;
                camSet.Camera.LookAt = cameraPivot;
                    
                _camInstance = camSet.Camera.gameObject;
                    
                _myOrbitalFollow = _camInstance.GetComponent<CinemachineOrbitalFollow>();
                _otherOrbitalFollow = otherCamSet.Camera.gameObject.GetComponent<CinemachineOrbitalFollow>();
                    
                var axisController = _camInstance.GetComponent<CinemachineInputAxisController>();
                if (axisController != null)
                {
                    axisController.enabled = false;
                }
            }
        }
    }
    
    private void OnYawChanged()
    {
        if (HasInputAuthority) return;
        if (_otherOrbitalFollow == null) return;
        _myOrbitalFollow.VerticalAxis.Value = NetworkYaw;
    }
    
    private void OnPitchChanged()
    {
        if (HasInputAuthority) return;
        if (_otherOrbitalFollow == null) return;
        _myOrbitalFollow.HorizontalAxis.Value = NetworkPitch;
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
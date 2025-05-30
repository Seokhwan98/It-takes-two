using Fusion;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : NetworkBehaviour
{
    private Animator _anim;
    
    public static int SpeedHash = Animator.StringToHash("speed");
    public static int IsGroundedHash = Animator.StringToHash("isGrounded");
    public static int JumpHash = Animator.StringToHash("jump");
    public static int IsWallHash = Animator.StringToHash("isWall");
    public static int IsGrabbingHash = Animator.StringToHash("isGrabbing");

    private float _speed;
    private bool _isGrounded;
    private Trigger _jump;
    private bool _isWall;
    private bool _isGrabbing;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        
        _speed = _anim.GetFloat(SpeedHash);
        _isGrounded = _anim.GetBool(IsGroundedHash);
        _jump = new Trigger();
        _isWall = _anim.GetBool(IsWallHash);
        _isGrabbing = _anim.GetBool(IsGrabbingHash);
    }

    public void SetSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }

    public void SetIsGrounded(bool newIsGrounded)
    {
        _isGrounded = newIsGrounded;
    }

    public void TriggerJump()
    {
        _jump.Ready();
    }

    public void SetIsWall(bool newIsWall)
    {
        _isWall = newIsWall;
    }

    public void SetIsGrabbing(bool newIsGrabbing)
    {
        _isGrabbing = newIsGrabbing;
    }

    public override void Render()
    {
        if (_jump.TryShot())
        {
            _anim.SetTrigger(JumpHash);
        }
        
        _anim.SetFloat(SpeedHash, _speed);
    }
}

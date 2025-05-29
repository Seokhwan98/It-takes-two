using Fusion.Addons.KCC;
using UnityEngine;

public class RunProcessor : KCCProcessor, ISetKinematicVelocity
{
    [SerializeField, Min(1f)] private float _runMultiplier = 2.5f;

    private readonly float DefaultPriority = default;
    public override float GetPriority(KCC kcc) => DefaultPriority;
    
    public void Execute(ISetKinematicVelocity stage, KCC kcc, KCCData data)
    {
        var playerData = kcc.GetComponent<PlayerMovement>().PlayerData;
        
        if (playerData is { Running: true })
        {
            ApplyRun(data);
        }
        
        SuppressOtherProcessors(kcc);
    }

    private void ApplyRun(KCCData data)
    {
        data.KinematicVelocity *= _runMultiplier;
    }

    private void SuppressOtherProcessors(KCC kcc)
    {
        kcc.SuppressProcessors<RunProcessor>();
    }
}

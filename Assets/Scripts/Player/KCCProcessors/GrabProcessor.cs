using Fusion.Addons.KCC;

public class GrabProcessor : KCCProcessor, ISetKinematicVelocity
{
    private readonly float DefaultPriority = 2000;
    public override float GetPriority(KCC kcc) => DefaultPriority;
    
    public void Execute(ISetKinematicVelocity stage, KCC kcc, KCCData data)
    {
        var playerData = kcc.GetComponent<PlayerMovement>().PlayerData;
        var fixedData = kcc.FixedData;
        
        if (playerData.Grabbable != null)
        {
            fixedData.KinematicVelocity *= playerData.Grabbable.SpeedMultiplier;
        }
    }
}

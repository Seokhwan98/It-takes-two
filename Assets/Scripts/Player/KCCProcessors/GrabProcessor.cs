using Fusion.Addons.KCC;

public class GrabProcessor : KCCProcessor, ISetKinematicVelocity
{
    private readonly float DefaultPriority = -5000;
    public override float GetPriority(KCC kcc) => DefaultPriority;
    
    public void Execute(ISetKinematicVelocity stage, KCC kcc, KCCData data)
    {
        var playerData = kcc.GetComponent<PlayerMovement>().PlayerData;

        if (playerData.Grabbable != null)
        {
            data.KinematicVelocity *= playerData.Grabbable.SpeedMultiplier;
        }
    }
}

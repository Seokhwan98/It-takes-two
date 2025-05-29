using System;
using UnityEngine;

public class PlayerData
{
    public int PlayerID { get; private set; }
    public bool Running { get; set; }
    public Grabbable Grabbable { get; set; }
    public bool TriggerJump { get; set; }
    public bool WallJump { get; set; }
    public int AirJump { get; private set; }
    public EPlayerScale PlayerScale { get; private set; }

    public readonly int AirJumpPlayerID = 1;
    public readonly int ScalePlayerID = 2;

    // Constructor
    public PlayerData(int playerID)
    {
        PlayerID = playerID;
        Running = false;
        Grabbable = null;
        TriggerJump = false;
        WallJump = false;
        AirJump = playerID == AirJumpPlayerID ? 1 : 0;
        PlayerScale = EPlayerScale.Normal;
    }
    
    public void ApplyAirJump()
    {
        AirJump--;
    }
    
    public void ResetAirJump()
    {
        if (PlayerID != AirJumpPlayerID) return;

        AirJump = 1;
    }

    public void Smaller()
    {
        if (PlayerID != ScalePlayerID) return;
        
        PlayerScale = PlayerScale switch
        {
            EPlayerScale.Big => EPlayerScale.Normal,
            _ => EPlayerScale.Small
        };
    }

    public void Bigger()
    {
        if (PlayerID != ScalePlayerID) return;
        
        PlayerScale = PlayerScale switch
        {
            EPlayerScale.Small => EPlayerScale.Normal,
            _ => EPlayerScale.Big
        };
    }
}

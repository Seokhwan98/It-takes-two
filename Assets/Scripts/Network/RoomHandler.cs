using System;
using System.Collections;
using Fusion;
using UnityEngine;

public class RoomHandler : NetworkBehaviour
{ 
    [SerializeField] private ConnectionData _data;
    
    public void OnClickStart()
    {
        Runner.SessionInfo.IsOpen = false;
        
        int targetSceneIndex = Runner.name switch
        {
            "FirstGame" => 3,
            "SecondGame" => 4,
            _ => throw new InvalidOperationException($"Unknown session name: {Runner.SessionInfo.Name}")
        };
        
        Runner.LoadScene(SceneRef.FromIndex(targetSceneIndex));
    }

    public void OnClickExit()
    {
        _ = ConnectionManager.Instance.ConnectToRunner(_data);
    }
}
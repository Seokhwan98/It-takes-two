using Fusion;
using UnityEngine;

public class RoomHandler : NetworkBehaviour
{ 
    [SerializeField] private ConnectionData _data;
    
    public void OnClickStart()
    {
        Runner.SessionInfo.IsOpen = false;
        Runner.LoadScene(SceneRef.FromIndex(3));
    }

    public void OnClickExit()
    {
        _ = ConnectionManager.Instance.ConnectToRunner(_data);
    }
}
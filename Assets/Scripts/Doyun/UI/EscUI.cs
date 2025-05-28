using Fusion;
using UnityEngine;

public class EscUI : UIScreen
{
    [SerializeField] private ConnectionData _lobbyConnectionData;
    
    public async void OnClickQuit()
    {
        await ConnectionManager.Instance.ConnectToRunner(_lobbyConnectionData);
        
        // if (ConnectionManager.Instance.IsGameHost())
        //     ConnectionManager.Instance.LoadGameLevel(_connectionData);

        // var _lobbyConnection = ConnectionManager.Instance.GetLobbyConnection();
        //
        // _lobbyConnection.Runner.LoadScene(SceneRef.FromIndex(_lobbyConnection.ActiveConnection.SceneIndex));
        
        Defocus();
    }
    
    public void OnClickContinue()
    {
        Defocus();
    }
}
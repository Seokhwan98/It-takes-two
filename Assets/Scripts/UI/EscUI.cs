using UnityEngine;
using UnityEngine.SceneManagement;

public class EscUI : UIScreen
{
    [SerializeField] private ConnectionData _lobbyConnectionData;
    
    public async void OnClickQuit()
    {
        Defocus();
        
        Scene activeScene = SceneManager.GetActiveScene();

        if (activeScene.name == "LobbyScene")
        {
            SceneManager.LoadScene("StartScene");
            
            var lobbyConnection = ConnectionManager.Instance.GetLobbyConnection();
            var lobbyRunner  = lobbyConnection.Runner;

            if (lobbyRunner != null && lobbyRunner.IsRunning)
            {
                await lobbyRunner.Shutdown();
            }
            
            InterfaceManager.Instance.MouseEnable();
        }
        else
        {
            await ConnectionManager.Instance.ConnectToRunner(_lobbyConnectionData);
        }

    }

    public void OnClickContinue()
    {
        Defocus();
    }
}
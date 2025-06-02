using UnityEngine;

public class EscUI : UIScreen
{
    [SerializeField] private ConnectionData _lobbyConnectionData;
    
    public async void OnClickQuit()
    {
        await ConnectionManager.Instance.ConnectToRunner(_lobbyConnectionData);
            
        Defocus();
    }
    
    public void OnClickContinue()
    {
        Defocus();
    }
}
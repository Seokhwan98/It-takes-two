using UnityEngine;

public class GateUI : UIScreen
{
    [SerializeField] private ConnectionData _connectionData;
    
    public async void OnClickYes()
    {
        await ConnectionManager.Instance.ConnectToRunner(_connectionData);
            
        if (ConnectionManager.Instance.IsGameHost())
            ConnectionManager.Instance.LoadGameLevel(_connectionData);
        
        Defocus();
    }
    
    public void OnClickNo()
    {
        Defocus();
    }
}
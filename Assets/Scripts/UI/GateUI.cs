using Cysharp.Threading.Tasks;
using UnityEngine;

public class GateUI : UIScreen
{
    private ConnectionData _connectionData;
    
    private void OnEnable()
    {
        ConnectionPortal.OnPortalTriggered += HandlePortalTriggered;
    }

    private void OnDisable()
    {
        ConnectionPortal.OnPortalTriggered -= HandlePortalTriggered;
    }
    
    private void HandlePortalTriggered(ConnectionData data)
    {
        _connectionData = data;

        // 데이터 바인딩 및 애니메이션 등 비동기 작업이 있다면 여기에
        Debug.Log("연결 데이터 준비 완료: " + data.name); 
    }
    
    public async void OnClickYes()
    {
        Debug.Log(_connectionData);
        
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
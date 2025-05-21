using Fusion;

public class App : NetworkBehaviour
{
    public override void Spawned()
    {
        DontDestroyOnLoad(this);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_ShutdownRunner(ConnectionData.ConnectionTarget target)
    {
        InterfaceManager.Instance.ClearInterface();
        
        var connection = ConnectionManager.Instance.GetLobbyConnection();
        
        if (connection.IsRunning)
            connection.Runner.Shutdown();
    }
}
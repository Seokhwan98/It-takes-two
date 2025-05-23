using Cysharp.Threading.Tasks;
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
        // InterfaceManager.Instance.ClearInterface();

        var connection = target == ConnectionData.ConnectionTarget.Lobby
            ? ConnectionManager.Instance.GetLobbyConnection()
            : ConnectionManager.Instance.GetGameConnection();

        if (connection.IsRunning)
        {
            var ins = ConnectionManager.Instance;
            DelayShutdown(ins.GetLobbyConnection(), ins.GetGameConnection()).Forget();
        }
    }
    
    private async UniTaskVoid DelayShutdown(ConnectionContainer lobby, ConnectionContainer game)
    {
        await UniTask.WaitUntil(() => game.Runner != null && game.IsRunning);
        await lobby.Runner.Shutdown();
    }
}
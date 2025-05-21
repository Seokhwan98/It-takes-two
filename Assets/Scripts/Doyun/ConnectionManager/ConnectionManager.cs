using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fusion;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public static ConnectionManager Instance { get; private set; }
    
    public ConnectionContainer GetLobbyConnection() => _lobbyConnection;
    
    [SerializeField] private App _app;
    
    private ConnectionContainer _lobbyConnection = new ConnectionContainer();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public async Task ConnectToRunner(ConnectionData connectionData, Action<NetworkRunner> onInitialized = default, Action<ShutdownReason> onFailed = default)
    {
        var connection = _lobbyConnection;
        connection.ActiveConnection = connectionData;
        
        var gameMode = GameMode.Shared;
        
        var sceneInfo = new NetworkSceneInfo();
        sceneInfo.AddSceneRef(SceneRef.FromIndex(connectionData.SceneIndex));
        
        var sessionProperties = new Dictionary<string, SessionProperty>()
            { { "ID", (int)connectionData.ID } };
        
        if (connection.Runner == default)
        {
            var child = new GameObject(connection.ActiveConnection.ID.ToString());
            child.transform.SetParent(transform);
            connection.Runner = child.AddComponent<NetworkRunner>();
        }

        if (connection.Callback == default)
        {
            connection.Callback = new ConnectionCallbacks();
        }
        
        if (connection.IsRunning)
        {
            Debug.Log("Shutdown");
            await connection.Runner.Shutdown();
        }
        connection.Runner.AddCallbacks(connection.Callback);
        
        onInitialized += runner =>
        {
            if (runner.IsServer || runner.IsSharedModeMasterClient)
            {
                connection.App = runner.Spawn(_app);
            }
        };
        
        var startResult = await connection.Runner.StartGame(new StartGameArgs()
        { 
            GameMode = gameMode,
            SessionProperties = sessionProperties,
            EnableClientSessionCreation = true,
            Scene = sceneInfo, PlayerCount = connectionData.MaxClients,
            OnGameStarted = onInitialized,
            SceneManager = connection.Runner.gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        if (!startResult.Ok)
            onFailed?.Invoke(startResult.ShutdownReason);
    }
}

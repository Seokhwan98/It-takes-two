using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class CustomSpawner : NetworkBehaviour
{
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    
    private NetworkRunner _networkRunner;
    private CustomCallbacks _callbacks;

    async void StartGame(GameMode mode)
    {
        try
        {
            // Create the Fusion runner and let it know that we will be providing user input
            var runnerGO = new GameObject("NetworkRunner");
            _networkRunner = runnerGO.AddComponent<NetworkRunner>();

            _callbacks = new CustomCallbacks();
            _callbacks.ActionOnPlayerJoined = CustomPlayerJoined;
            _callbacks.ActionOnPlayerLeft = CustomPlayerLeft;
            _callbacks.ActionOnShutdown = CustomShutdown;

            _networkRunner.AddCallbacks(_callbacks);

            // Create the NetworkSceneInfo from the current scene
            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);

            var sceneInfo = new NetworkSceneInfo();
            if (scene.IsValid)
            {
                sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
            }

            // Start or join (depends on gamemode) a session with a specific name
            await _networkRunner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = "TestRoom2",
                Scene = scene,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[StartGame] Exception occurred: {e.GetType().Name} - {e.Message}\n{e.StackTrace}");
        }
    }
    
    private void OnGUI()
    {
        if (_networkRunner == null)
        {
            if (GUI.Button(new Rect(0,0,200,40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0,40,200,40), "Join"))
            {
                StartGame(GameMode.AutoHostOrClient);
            }
        }
    }
    
    
    private void CustomPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            // Create a unique position for the player
            Vector3 spawnPosition = Vector3.up;
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            // Keep track of the player avatars for easy access
            _spawnedCharacters.Add(player, networkPlayerObject);
            
            // 📌 새로 들어온 플레이어가 기존 플레이어들을 바라볼 수 있도록 설정
            foreach (var kvp in _spawnedCharacters)
            {
                var playerMovement = kvp.Value.GetComponent<PlayerMovement>();
                // playerMovement?.TryBindOtherCamera();
            }
        }
    }

    private void CustomPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    private void CustomShutdown(NetworkRunner runner, ShutdownReason reason)
    {
        Debug.Log($"[CustomSpawner] Runner shutdown: {reason}");
    }
}

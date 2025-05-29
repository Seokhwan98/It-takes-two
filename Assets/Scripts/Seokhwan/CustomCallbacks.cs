using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class CustomCallbacks : INetworkRunnerCallbacks {
    public Action<NetworkRunner, PlayerRef> ActionOnPlayerJoined;
    public Action<NetworkRunner, PlayerRef> ActionOnPlayerLeft;
    public Action<NetworkRunner, ShutdownReason> ActionOnShutdown;

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"{player} Joined");
        Debug.Log($"total players: {runner.ActivePlayers.Count()}");
        
        ActionOnPlayerJoined?.Invoke(runner, player);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        ActionOnPlayerLeft?.Invoke(runner, player);
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        ActionOnShutdown?.Invoke(runner, shutdownReason);
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var inputs = new MyNetworkInput();

		// inputs.LookYaw = InputBuffer.Instance.LookYaw;
		// inputs.LookPitch = InputBuffer.Instance.LookPitch;
        
        if (Input.GetKey(KeyCode.W)) {
            inputs.Buttons.Set(MyNetworkInput.BUTTON_FORWARD, true);
        }
        
        if (Input.GetKey(KeyCode.S)) {
            inputs.Buttons.Set(MyNetworkInput.BUTTON_BACKWARD, true);
        }
        
        if (Input.GetKey(KeyCode.A)) {
            inputs.Buttons.Set(MyNetworkInput.BUTTON_LEFT, true);
        }
        
        if (Input.GetKey(KeyCode.D)) {
            inputs.Buttons.Set(MyNetworkInput.BUTTON_RIGHT, true);
        }
        
        if (Input.GetKey(KeyCode.E)) {
            inputs.Buttons.Set(MyNetworkInput.BUTTON_INTERACT, true);
        }
        
        if (Input.GetKey(KeyCode.Q)) {
            inputs.Buttons.Set(MyNetworkInput.BUTTON_END_INTERACT, true);
        }
        
        // if (Input.GetMouseButton(0)) {
        //     inputs.Buttons.Set(MyNetworkInput.BUTTON_FIRE, true);
        // }
        
        // 마우스 입력 (회전 값 추가)
        // inputs.LookYaw = Input.GetAxis("Mouse X");
        // inputs.LookPitch = Input.GetAxis("Mouse Y");
        //
        input.Set(inputs);
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) {}
    public void OnConnectedToServer(NetworkRunner runner) {}
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) {}
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) {}
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) {}
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) {}
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) {}
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) {}
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

    public void OnSceneLoadDone(NetworkRunner runner) {}
    public void OnSceneLoadStart(NetworkRunner runner) {}
}

using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Photon.Realtime;
using Fusion.Sockets;
using UnityEngine;

public class FusionLauncher : MonoBehaviour, INetworkRunnerCallbacks
{
    private GameManager gameManagerPrefab;
    private Action<NetworkRunner, ConnectionStatus, string> _connectionCallback;

    public enum ConnectionStatus
    {
        Disconnected,
        Failed,
        Connecting,
        Connected,
        Loading,
        Loaded
    }

    internal static void Launch(
        GameMode mode,
        string region,
        string room,
        GameManager sessionPrefab,
        LevelManager sceneLoader,
        Action<NetworkRunner, ConnectionStatus, string> onConnect)
    {
        FusionLauncher launcher = new GameObject("Launcher").AddComponent<FusionLauncher>();

        launcher.InternalLaunch(mode, region, room, sessionPrefab, sceneLoader, onConnect);
    }

    private async void InternalLaunch(GameMode mode, string region, string room,
        GameManager sessionPrefab,
        LevelManager sceneManager,
        Action<NetworkRunner, ConnectionStatus, string> onConnect)
    {
        gameManagerPrefab = sessionPrefab;
        _connectionCallback = onConnect;

        DontDestroyOnLoad(gameObject);

        NetworkRunner runner = gameObject.AddComponent<NetworkRunner>();
        // runner.name = name;
        // runner.ProvideInput = mode != GameMode.Server;

        // NetworkSceneInfo scene = new NetworkSceneInfo();
        // scene.AddSceneRef(SceneRef.FromIndex(sceneManager.GetActiveScene().buildIndex));

        // An empty region will use the best region.
        PhotonAppSettings.Global.AppSettings.FixedRegion = region;

        SetConnectionStatus(runner, ConnectionStatus.Connecting, "");

        await runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = room,
            SceneManager = sceneManager,
            // Scene = scene,
        });
    }

    public void SetConnectionStatus(NetworkRunner runner, ConnectionStatus status, string message)
    {
        _connectionCallback?.Invoke(runner, status, message);
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        GameManager session = runner.Spawn(gameManagerPrefab, Vector3.zero, Quaternion.identity);
        session.SetLocalPlayer(player);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        SetConnectionStatus(runner, ConnectionStatus.Connected, "");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        SetConnectionStatus(runner, ConnectionStatus.Disconnected, "");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        SetConnectionStatus(runner, ConnectionStatus.Failed, reason.ToString());
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }
}
using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Photon.Realtime;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wiviart.Utilities;
using Object = System.Object;

public class FusionLauncher : Singleton<FusionLauncher>, INetworkRunnerCallbacks
{
    #region INetworkRunnerCallbacks

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        players++;
        Debug.Log("Debugging: Player joined " + player.PlayerId + " " + players + " Local: " +
                  runner.LocalPlayer.PlayerId);
        userId = runner.UserId;
        sessionName = runner.SessionInfo.Name;

        if (players == settings.maxPlayers)
        {
            NetworkSceneManager.Instance.LoadLevel(runner, 1);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        players--;
        Debug.Log(
            "Debugging: Player left " + player.PlayerId + " " + players + " Local: " + runner.LocalPlayer.PlayerId);
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
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
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

    #endregion

    public void LaunchGame(GameMode gameMode, string region, string roomName)
    {
        if (gameMode != GameMode.Shared)
            this.gameObject.AddComponent<HitboxManager>();

        InternalLaunch(gameMode, region, roomName);
    }

    private async void InternalLaunch(GameMode mode, string region, string room)
    {
        _runner = this.gameObject.AddComponent<NetworkRunner>();
        DontDestroyOnLoad(_runner);
        _runner.ProvideInput = mode != GameMode.Server;

        // var scene = new NetworkSceneInfo();
        // var index = SceneManager.GetActiveScene().buildIndex;
        // scene.AddSceneRef(SceneRef.FromIndex(index));

        // An empty region will use the best region.
        // PhotonAppSettings.Global.AppSettings.FixedRegion = region;

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            PlayerCount = settings.maxPlayers,
            MatchmakingMode = MatchmakingMode.RandomMatching,
            SessionName = sessionName,
            // Scene = scene,
        });

        Debug.Log("Game started " + mode + " " + region + " " + room);
    }

    public FusionSetting settings;
    [Networked] public int players { set; get; }
    private NetworkRunner _runner;
    private string userId;
    private string sessionName;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _runner.SpawnAsync(settings.playerPrefab, Vector3.zero, Quaternion.identity);
    }

    public void Disconnect()
    {
        _runner.Shutdown(false);
        Destroy(_runner);
        players = 0;
    }
}
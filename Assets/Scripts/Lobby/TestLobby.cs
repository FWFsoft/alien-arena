using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using UnityEngine.Rendering;
using Unity.Services.Lobbies.Models;

public class TestLobby : MonoBehaviour
{
    private Lobby hostLobby;
    private float heartbeatTimer = 0;
    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        hostLobby = await CreateLobby("my super awesome lobby pt 2");
        ListLobbies();
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
        Debug.Log("yis");
    }

    private async void HandleLobbyHeartbeat()
    {
        try
        {
            if (hostLobby != null)
            {
                heartbeatTimer -= Time.deltaTime;
                if (heartbeatTimer < 0f)
                {
                    float heartbeatTimerMax = 15;
                    heartbeatTimer = heartbeatTimerMax;

                    await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
                    Debug.Log("hi");
                }
            }
        } catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async Task<Lobby> CreateLobby(string lobbyName = "myLobby")
    {
        try
        {
            int maxPlayers = 4;
            var lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);
            Debug.Log("Created lobby! " + lobby.Name + " " + lobby.MaxPlayers);
            return lobby;
        } catch (LobbyServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    private async void ListLobbies()
    {
        var queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
        Debug.Log("Lobbies found " + queryResponse.Results.Count);
        foreach(var lobby in queryResponse.Results)
        {
            Debug.Log(lobby.Name);
        }
    }

}

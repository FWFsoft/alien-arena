using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TestLobby : MonoBehaviour
{
    private static readonly string PLAYER_NAME_KEY = "PlayerName";
    private static readonly string GAME_MODE_KEY = "GameMode";
    private static readonly string MAP_KEY = "Map";

    [SerializeField]
    private InputField lobbyName;
    [SerializeField]
    private InputField lobbyId;
    [SerializeField]
    private Text lobbyInfoBody;

    private Lobby hostLobby;
    private Lobby joinedLobby;
    private float heartbeatTimer = 0;
    private float lobbyUpdateTimer = 0;
    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

    }

    private void Update()
    {
        HandleLobbyHeartbeat();
        PollForLobbyUpdates();
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
                    //Debug.Log("Inside lobby heartbeat");
                    await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
                }
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void PollForLobbyUpdates()
    {
        if (joinedLobby != null)
        {
            lobbyUpdateTimer -= Time.deltaTime;
            if (lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateTimerMax = 3f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;

                var lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;
            }
        }

    }

    public async void CreateLobby()
    {
        await CreateLobby(this.lobbyName.text);
    }

    public async Task<Lobby> CreateLobby(string lobbyName = "myLobby")
    {
        try
        {
            int maxPlayers = 4;
            var createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = getPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    {GAME_MODE_KEY, new DataObject(DataObject.VisibilityOptions.Public, "Training", DataObject.IndexOptions.S1) },
                    {MAP_KEY, new DataObject(DataObject.VisibilityOptions.Public, "zone_1", DataObject.IndexOptions.S2) }
                }
            };
            var lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);
            var lobbyInfo = "Created lobby! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode;
            setLobbyInfo(lobbyInfo);
            hostLobby = lobby;
            return lobby;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            return null;
        }
    }

    void setLobbyInfo(string lobbyInfo)
    {
        this.lobbyInfoBody.text = lobbyInfo;
        Debug.Log(lobbyInfo);
    }

    private async void UpdateLobbyGameMode(string gameMode)
    {
        try
        {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject> {
                    {GAME_MODE_KEY, new DataObject(DataObject.VisibilityOptions.Public, gameMode) }
                }
            });
            Debug.Log(hostLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private Player getPlayer(string playerName = null)
    {
        var realizedPlayerName = playerName != null ? playerName : "Player" + Random.Range(1, 10000).ToString();
        var player = new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
                {
                    {PLAYER_NAME_KEY, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, realizedPlayerName)}
                }
        };
        return player;
    }

    private async void UpdatePlayerName(string playerName)
    {
        await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
        {
            Data = getPlayer(playerName).Data
        });
    }


    public void JoinLobby()
    {
        JoinLobbyByCode(this.lobbyId.text);
    }

    private async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            var joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = getPlayer()
            };
            var lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode);
            PrintPlayers(lobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void QuickJoinLobby()
    {
        try { await LobbyService.Instance.QuickJoinLobbyAsync(); }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void ListLobbies()
    {
        var queryLobbiesOptions = new QueryLobbiesOptions
        {
            Count = 25,
            Filters = new List<QueryFilter> {
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
                //new QueryFilter(QueryFilter.FieldOptions.S1, "Training", QueryFilter.OpOptions.EQ)
            },
            Order = new List<QueryOrder>
            {
                new QueryOrder(false, QueryOrder.FieldOptions.Created)
            }
        };
        var queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);
        Debug.Log("Lobbies found " + queryResponse.Results.Count);
        foreach (var lobby in queryResponse.Results)
        {
            PrintPlayers(lobby);
        }
    }

    private void PrintPlayers(Lobby lobby)
    {
        var builder = new StringBuilder();
        builder.Append("Players in lobby " + lobby.Id + " " + lobby.Data[GAME_MODE_KEY].Value + " " + lobby.Data[MAP_KEY].Value);
        builder.Append("\n");
        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Id + " " + player.Data[PLAYER_NAME_KEY].Value);
            builder.Append("\n");
        }
        setLobbyInfo(builder.ToString());
    }

}

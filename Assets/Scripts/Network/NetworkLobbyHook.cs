using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        //base.OnLobbyServerSceneLoadedForPlayer(manager, lobbyPlayer, gamePlayer);

        LobbyPlayer lPlayer = lobbyPlayer.GetComponent<LobbyPlayer>();

        NetworkPlayerSetup playerSetup = gamePlayer.GetComponent<NetworkPlayerSetup>();
        if (playerSetup == null)
            return; 

        playerSetup.PlayerName = lPlayer.playerName;
        playerSetup.PlayerColor = lPlayer.playerColor;


        NetworkPlayerController playerController = gamePlayer.GetComponent<NetworkPlayerController>();
        if (playerController != null)
        {
            NetworkGameManager.AllPlayers.Add(playerController);
        }

    }


}

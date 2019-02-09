using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkPlayerSetup : NetworkBehaviour
{

    [SyncVar(hook = "UpdateColor")]
    public Color PlayerColor;
    [SyncVar(hook = "UpdateName")]
    public string PlayerName = "PLAYER";

    [SerializeField]
    private Text _playerNameText;
    [SerializeField]
    private MeshRenderer[] meshesToColour;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Camera.main.GetComponent<CameraFollow>().SetTarget(gameObject);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!isServer)
        {
            NetworkPlayerController pManager = GetComponent<NetworkPlayerController>();

            if (pManager != null)
            {
                NetworkGameManager.AllPlayers.Add(pManager);
            }
        }

        UpdateName(PlayerName);
        UpdateColor(PlayerColor);

    }

    void UpdateColor(Color pColor)
    {

        foreach (MeshRenderer r in meshesToColour)
        {
            //r.material.color = pColor;
            //TODO check colours are correct
            r.materials[0].color = pColor;
        }
    }

    void UpdateName(string name)
    {
        if (_playerNameText == null)
        {
            //TODO create coroutine to disable player name after a while?
            _playerNameText.enabled = false;
            return;
        }
        _playerNameText.text = name;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkPlayerSetup : NetworkBehaviour {

    [SerializeField]
    private Color playerColor;
    [SerializeField]
    private string playerName = "PLAYER";
    [SerializeField]
    private int playerNum = 1;
    [SerializeField]
    private Text playerNameText;
    [SerializeField]
    private MeshRenderer[] meshesToColour;

    private void Start()
    {
        /*
        if (!isLocalPlayer)
        {
            foreach (Behaviour component in componentsToDisable)
            {
                component.enabled = false;
            }
        }*/
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Camera.main.GetComponent<CameraFollow>().SetTarget(gameObject);

        for (int i = 0; i < meshesToColour.Length; i++)
        {
            meshesToColour[i].materials[0].color = playerColor;
        }

        if (playerNameText != null)
        {
            playerNameText.enabled = true;
            playerNameText.text = playerName + " "+ + playerNum;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (playerNameText != null)
        {
            playerNameText.enabled = true;
        }
       
    }
}

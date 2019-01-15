using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Prototype.NetworkLobby;

public class NetworkGameManager : NetworkBehaviour
{
    #region Singleton 
    static NetworkGameManager instance;

    public static NetworkGameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<NetworkGameManager>();

                if (instance == null)
                {
                    instance = new GameObject().AddComponent<NetworkGameManager>();
                }
            }
            return instance;
        }
    }
    #endregion

    
}

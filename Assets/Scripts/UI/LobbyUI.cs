using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LobbyUI : MonoBehaviour
{

    public void LoadSceneOnClick(int index)
    {
        //TODO close off any network connections if needed
        SceneManager.LoadScene(index);
    }
}

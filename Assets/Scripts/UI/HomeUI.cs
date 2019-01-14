using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 
/// </summary>
public class HomeUI : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    public void LoadSceneOnClick(int index)
    {
        SceneManager.LoadScene(index);
    }
}

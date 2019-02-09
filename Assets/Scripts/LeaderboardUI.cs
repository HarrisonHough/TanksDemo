using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour {

    [SerializeField]
    private Text[] _playerNameText;
    [SerializeField]
    private Text[] _playerScoreText;



    // Use this for initialization
    void Start () {
        ClearScoreBoard();

    }

    public void ClearScoreBoard()
    {
        for (int i = 0; i < _playerNameText.Length; i++)
        {
            _playerNameText[i].text =  "";
            _playerScoreText[i].text = "";
        }
    }

    public void UpdateScores(string[] playerNames, int[] playerScores)
    {
        for (int i = 0; i < playerNames.Length; i++)
        {
            _playerNameText[i].text = playerNames[i];
            _playerScoreText[i].text = ""+playerScores[i];
        }
    }
}

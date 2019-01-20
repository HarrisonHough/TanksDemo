using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour {

    [SerializeField]
    private Text[] playerNameText;
    [SerializeField]
    private Text[] playerScoreText;

	// Use this for initialization
	void Start () {
        ClearScoreBoard();

    }

    public void ClearScoreBoard()
    {
        for (int i = 0; i < playerNameText.Length; i++)
        {
            playerNameText[i].text =  "";
            playerScoreText[i].text = "";
        }
    }

    public void UpdateScores(string[] playerNames, int[] playerScores)
    {
        for (int i = 0; i < playerNames.Length; i++)
        {
            playerNameText[i].text = playerNames[i];
            playerScoreText[i].text = ""+playerScores[i];
        }
    }
}

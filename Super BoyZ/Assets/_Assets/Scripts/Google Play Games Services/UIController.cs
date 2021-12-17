using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public InputField scoreInputField;
    public Text errorText;

    private void Start()
    {
        errorText.text = "";
    }

    public void OnButtonPostToLeaderboard()
    {
        Debug.Log("Posting score to leaderboard");
        errorText.text = "";

        //If there is no value in the score input field
        if (string.IsNullOrEmpty(scoreInputField.text))
        {
            errorText.text = "Error: Could not post score to leaderboard. Please enter a value in the score input field.";
            return;
        }
        else
        {
            long scoreToPost;
            //Convert the value in the input field from string to long
            if(long.TryParse(scoreInputField.text, out scoreToPost))
            {
                PlayGamesController.PostToLeaderboard(scoreToPost);
            }
            else
            {
                errorText.text = "Error: Could not post score to leaderboard. Please enter a valid score value.";
            }
        }
    }

    public void OnButtonShowLeaderboard()
    {
        Debug.Log("Showing leaderboard");
        PlayGamesController.ShowLeaderboardUI();
    }
}

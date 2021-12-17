using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.Events;
using System.Collections;
using GooglePlayGames.BasicApi;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;
public class PlayGamesController : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI logInText;
    [SerializeField] private UnityEvent onLogIn;
    private bool isAuthenticated;
    private void Start(){
        TryLogIn();
    }
    public void TryLogIn(){
        LoadInitScens();
    // #if UNITY_EDITOR
    // #else
    //     // AuthenticateUser();
    // #endif
    }
    
    private void AuthenticateUser(){
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) => {
            if (success){
                Debug.Log("Logged in to Google Play Games Services");
                logInText.SetText("Logged in to Google Play Games Services");
                Invoke(nameof(OnLogin),1f);
                isAuthenticated = true;

            }
            else {
                Debug.LogError("Unable to sign in to Google Play Games Services");
                logInText.SetText("Unable to sign in to Google Play Games Services");
                Invoke(nameof(OnLogin),1f);
                isAuthenticated = false;
            }
        });
        
    }
    private void LoadInitScens(){
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void OnLogin(){
        onLogIn?.Invoke();
    }
    public static void UnlockAchivemnts(string acivementIds){
        if(PlayGamesPlatform.Instance.IsAuthenticated()){
            Social.ReportProgress(acivementIds,20000f, (bool success) =>{
                if(success){
                    Debug.Log("Unloack New Achivemnt");
                }else{
                    Debug.Log("Cannot Unloack new Acivements");
                }
            });
        }
    }

    public static void PostToLeaderboard(long newScore){
        if(PlayGamesPlatform.Instance.IsAuthenticated()){
            Social.ReportScore(newScore, GPGSIds.leaderboard_ranking, (success) => {
                if (success){
                    Debug.Log("Posted new score to leaderboard");
                }
                else{
                    Debug.LogError("Unable to post new score to leaderboard");
                }
            });
        }
    }

    public static void ShowLeaderboardUI(){
        if(PlayGamesPlatform.Instance.IsAuthenticated()){
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_ranking);
        }
    }
}

using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

namespace GamerWolf.Super_BoyZ {
    public class GameHandler : MonoBehaviour {
        
        
        [Header("Events")]
        [SerializeField] private UnityEvent OnGameStart;
        [SerializeField] private UnityEvent OnGameplaying,OnGameEnd,OnGamePause,OnGameResume;


        [Header("Testing")]
        [SerializeField] private bool isGamePlaying,isGameOver;
        [SerializeField] private bool canShowAd,isShowingAds;
        [SerializeField] private bool isPlayerDead;
        [SerializeField] private bool isRevived;
        private LevelManager levelManager;
        public static bool hasAdInGame = true;
        private AdController adController;

        #region Singelton..........
        public static GameHandler current;
        private void Awake(){
            if(current == null){
                current = this;
            }else{
                Destroy(current.gameObject);
            }

            levelManager = GetComponent<LevelManager>();
            adController = GetComponent<AdController>();
        }

        #endregion


        private void Start(){
            isGameOver = false;
            StartCoroutine(nameof(GamePlayStartRoutine));
        }
        private void Update(){

            // Debuing....
            // Need to remove.......at build..
            if(Input.GetKeyDown(KeyCode.Escape)){
                
                Application.Quit();
            }
        }

        #region Game Play Routine.....

        private IEnumerator GamePlayStartRoutine(){
            OnGameStart?.Invoke();
            while(!isGamePlaying){
                // to do.
                yield return null;
            }
            OnGameplaying?.Invoke();
            yield return StartCoroutine(GamePlayRoutine());
        }
        private IEnumerator GamePlayRoutine(){
            
            SetIsPlayerDead(false);
            while(!isGameOver){
                // To do..
                if(!isPlayerDead){
                    levelManager.UpdateKillInUI();
                    levelManager.CheckExtraEnemy();
                    UiHandler.current.ShowAdWindow(false);
                }else{
                    isRevived = false;
                    if(canShowAd){
                        yield return StartCoroutine(AskAdRoutine());
                    }else{
                        SetGameOver();
                    }
                    
                }
                yield return null;
            }
            Debug.Log("Game Over");
            levelManager.KillTimer();
            OnGameEnd?.Invoke();
        }
        private IEnumerator AskAdRoutine(){
            
            while(isPlayerDead && !isRevived){
                if(!isGameOver){
                    UiHandler.current.ShowAdWindow(true);
                }else{
                    
                    break;
                }
                yield return null;
            }
            Debug.Log("After Player is Un Dead or Game is Over");
            UiHandler.current.ShowAdWindow(false);
            if(isRevived){
                levelManager.Revied();
                SetIsPlayerDead(false);
                yield return StartCoroutine(GamePlayRoutine());
            }else{
                OnGameEnd?.Invoke();
            }
        }

        #endregion

        #region public Set Methods..
        public void PlayGame(){
            isGamePlaying = true;
        }
        public void Restart(){
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
        public void PauseGame(){
            Time.timeScale = 0f;
            OnGamePause?.Invoke();
        }
        public void ResumeGame(){
            Time.timeScale = 1f;
            OnGameResume?.Invoke();
        }
        public void SetIsPlayerDead(bool _isdead){
            isPlayerDead = _isdead;
        }
        public void SetGameOver(){
            StartCoroutine(GameOverRoutine(0.1f));
        }
        private IEnumerator GameOverRoutine(float delay){
            yield return new WaitForSeconds(delay);
            isGameOver = true;
        }
        
        #endregion



        #region public Get Methods........
        public bool GetIsGamePlaying(){
            return isGamePlaying;
        }


        #endregion

        #region Public Ads Set Methods.......
        public void SetCanRewardedShowAd(bool _value){
            canShowAd = _value;
        }
        public void SetIsAdsPlaying(bool _value){
            isShowingAds = _value;
        }
        public void SetIsRevived(bool _value){
            isRevived = _value;
            
        }
        public void ShowInterstetialAds(){
            if(hasAdInGame){
                Invoke(nameof(InvokeInterstetialAds),0.4f);
            }
        }
        private void InvokeInterstetialAds(){
            adController.ShowInterstitialAd();
        }
        #endregion
        
    }

}
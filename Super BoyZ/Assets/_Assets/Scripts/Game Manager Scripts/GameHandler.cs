using UnityEngine;
using GamerWolf.Utils;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;
namespace GamerWolf.Super_BoyZ {
    public class GameHandler : MonoBehaviour {
        
        
        [Header("Events")]
        [SerializeField] private UnityEvent OnGameStart;
        [SerializeField] private UnityEvent OnGameplaying,OnGameEnd,OnGamePause,OnGameResume;


        [Header("References")]
        [SerializeField] private PlayerDataSO playerData;

        [Header("Testing Exposed Variable")]
        [SerializeField] private bool isPlayerDead;
        [SerializeField] private bool isRevived;

        [SerializeField] private bool isGamePlaying,isGameOver;
        [SerializeField] private bool canShowAd,isShowingAds;
        private LevelManager levelManager;
        public static bool hasAdInGame = true;
        private AdController adController;
        public bool isGamePause;
        private int currentKillCount;
        
        

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
            
            StartCoroutine(nameof(GamePlayStartRoutine));
        }
        

        #region Game Play Routine.....
        private void InvokeStartGame(){
            PlayGame();
        }
        private IEnumerator GamePlayStartRoutine(){
            Invoke(nameof(InvokeStartGame),0.2f);
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
                    
                    levelManager.CheckExtraEnemy();
                    UiHandler.current.ShowAdWindow(false);
                    UiHandler.current.SetCoinCounts(currentKillCount);
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
            Time.timeScale = 1f;
            playerData.AddCoins(currentKillCount);
            LevelLoader.current.PlayLevel(SceneIndex.Game_Scene);
        }
        public void Play_PauseGame(){
            if(isGamePlaying){
                isGamePause = !isGamePause;
                if(isGamePause){
                    Time.timeScale = 0f;
                    OnGamePause?.Invoke();
                }else{
                    Time.timeScale = 1f;
                    OnGameResume?.Invoke();
                }
            }
            
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
        public void AddCoins(int _amount){
            currentKillCount += _amount;
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
            levelManager.SubscribeOnPlayerRevived();
            
        }
        public void ShowInterstetialAds(){
            if(hasAdInGame){
                Invoke(nameof(InvokeInterstetialAds),0.4f);
            }
        }
        private void InvokeInterstetialAds(){
            adController.ShowInterstitialAd();
        }
        public void MoveToMenu(){
            LevelLoader.current.PlayLevel(SceneIndex.Main_Menu);
        }
        
        #endregion
        
    }

}
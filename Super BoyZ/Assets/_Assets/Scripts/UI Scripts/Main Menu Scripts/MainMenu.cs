using UnityEngine;
using GamerWolf.Utils;
using UnityEngine.Events;

namespace GamerWolf.Super_BoyZ {
    public class MainMenu : MonoBehaviour {

        [SerializeField] private UnityEvent onGameStart;


        private void Start(){
            onGameStart?.Invoke();
        }

        public void PlayGame(){
            LevelLoader.current.PlayLevel(SceneIndex.Game_Scene);
        }
        public void ShowLeaderboard(){
            PlayGamesController.ShowLeaderboardUI();
        }
        
    }

}
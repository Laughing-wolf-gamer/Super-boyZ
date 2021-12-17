using UnityEngine;


namespace GamerWolf.Super_BoyZ {
    [CreateAssetMenu(fileName = "New Player Item",menuName = "ScriptableObject/Player Data")]
    public class PlayerDataSO : ScriptableObject {
        
        public int coinAmount;
        public ShopItemSO curentSkin;


        public void AddCoins(int _amount){
            coinAmount += _amount;
            PostScoreToLeaderBoard();
        }
        public void PostScoreToLeaderBoard(){
            PlayGamesController.PostToLeaderboard(coinAmount);
        }
        [ContextMenu("Save")]
        public void SaveCoinData(){
            PlayerPrefs.SetInt("Coin",coinAmount);
        }
        [ContextMenu("Load")]
        public void LoadCoinData(){
            coinAmount = PlayerPrefs.GetInt("Coin");
            
        }
        [ContextMenu("Clear")]
        public void ResetCoinAmount(){
            coinAmount = 0;
            SaveCoinData();
        }
        
        public void RemoveCoins(int _value){
            coinAmount -= _value;
            if(coinAmount <= 0){
                coinAmount = 0;
            }
        }
    }

}
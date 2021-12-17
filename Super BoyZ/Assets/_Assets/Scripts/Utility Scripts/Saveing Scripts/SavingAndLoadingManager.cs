using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace GamerWolf.Super_BoyZ{

    public class SavingAndLoadingManager : MonoBehaviour{
        public static SavingAndLoadingManager instance {get;private set;}
        
        [SerializeField] private SaveData saveData;
        
        
        private void Awake(){
            if(instance == null){
                instance = this;
            }else{
                Destroy(instance);
            }
            DontDestroyOnLoad(instance);
            
            LoadGame();
            

        }
        [ContextMenu("SAVE GAME")]
        public void SaveGame(){
            // saveData.settingsData.Save();
            saveData.playerData.SaveCoinData();
            for (int i = 0; i < saveData.shopItemData.Length; i++){
                saveData.shopItemData[i].Save();
            }
            // saveData.coins.Save();
        }
        [ContextMenu("LOAD GAME")]
        public void LoadGame(){
            // saveData.settingsData.Load();
            for (int i = 0; i < saveData.shopItemData.Length; i++){
                saveData.shopItemData[i].Load();
            }
            saveData.playerData.LoadCoinData();
            // saveData.coins.Load();
        }      
        public void ResetGame(){
            for (int i = 0; i < saveData.shopItemData.Length; i++){
                saveData.shopItemData[i].Reset();
            }
        }
        private void OnApplicationQuit(){
            SaveGame();
            
        }

    }
    [System.Serializable]
    public struct SaveData{
        // public SettingsSO settingsData;
        // public LevelDataSO[] levelDataSOArray;
        public PlayerDataSO playerData;
        public ShopItemSO[] shopItemData;
        // public CoinDataSO coins;
    }

}
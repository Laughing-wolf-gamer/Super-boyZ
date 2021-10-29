using System;
using UnityEngine;
using GamerWolf.Utils;
using System.Collections.Generic;
namespace GamerWolf.Super_BoyZ {
    public enum SpawnLevels{
        Level_1,
        Level_2,
        Level_3,
        Level_4,
        Level_5,
        

    }
    public class LevelManager : MonoBehaviour {
        
        [Header("Level Progressin")]
        [SerializeField] private int killForLevel2 = 5;
        [SerializeField] private int killForLevel3 = 15,killForLevel4 = 25,killForLevel5 = 40;
        

        
        [Header("Player")]
        [SerializeField] private Player player;

        [Header("Boss Platform")]
        [SerializeField] private Platform bossSpawnPoint;

        [Header("Minions Platform")]
        
        [SerializeField] private Platform[] minionTopPlatformspawnPointArray;
        [SerializeField] private Platform[] minionBottomPlatformspawnPointArray;
        

        [Header("Enemy Killed")]
        [SerializeField] private int minionEnemyKilledCount;
        [SerializeField] private int bossKillCount;


        
        private float spawnTime;
        private SpawnLevels spawnLevels;
        private GameHandler gameHandler;
        private string enemyTimerName = "Top Minions";

        [SerializeField] private List<EnemyBase> minionEnemyList;
        [SerializeField] private List<EnemyBase> bossList;
        #region Singelton....
        public static LevelManager current;
        private void Awake(){
            if(current == null){
                current = this;
            }else{
                Destroy(current.gameObject);
            }
            
            gameHandler = GetComponent<GameHandler>();
            
        }
        #endregion
        
        private void Start(){
            minionEnemyList = new List<EnemyBase>();
            bossList = new List<EnemyBase>();
            
            player.onDead += SetPlayerDead;
        }
        
       
        public void StartTimer(){
            spawnTime = 2f;
            TimerTickSystem.CreateTimer(SpawnEnemies,spawnTime,enemyTimerName);
        }
        
        #region Spawning Level Progressen....
        
        private void SpawnEnemies(){
            if(minionEnemyList.Count <= 0){
                switch (spawnLevels){
                    
                    case SpawnLevels.Level_1:
                        Debug.Log("On Level 1 Difficulty");
                        SetTimer(2f);
                        SpawnLevel_1_Defficulty();
                    break;
                    case SpawnLevels.Level_2:
                        Debug.Log("On Level 2 Difficulty");
                        
                        SpawnLevel_2_Defficultiy();
                        SetTimer(1.9f);

                    break;
                    case SpawnLevels.Level_3:
                        Debug.Log("On Level 3 Difficulty");
                        
                        SpawnLevel_3_Defficultiy();
                        SetTimer(1.7f);
                    break;
                    case SpawnLevels.Level_4:
                        Debug.Log("On Level 4 Difficulty");
                        
                        SpawnLevel_4_Defficultiy();
                        SetTimer(1.5f);
                    break;
                    case SpawnLevels.Level_5:
                        Debug.Log("On Level 5 Difficulty");
                        SpawnLevel_5_Defficultiy();
                        SetTimer(1f);
                    break;
                    
                    
                }
                
            }
            
        }
        private void SpawnLevel_1_Defficulty(){
            if(minionEnemyKilledCount > killForLevel2){
                spawnLevels = SpawnLevels.Level_2;
                SpawnEnemies();
                return;
            }
            int rand = UnityEngine.Random.Range(0,2);
            SpawnBottomPlatformMinionEnemys();
        }
        private void SpawnLevel_2_Defficultiy(){
            
            if(minionEnemyKilledCount > killForLevel3){
                spawnLevels = SpawnLevels.Level_3;
                SpawnEnemies();
                return;
            }
            int rand = UnityEngine.Random.Range(0,7);
            int bossRandSpawnNumber = UnityEngine.Random.Range(0,5);
            if(rand >= 3){
                SpawnTopPlatformMinionEnemys();
            }else{
                SpawnBottomPlatformMinionEnemys();
                if(bossRandSpawnNumber >= 2){
                    SpawnBossEnemys();
                }
            }
            
        }
        private void SpawnLevel_3_Defficultiy(){
            if(minionEnemyKilledCount > killForLevel4){
                spawnLevels = SpawnLevels.Level_4;
                SpawnEnemies();
                return;
            }
            int rand = UnityEngine.Random.Range(0,7);
            int bossRandSpawnNumber = UnityEngine.Random.Range(0,10);
            if(rand <= 2){

                SpawnTopPlatformMinionEnemys();
            }
            if(rand > 2){
                SpawnBottomPlatformMinionEnemys();
                if(bossRandSpawnNumber >= 4){
                    SpawnBossEnemys();
                }
                
            }
            
        }
        private bool hasBoss(){
            if(bossList.Count > 0){
                return true;
            }
            return false;

        }
        private void SpawnLevel_4_Defficultiy(){
            if(minionEnemyKilledCount > killForLevel5){
                spawnLevels = SpawnLevels.Level_5;
                SpawnEnemies();
                return;
            }
            int rand = UnityEngine.Random.Range(0,7);
            int bossRandSpawnNumber = UnityEngine.Random.Range(0,10);
            if(rand > 2){
                SpawnBottomPlatformMinionEnemys();
                if(bossRandSpawnNumber >= 2){
                    SpawnBossEnemys();
                }
            }else{
                SpawnTopPlatformMinionEnemys();
            }
            
        }
        private void SpawnLevel_5_Defficultiy(){
            int rand = UnityEngine.Random.Range(0,7);
            int bossRandSpawnNumber = UnityEngine.Random.Range(0,10);
            if(rand > 5){
                SpawnTopPlatformMinionEnemys();
            }else{
                SpawnBottomPlatformMinionEnemys();
                if(bossRandSpawnNumber >= 1){
                    SpawnBossEnemys();
                }
            }
            
        }
        private void SetTimer(float newTime){
            if(spawnTime >= 0.01f){
                spawnTime = newTime;

            }
        }
        public void AddMinionEnemy(EnemyBase enemy){
            if(!minionEnemyList.Contains(enemy)){
                minionEnemyList.Add(enemy);
            }
        }
        public void AddBossEnemy(EnemyBase enemy){
            if(!bossList.Contains(enemy)){
                bossList.Add(enemy);
            }
        }
        public void RemoveMinonEnemy(EnemyBase enemy){
            if(minionEnemyList.Contains(enemy)){
                minionEnemyList.Remove(enemy);
            }
        }
        public void RemoveBossEnemy(EnemyBase enemy){
            if(bossList.Contains(enemy)){
                bossList.Remove(enemy);
            }
        }
        
        #endregion

        // private bool hasEnemonBottomPlatforms(){
        //     foreach(Platform platform in minionBottomPlatformspawnPointArray){
        //         if(platform.hasEnemyOnPlatform()){
        //             return true;
        //         }
        //     }
        //     return false;
        // }
        
        
        
        
        #region Bottom Enemy Spawing Conditions....

        private void SpawnBottomPlatformMinionEnemys(){

            int enemyspawnNumber = UnityEngine.Random.Range(0,5);
            
            for (int i = 0; i < minionBottomPlatformspawnPointArray.Length; i++){
                if(minionBottomPlatformspawnPointArray[i].hasEnemyOnPlatform()){
                    break;
                }else{
                    if(enemyspawnNumber >= 1){
                        SpawnSingelEnemey();

                    }else{
                        SpawnBothSideEnemy();
                    }

                }
                
            }
            
        }
        private void SpawnSingelEnemey(){
            int rand = UnityEngine.Random.Range(0,minionBottomPlatformspawnPointArray.Length);
            minionBottomPlatformspawnPointArray[rand].SpawnMinionEnemy(player.GetBottomtargetPoint(),EnemyPositions.Bottom);
            
        }
        private void SpawnBothSideEnemy(){
            foreach(Platform pl in minionBottomPlatformspawnPointArray){
                pl.SpawnMinionEnemy(player.GetBottomtargetPoint(),EnemyPositions.Bottom);
            }
        }


        #endregion

        #region Top Platform Enemy Spawning Conditions....
        private void SpawnTopPlatformMinionEnemys(){
            if(!hasBoss()){
                foreach(Platform platforms in minionTopPlatformspawnPointArray){
                    platforms.SpawnMinionEnemy(player.GetToptargetPoint(),EnemyPositions.Top);
                    
                }
            }
        }
        
        #endregion

        
        #region Boss Enemy Spawing.........

        private void SpawnBossEnemys(){
            bossSpawnPoint.SpawnBossEnemy(player.GetToptargetPoint(),EnemyPositions.Top);
        }

        #endregion


        #region Check if Any Extra Enemy is Spawing In the Same Position....
        public void CheckExtraEnemy(){
            for (int i = 0; i < minionTopPlatformspawnPointArray.Length; i++){
                minionTopPlatformspawnPointArray[i].CheckForExtraEnemy();
            }
            for (int i = 0; i < minionBottomPlatformspawnPointArray.Length; i++){
                minionBottomPlatformspawnPointArray[i].CheckForExtraEnemy();
            }
        }
        
        #endregion
        
        #region Set Player Dead and Ask for Ads.....
        public void SetPlayerDead(object sender,EventArgs e){

            Debug.Log("Player Once Dead");
            for (int i = 0; i < minionEnemyList.Count; i++){
                minionEnemyList[i].SetCanShoot(false);
                minionEnemyList[i].SetCanShoot(false);
            }
            player.SetInputsEnableDesable(false);
            gameHandler.SetIsPlayerDead(true);
        }
        public void Revied(){
            player.SetInputsEnableDesable(true);
            player.RestHealth();
            player.onDead += SetPlayerDead;
            for (int i = 0; i < minionEnemyList.Count; i++){
                minionEnemyList[i].SetCanShoot(true);
            }
            
        }

        #endregion

        #region Final Player Dead....
        public void KillTimer(){
            player.SetInputsEnableDesable(false);
            TimerTickSystem.StopTimer(enemyTimerName);
            for (int i = 0; i < minionEnemyList.Count; i++){
                minionEnemyList[i].SetCanShoot(false);
            }
        }


        #endregion

        #region Kills Count and UI...
        public void IncreaseMinionEnemyKilledCount(){
            minionEnemyKilledCount += 5;

        }
        public void IncreaseBossKillCount(){
            bossKillCount += 25;
        }
        public void UpdateKillInUI(){
            int killCount = minionEnemyKilledCount + bossKillCount;
            
            UiHandler.current.SetKillCounts(killCount);
        }

        #endregion
        
        
        
        
        
        
        
    }

}
using System;
using UnityEngine;
using GamerWolf.Utils;
using System.Collections;
using System.Collections.Generic;
namespace GamerWolf.Super_BoyZ {
    public enum SpawnLevels{
        Level_1,
        Level_2,
        Level_3,
        Level_4,
    }
    public class LevelManager : MonoBehaviour {
        
        [Header("Level Progressin")]
        [SerializeField] private int killForLevel2 = 7;
        [SerializeField] private int killForLevel3 = 20,killForLevel4 = 40;
        
        [Header("Player")]
        [SerializeField] private Player player;

        [Header("Boss Platform")]
        [SerializeField] private float timeToSpawnBoss = 2f;
        [SerializeField] private Platform bossSpawnPoint;

        [Header("Minions Platform")]
        [SerializeField] private float timeToSpawnMinions = 2f;
        [SerializeField] private Platform[] minionTopPlatformspawnPointArray;
        [SerializeField] private Platform[] minionBottomPlatformspawnPointArray;
        

        [Header("Enemy Killed")]
        [SerializeField] private int minionEnemyKilledCount;
        [SerializeField] private int bossKillCount;

        private string topMinionSpawnTimerName = "Top Minions",bottomMinionSpawnTimerName = "Bottom Minions";

        private SpawnLevels spawnLevels;
        private int rand;
        private float randomTimer;
        private GameHandler gameHandler;

        [SerializeField] private List<EnemyBase> enemyList;
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
            enemyList = new List<EnemyBase>();
            
            player.onDead += SetPlayerDead;

            
            
        }
        
       
        public void StartTimer(){
            randomTimer = 1.5f;
            TimerTickSystem.CreateTimer(spawnMinonsEnemies,randomTimer,topMinionSpawnTimerName);
        }
        
        
        private void spawnMinonsEnemies(){
            if(enemyList.Count <= 0){
                switch (spawnLevels){
                    
                    case SpawnLevels.Level_1:
                        Debug.Log("On Level 1 Difficulty");
                        if(minionEnemyKilledCount > killForLevel2 && minionEnemyKilledCount <= killForLevel3){
                            spawnLevels = SpawnLevels.Level_2;
                        }
                        SetTimer(1.5f);
                        SpawnLevel_1_Defficulty();

                    break;
                    case SpawnLevels.Level_2:
                        Debug.Log("On Level 2 Difficulty");
                        if(minionEnemyKilledCount > killForLevel3 && minionEnemyKilledCount <= killForLevel4){
                            spawnLevels = SpawnLevels.Level_3;
                        }
                        SpawnLevel_2_Defficultiy();
                        SetTimer(1f);

                    break;
                    case SpawnLevels.Level_3:
                        Debug.Log("On Level 3 Difficulty");
                        if(minionEnemyKilledCount > killForLevel4){
                            spawnLevels = SpawnLevels.Level_4;
                        }
                        SpawnLevel_3_Defficultiy();
                        SetTimer(0.7f);
                    break;
                    case SpawnLevels.Level_4:
                        Debug.Log("On Level 4 Difficulty");
                        SpawnLevel_3_Defficultiy();
                        SetTimer(0.5f);
                    break;
                }
                
            }
            
        }
        private void SetTimer(float newTime){
            if(randomTimer >= 0.01f){
                randomTimer = newTime;

            }
        }
        private bool hasEnemonBottomPlatforms(){
            foreach(Platform platform in minionBottomPlatformspawnPointArray){
                if(platform.hasEnemyOnPlatform()){
                    return true;
                }
            }
            return false;
        }
        private void SpawnLevel_1_Defficulty(){
            // SpawnBossEnemy();
            int rand = UnityEngine.Random.Range(0,2);
            SpawnBottomPlatformMinionEnemys();
        }
        private void SpawnLevel_2_Defficultiy(){
            SpawnBottomPlatformMinionEnemys();
            int rand = UnityEngine.Random.Range(0,6);
            if(rand >= 2){
                SpawnTopPlatformMinionEnemys();
            }
        }
        private void SpawnLevel_3_Defficultiy(){
            int rand = UnityEngine.Random.Range(0,7);
            if(rand < 2){
                SpawnBottomPlatformMinionEnemys();
            }
            if(rand >= 2 && rand <= 4){
                SpawnTopPlatformMinionEnemys();
                SpawnBottomPlatformMinionEnemys();
            }
            if(rand >= 4){
                SpawnTopPlatformMinionEnemys();
                SpawnBottomPlatformMinionEnemys();
                SpawnBossEnemy();
            }
        }
        
        

        private void SpawnBottomPlatformMinionEnemys(){

            int rand = UnityEngine.Random.Range(0,5);
            
            for (int i = 0; i < minionBottomPlatformspawnPointArray.Length; i++){
                if(minionBottomPlatformspawnPointArray[i].hasEnemyOnPlatform()){
                    break;
                }else{
                    if(rand >= 1){
                        SpawnSingelEnemey();

                    }else{
                        SpawnBothSideEnemy();
                    }

                }
                
            }
            
        }
        private void SpawnSingelEnemey(){
            int rand = UnityEngine.Random.Range(0,minionBottomPlatformspawnPointArray.Length);
            minionBottomPlatformspawnPointArray[rand].SpawnMinionEnemy(player.GetBottomtargetPoint(),EnemyPos.Bottom);
            
        }
        private void SpawnBothSideEnemy(){
            foreach(Platform pl in minionBottomPlatformspawnPointArray){
                pl.SpawnMinionEnemy(player.GetBottomtargetPoint(),EnemyPos.Bottom);
            }
        }
        private void SpawnTopPlatformMinionEnemys(){
            foreach(Platform platforms in minionTopPlatformspawnPointArray){
                platforms.SpawnMinionEnemy(player.GetToptargetPoint(),EnemyPos.Top);
                
            }
            
            
        }
        public void CheckExtraEnemy(){
            for (int i = 0; i < minionTopPlatformspawnPointArray.Length; i++){
                minionTopPlatformspawnPointArray[i].CheckForExtraEnemy();
            }
            for (int i = 0; i < minionBottomPlatformspawnPointArray.Length; i++){
                minionBottomPlatformspawnPointArray[i].CheckForExtraEnemy();
            }
        }
        private void SpawnBossEnemy(){
            bossSpawnPoint.SpawnBossEnemy(player.GetToptargetPoint(),EnemyPos.Top);
        }

        
        
        public void SetPlayerDead(object sender,EventArgs e){

            Debug.Log("Player Once Dead");
            for (int i = 0; i < enemyList.Count; i++){
                enemyList[i].SetCanShoot(false);
                enemyList[i].SetCanShoot(false);
            }
            player.SetInputsEnableDesable(false);
            gameHandler.SetIsPlayerDead(true);
        }
        public void KillTimer(){
            player.SetInputsEnableDesable(false);
            TimerTickSystem.StopTimer(topMinionSpawnTimerName);
            TimerTickSystem.StopTimer(bottomMinionSpawnTimerName);
            for (int i = 0; i < enemyList.Count; i++){
                enemyList[i].SetCanShoot(false);
            }
        }
        public void IncreaseMinionEnemyKilledCount(){
            minionEnemyKilledCount++;

        }
        public void UpdateKillInUI(){
            int killCount = minionEnemyKilledCount;
            UiHandler.current.SetKillCounts(killCount);
        }
        public void AddEnemy(EnemyBase enemy){
            if(!enemyList.Contains(enemy)){
                enemyList.Add(enemy);
            }
        }
        public void RemoveEnemy(EnemyBase enemy){
            if(enemyList.Contains(enemy)){
                enemyList.Remove(enemy);
            }
        }
        
        public void Revied(){
            player.SetInputsEnableDesable(true);
            player.RestHealth();
            player.onDead += SetPlayerDead;
            for (int i = 0; i < enemyList.Count; i++){
                enemyList[i].SetCanShoot(true);
            }
            
        }
        
        
    }

}
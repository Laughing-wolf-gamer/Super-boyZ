using UnityEngine;
using GamerWolf.Utils;
using System.Collections.Generic;

namespace GamerWolf.Super_BoyZ {
    public enum PlatformType{
        Boss_Platform,
        minion_Platform,
    }
    public class Platform : MonoBehaviour {
        
        
        [SerializeField] private float checkRadius = 0.3f;
        [SerializeField] private Vector2 offset;
        [SerializeField] private LayerMask whatIsEnemy;
        [SerializeField] private Transform jumpPoint;

        private ObjectPoolingManager objectPooling;
        private LevelManager levelManager;
        private void Start(){
            objectPooling = ObjectPoolingManager.i;
            levelManager = LevelManager.current;
        }
        public void CheckForExtraEnemy(){
            if(hasEnemyOnPlatform()){
                foreach(EnemyBase enemys in platformEnemy()){
                    enemys.DestroyMySelf();
                    levelManager.RemoveMinonEnemy(enemys);
                    levelManager.RemoveBossEnemy(enemys);
                }
            }
            
            
        }

        
        public bool hasEnemyOnPlatform(){
            Vector2 checkPoint = (Vector2)transform.position + offset;
            RaycastHit2D hit2D = Physics2D.CircleCast(checkPoint,checkRadius,Vector2.up,0.1f,whatIsEnemy);
            if(hit2D.collider != null){
                return true;
            }
            return false;
        }
        private List<EnemyBase> platformEnemy(){
            List<EnemyBase> enemyList = new List<EnemyBase>();
            Vector2 checkPoint = (Vector2)transform.position + offset;
            RaycastHit2D[] hit2D = Physics2D.CircleCastAll(checkPoint,checkRadius,Vector2.up,0.1f,whatIsEnemy);
            for (int i = 1; i < hit2D.Length; i++){
                EnemyBase enemy = hit2D[i].collider.GetComponent<EnemyBase>();
                if(enemy != null){
                    if(!enemyList.Contains(enemy)){
                        enemyList.Add(enemy);
                    }
                }
            }
            
            
            return enemyList;
        }

        
        public void SpawnMinionOnTopPlatform(Transform _viewTarget,EnemyPositions pos){
            
            if(!hasEnemyOnPlatform()){
                GameObject enemy = objectPooling.SpawnFromPool(PoolObjectTag.Top_Minion_Enemy,jumpPoint.position,transform.rotation);
                EnemyBase minion = enemy.GetComponent<EnemyBase>();
                minion.SetJumpDir(jumpPoint.right);
                minion.SetEnemyPosition(pos);
                
                if(minion != null){
                    minion.SetTarget(_viewTarget);
                }
                levelManager.AddMinionEnemy(minion);
            }else{
                for (int i = 0; i < platformEnemy().Count; i++){
                    foreach(EnemyBase enemys in platformEnemy()){
                        levelManager.RemoveMinonEnemy(enemys);
                        enemys.DestroyMySelf();
                    }
                }
            }
        }
        public void SpawnMinionOnBottomPlatform(Transform _viewTarget,EnemyPositions pos){
            
            if(!hasEnemyOnPlatform()){
                
                GameObject enemy = objectPooling.SpawnFromPool(PoolObjectTag.Bottom_Minion_Enemy,jumpPoint.position,transform.rotation);
                EnemyBase minion = enemy.GetComponent<EnemyBase>();
                minion.SetEnemyPosition(pos);
                minion.SetJumpDir(jumpPoint.right);
                if(minion != null){
                    minion.SetTarget(_viewTarget);
                }
                levelManager.AddMinionEnemy(minion);
            }else{
                for (int i = 0; i < platformEnemy().Count; i++){
                    foreach(EnemyBase enemys in platformEnemy()){
                        enemys.DestroyMySelf();
                        levelManager.RemoveMinonEnemy(enemys);
                    }
                }
            }
        }
        
        public void SpawnBossEnemy(Transform _viewTarget,EnemyPositions pos){
            if(!hasEnemyOnPlatform()){
                
                Vector2 spanwPoint = (Vector2) transform.position + offset;
                GameObject enemy = objectPooling.SpawnFromPool(PoolObjectTag.Boss_Enemy,spanwPoint,transform.rotation);
                EnemyBase boss = enemy.GetComponent<EnemyBase>();
                boss.SetEnemyPosition(pos);
                if(boss != null){
                    boss.SetTarget(_viewTarget);
                }
                levelManager.AddBossEnemy(boss);
            }else{
                for (int i = 0; i < platformEnemy().Count; i++){
                    foreach(EnemyBase enemys in platformEnemy()){
                        enemys.DestroyMySelf();
                        levelManager.RemoveBossEnemy(enemys);
                    }
                }
            }
        }

        private void OnDrawGizmosSelected(){
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere((Vector2)transform.position + offset,checkRadius);
        }
        
    }

}
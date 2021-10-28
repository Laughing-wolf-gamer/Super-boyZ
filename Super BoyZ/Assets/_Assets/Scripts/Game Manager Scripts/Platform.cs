using UnityEngine;
using GamerWolf.Utils;
using System.Collections;
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

        private ObjectPoolingManager objectPooling;
        private void Start(){
            
            objectPooling = ObjectPoolingManager.i;
            
        }
        public void CheckForExtraEnemy(){
            
            if(hasEnemyOnPlatform()){
                foreach(EnemyBase enemys in platformEnemy()){
                    enemys.DestroyMySelf();
                    LevelManager.current.RemoveEnemy(enemys);
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

        
        public void SpawnMinionEnemy(Transform _viewTarget,EnemyPos pos){
            
            if(!hasEnemyOnPlatform()){
                Vector2 spanwPoint = (Vector2) transform.position + offset;
                GameObject enemy = objectPooling.SpawnFromPool(PoolObjectTag.Minion_Enemy,spanwPoint,transform.rotation);
                EnemyBase minion = enemy.GetComponent<EnemyBase>();
                minion.SetEnemyPosition(pos);
                if(minion != null){
                    minion.SetTarget(_viewTarget);
                }
                
            }else{
                for (int i = 0; i < platformEnemy().Count; i++){
                    foreach(EnemyBase enemys in platformEnemy()){
                        enemys.DestroyMySelf();
                    }
                }
            }
        }
        
        public void SpawnBossEnemy(Transform _viewTarget,EnemyPos pos){
            if(!hasEnemyOnPlatform()){
                Vector2 spanwPoint = (Vector2) transform.position + offset;
                GameObject enemy = objectPooling.SpawnFromPool(PoolObjectTag.Boss_Enemy,spanwPoint,transform.rotation);
                EnemyBase boss = enemy.GetComponent<EnemyBase>();
                boss.SetEnemyPosition(pos);
                if(boss != null){
                    boss.SetTarget(_viewTarget);
                }
            }else{
                for (int i = 0; i < platformEnemy().Count; i++){
                    foreach(EnemyBase enemys in platformEnemy()){
                        enemys.DestroyMySelf();
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
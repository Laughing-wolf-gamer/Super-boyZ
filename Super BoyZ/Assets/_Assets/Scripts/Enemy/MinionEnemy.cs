using System;
using UnityEngine;

namespace GamerWolf.Super_BoyZ {
    public class MinionEnemy : EnemyBase {


        
        protected override void Start(){
            base.Start();
            onDead += OnMinionDead;
            base.onFire = OnMinonFire;
            base.OnHit += OnMinoHit;
        }

        private void OnMinoHit(object sender, EventArgs e){
            
        }

        private void OnMinonFire(){
            Debug.Log("Minon fired from " + base.enemyPos);
            // Shoot();
        }
        

        private void OnMinionDead(object sender,EventArgs e){
            Debug.Log("Minion Killed");
            LevelManager.current.IncreaseMinionEnemyKilledCount();
            // DestroyMySelf();
        }
        
    }

}
using System;
using UnityEngine;

namespace GamerWolf.Super_BoyZ {
    public class MinionEnemy : EnemyBase {


        
        protected override void Start(){
            base.Start();
            onDead += OnMinionDead;
        }
        

        private void OnMinionDead(object sender,EventArgs e){
            Debug.Log("Minion Killed");
            LevelManager.current.IncreaseMinionEnemyKilledCount();
            // DestroyMySelf();
        }
        
    }

}
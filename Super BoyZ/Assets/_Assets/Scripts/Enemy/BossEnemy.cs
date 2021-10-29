using System;
using UnityEngine;

namespace GamerWolf.Super_BoyZ {
    public class BossEnemy : EnemyBase {
        protected override void Start(){
            base.Start();
            base.onDead += OnBossDead;
            base.OnHit += OnProjectileHit;
        }
        private void OnProjectileHit(object sender,EventArgs e){

        }

        private void OnBossDead(object sender,EventArgs e){
            Debug.Log("Boss Dead");
            LevelManager.current.IncreaseBossKillCount();
            GameHandler.current.ShowInterstetialAds();
        }
        
        
    }

}
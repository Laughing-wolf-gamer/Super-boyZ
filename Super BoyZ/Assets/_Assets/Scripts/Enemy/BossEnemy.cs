using System;
using UnityEngine;

namespace GamerWolf.Super_BoyZ {
    public class BossEnemy : EnemyBase {
        protected override void Start(){
            base.Start();
            base.onDead += OnBossDead;
        }

        private void OnBossDead(object sender,EventArgs e){
            Debug.Log("Boss Dead");
            GameHandler.current.ShowInterstetialAds();
        }

        
    }

}
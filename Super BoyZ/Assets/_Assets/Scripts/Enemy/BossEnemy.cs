using System;
using UnityEngine;
using GamerWolf.Utils;
namespace GamerWolf.Super_BoyZ {
    public class BossEnemy : EnemyBase {
        [SerializeField] private bool isFierd;
        private ScreenShakeManager screenShakeManager;
        protected override void Start(){
            base.Start();
            base.onDead += OnBossDead;
            base.OnHit += OnProjectileHit;
            base.onFire = OnBossFire;
            screenShakeManager = ScreenShakeManager.current;
        }

        private void OnBossFire(){
            Debug.Log("Boss Fired ");
            
        }

        private void CancleFire(){
            isFierd = false;
        }
        private void OnProjectileHit(object sender,EventArgs e){
            
        }

        private void OnBossDead(object sender,EventArgs e){
            Debug.Log("Boss Dead");
            screenShakeManager.StartShake();
            LevelManager.current.IncreaseBossKillCount();
            GameHandler.current.ShowInterstetialAds();
        }
        public bool GetIsFired(){
            return isFierd;
        }
        
    }

}
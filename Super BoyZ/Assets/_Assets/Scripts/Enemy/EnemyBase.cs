using System;
using UnityEngine;
using GamerWolf.Utils;
using System.Collections.Generic;
public enum EnemyPositions{
    Top,Bottom
}
namespace GamerWolf.Super_BoyZ {
    public class EnemyBase : HealthEntity,IPooledObject{
        
        [SerializeField] private float bulletSpeedBottomEnemy = 100f,bulletSpeedTopEnemy = 150f;
        
        [SerializeField] private float topEnemyFireRate = 1f,bottomEnmyFireRate = 0.5f;

        [SerializeField] private EnemyWepon wepon;
        [SerializeField] protected Transform weponPivot;
        // [SerializeField] private HealthCounterPopUp healthCounterPopUp;
        private float maxTimeToFire;
        private float fireRate;
        private Transform target;
        private float bulletSpeed;
        public bool canShoot;
        protected List<Projectile> currentShootingProjectileList;
        private EnemyPositions enemyPos;
        private LevelManager levelManager;
        
        
        protected override void Start(){
            base.Start();
            fireRate = 0f;
            SetCanShoot(true);
            currentShootingProjectileList = new List<Projectile>();
            bulletSpeed = bulletSpeedBottomEnemy;
            levelManager = LevelManager.current;
            base.OnHit += OnProjecitleHit;
        }
        
        protected virtual void Update(){
            if(canShoot){
                if(!isDead){
                    switch (enemyPos){
                        
                        case EnemyPositions.Top:
                            SetWeponRotation();
                        break;

                        case EnemyPositions.Bottom:
                            weponPivot.localRotation = Quaternion.Euler(Vector3.zero);
                        break;
                    }
                    SetWeponRotation();
                    
                    if(fireRate >= maxTimeToFire){
                        Shoot();
                        fireRate = 0f;
                        
                    }else{
                        fireRate += Time.deltaTime;
                    }
                }else{
                    DestroyMySelf();
                }
            }
            
        }
        private void OnProjecitleHit(object sender,EventArgs e){
            // healthCounterPopUp.SetText(currentHealth.ToString());
        }
        private void SetWeponRotation(){
            Vector3 dir = (target.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
            weponPivot.rotation = Quaternion.AngleAxis(angle,Vector3.forward);
        }
        private void DestroyEveryBulletCamFrom(){
            foreach(Projectile bullet in currentShootingProjectileList){
                bullet.DestroyMySelf();
            }
            currentShootingProjectileList = new List<Projectile>();
        }

        #region public Setters Methods......
        
        public void SetTarget(Transform _target){
            target = _target;
        }
        public void AddToCurrentBulletList(Projectile projectile){
            if(!currentShootingProjectileList.Contains(projectile)){
                currentShootingProjectileList.Add(projectile);
            }
        }
        public void RemoveCurrentBulletList(Projectile projectile){
            if(currentShootingProjectileList.Count > 0){
                if(currentShootingProjectileList.Contains(projectile)){
                    currentShootingProjectileList.Remove(projectile);
                }
            }
        }
        public void SetCanShoot(bool _value){
            canShoot = _value;
            
        }
        public void SetFireRate(float fireRate){
            this.maxTimeToFire = fireRate;
        }
        public void SetEnemyPosition(EnemyPositions _enemyPos){

            this.enemyPos = _enemyPos;
            switch (this.enemyPos){
                case EnemyPositions.Top:
                    bulletSpeed = bulletSpeedTopEnemy;
                    SetFireRate(topEnemyFireRate);
                break;

                case EnemyPositions.Bottom:
                    bulletSpeed = bulletSpeedBottomEnemy;
                    SetFireRate(bottomEnmyFireRate);
                break;
            }
            
        }


        #endregion

        #region Parent - Child Methods......
        
        protected void Shoot(){
            wepon.ShootBullet(bulletSpeedBottomEnemy);
            
        }

        public void OnObjectReuse(){
            SetCanShoot(true);
            fireRate = 0f;
            RestHealth();
        }
        

        public void DestroyMySelf(){
            canShoot = false;
            gameObject.SetActive(false);
            weponPivot.localRotation = Quaternion.Euler(Vector3.zero);
            DestroyEveryBulletCamFrom();
            levelManager.RemoveMinonEnemy(this);
            levelManager.RemoveBossEnemy(this);
        }
        #endregion
        
    }

}
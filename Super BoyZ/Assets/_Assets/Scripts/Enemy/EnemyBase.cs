using UnityEngine;
using GamerWolf.Utils;
using System.Collections.Generic;
public enum EnemyPos{
    Top,Bottom
}
namespace GamerWolf.Super_BoyZ {
    public class EnemyBase : HealthEntity,IPooledObject{
        
        [SerializeField] private float bulletSpeedBottomEnemy = 100f,bulletSpeedTopEnemy = 150f;
        
        [SerializeField] private float topEnemyFireRate = 1f,bottomEnmyFireRate = 0.5f;

        [SerializeField] private EnemyWepon wepon;
        [SerializeField] protected Transform weponPivot;
        private float maxTimeToFire;
        private float fireRate;
        private Transform target;
        private float bulletSpeed;
        public bool canShoot;
        protected List<Projectile> currentShootingProjectileList;
        private EnemyPos enemyPos;
        
        protected override void Start(){
            base.Start();
            fireRate = 0f;
            SetCanShoot(true);
            currentShootingProjectileList = new List<Projectile>();
            bulletSpeed = bulletSpeedBottomEnemy;
            
            
        }
        protected virtual void Update(){
            if(canShoot){
                if(!isDead){
                    switch (enemyPos){
                        
                        case EnemyPos.Top:
                            SetWeponRotation();
                        break;

                        case EnemyPos.Bottom:
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
        
        public void SetTarget(Transform _target){
            target = _target;
        }
        private void SetWeponRotation(){
            Vector3 dir = (target.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
            weponPivot.rotation = Quaternion.AngleAxis(angle,Vector3.forward);
        }
        
        protected void Shoot(){
            wepon.ShootBullet(bulletSpeedBottomEnemy);
            
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

        public void OnObjectReuse(){
            SetCanShoot(true);
            fireRate = 0f;
            RestHealth();
            LevelManager.current.AddEnemy(this);
            
            
        }
        public void SetCanShoot(bool _value){
            canShoot = _value;
            
        }
        public void SetEnemyPosition(EnemyPos _enemyPos){

            this.enemyPos = _enemyPos;
            switch (this.enemyPos){
                case EnemyPos.Top:
                    bulletSpeed = bulletSpeedTopEnemy;
                    maxTimeToFire = topEnemyFireRate;
                break;

                case EnemyPos.Bottom:
                    bulletSpeed = bulletSpeedBottomEnemy;
                    maxTimeToFire = bottomEnmyFireRate;
                break;
            }
            
        }
        

        public void DestroyMySelf(){
            canShoot = false;
            gameObject.SetActive(false);
            weponPivot.localRotation = Quaternion.Euler(Vector3.zero);
            LevelManager.current.RemoveEnemy(this);
            DestroyEveryBulletCamFrom();

        }
        private void DestroyEveryBulletCamFrom(){
            foreach(Projectile bullet in currentShootingProjectileList){
                bullet.DestroyMySelf();
                
            }
            currentShootingProjectileList = new List<Projectile>();
        }
    }

}
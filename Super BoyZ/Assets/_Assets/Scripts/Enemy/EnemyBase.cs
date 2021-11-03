using System;
using UnityEngine;
using GamerWolf.Utils;
using System.Collections.Generic;
public enum EnemyPositions{
    Top,Bottom,withBoss
}
public enum EnemyType{
    Minions,Boss,
}
namespace GamerWolf.Super_BoyZ {
    public class EnemyBase : HealthEntity,IPooledObject{

        #region Exposed Variables......
        [SerializeField] private EnemyType enemyType;
        
        [SerializeField] private float bulletSpeedBottomEnemy = 100f,bulletSpeedTopEnemy = 150f;
        
        [SerializeField] private float topEnemyFireRate = 1f,bottomEnmyFireRate = 0.5f;

        [SerializeField] private EnemyWepon wepon;
        [SerializeField] protected Transform weponPivot;

        [Header("Animators")]
        [SerializeField] protected Animator topEnemyGFXanimator;
        [SerializeField] protected Animator bottomEnemyGFXanimator;
        [SerializeField] protected Vector3 topEnemyWeponPivotPos,bottomEnemyWeponPivotPos;


        #endregion

        #region Private Variables......
        private float maxTimeToFire;
        private float fireRate;
        private Transform target;
        private float bulletSpeed;
        public bool canShoot;
        protected List<Projectile> currentShootingProjectileList;
        protected EnemyPositions enemyPos;
        private LevelManager levelManager;
        private bool readyToShoot;
        protected Animator currentGFXAnimator;


        #endregion

        #region Events...
        public Action onFire;
        #endregion
        
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
                            Fireing();
                        break;

                        case EnemyPositions.Bottom:
                            
                            Fireing();
                        break;
                        case EnemyPositions.withBoss:
                            SetWeponRotation();
                            
                        break;
                    }
                    
                    
                }else{
                    DestroyMySelf();
                }
            }
            
        }
        private void Fireing(){
            if(fireRate >= maxTimeToFire){
                Shoot();
                fireRate = 0f;
                
            }else{
                fireRate += Time.deltaTime;
            }
        }
        private void ChangeAnimator(bool bottomSide,bool topSide){
            if(bottomEnemyGFXanimator != null){
                bottomEnemyGFXanimator.gameObject.SetActive(bottomSide);
                if(bottomSide){
                    weponPivot.localPosition = bottomEnemyWeponPivotPos;
                }
                
            }
            if(topEnemyGFXanimator != null){
                topEnemyGFXanimator.gameObject.SetActive(topSide);
                if(topSide){
                    weponPivot.localPosition = topEnemyWeponPivotPos;
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
                    ChangeAnimator(false,true);
                    SetCurrentGFXAnimator(topEnemyGFXanimator);
                break;

                case EnemyPositions.Bottom:
                    bulletSpeed = bulletSpeedBottomEnemy;
                    SetFireRate(bottomEnmyFireRate);
                    ChangeAnimator(true,false);
                    SetCurrentGFXAnimator(bottomEnemyGFXanimator);
                break;
                case EnemyPositions.withBoss:
                    Debug.Log("is With boss");
                    SetFireRate(maxTimeToFire);
                    ChangeAnimator(true,false);
                    
                    SetCurrentGFXAnimator(bottomEnemyGFXanimator);
                break;
            }
            
        }
        private void SetCurrentGFXAnimator(Animator anim){
            if(anim != null){
                currentGFXAnimator = anim;
            }
        }


        #endregion

        #region Parent - Child Methods......
        
        public void Shoot(){
            if(readyToShoot){
                wepon.ShootBullet(bulletSpeedBottomEnemy);
                currentGFXAnimator.SetTrigger("Fire");
                Invoke(nameof(InvokeOnFire),0.5f);
            }
            
        }
        private void InvokeOnFire(){
            onFire?.Invoke();
        }

        public void OnObjectReuse(){
            SetCanShoot(true);
            fireRate = 0f;
            RestHealth();
            Invoke(nameof(InovkeReadyToShoot),1f);
        }
        private void InovkeReadyToShoot(){
            readyToShoot = true;
        }
        

        public void DestroyMySelf(){
            readyToShoot = false;
            canShoot = false;
            gameObject.SetActive(false);
            weponPivot.localRotation = Quaternion.Euler(Vector3.zero);
            DestroyEveryBulletCamFrom();
            levelManager.RemoveMinonEnemy(this);
            levelManager.RemoveBossEnemy(this);
        }
        #region Public Getters...
        public EnemyType GetEnemyType(){
            return enemyType;
        }
        public EnemyPositions GetEnemyPositions(){
            return enemyPos;
        }
        #endregion
        #endregion
        
    }

}
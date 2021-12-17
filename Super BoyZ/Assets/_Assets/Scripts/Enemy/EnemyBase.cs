using System;
using UnityEngine;
using GamerWolf.Utils;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
public enum EnemyPositions{
    No_Boss,withBoss
}
public enum EnemyType{
    Minions,Boss,
}
namespace GamerWolf.Super_BoyZ {
    public class EnemyBase : HealthEntity,IPooledObject{

        #region Exposed Variables......
        [Header("jump Attributes")]
        [SerializeField] private float initJumpForce;
        [Header("Events")]
        [SerializeField] protected UnityEvent onDeathEvent;
        [SerializeField] private EnemyType enemyType;
        
        [SerializeField] private float bulletSpeedBottomEnemy = 100f,bulletSpeedTopEnemy = 150f;
        
        [SerializeField] private float topEnemyFireRate = 1f,bottomEnmyFireRate = 0.5f;

        [SerializeField] private EnemyWepon wepon;
        [SerializeField] protected Transform weponPivot;

        [Header("Animators")]
        [SerializeField] protected Vector3 topEnemyWeponPivotPos,bottomEnemyWeponPivotPos;
        [SerializeField] protected EnemyOnDeath enemyOnDeath;
        [SerializeField] protected Animator currentGFXAnimator;

        [Header("Effects")]
        [SerializeField] private GameObject muzzelFlash;
        


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
        private Rigidbody2D rb2D;
        private bool readyToShoot;


        #endregion

        #region Events...
        public Action onFire;
        #endregion
        private void Awake(){
            rb2D = GetComponent<Rigidbody2D>();
        }
        protected override void Start(){
            base.Start();
            fireRate = 0f;
            SetCanShoot(true);
            currentShootingProjectileList = new List<Projectile>();
            bulletSpeed = bulletSpeedBottomEnemy;
            levelManager = LevelManager.current;
            // base.OnHit += OnProjecitleHit;
        }
        
        protected virtual void Update(){
            if(canShoot){
                if(!isDead){
                    switch (enemyPos){
                        
                        case EnemyPositions.No_Boss:
                            SetWeponRotation();
                            Fireing();
                        break;
                        case EnemyPositions.withBoss:
                            SetWeponRotation();
                        break;
                    }
                    
                    
                }else{
                    PlayDeathAnimaitons();
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
            rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            this.enemyPos = _enemyPos;
            switch (this.enemyPos){
                case EnemyPositions.No_Boss:
                    bulletSpeed = bulletSpeedTopEnemy;
                    SetFireRate(topEnemyFireRate);
                break;
                case EnemyPositions.withBoss:
                    Debug.Log("is With boss");
                    SetFireRate(maxTimeToFire);
                    
                break;
            }
            
        }
        


        #endregion

        #region Parent - Child Methods......
        
        public void Shoot(){
            if(readyToShoot){
                currentGFXAnimator.SetTrigger("Fire");
                wepon.ShootBullet(bulletSpeedBottomEnemy);
                StartCoroutine(showMuzzelFlash());
                Invoke(nameof(InvokeOnFire),0.5f);
            }
            
        }
        private void InvokeOnFire(){
            onFire?.Invoke();
        }
        private IEnumerator showMuzzelFlash(){
            muzzelFlash.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            muzzelFlash.SetActive(false);
        }

        public void OnObjectReuse(){
            
            SetCanShoot(true);
            fireRate = 0f;
            RestHealth();
        
            Invoke(nameof(InovkeReadyToShoot),1f);
        }
        
        public void SetJumpDir(Vector3 dir){
            rb2D.AddForce(dir * initJumpForce);
            
            Invoke(nameof(InvokeConstrains),0.5f);
        }
        
        private void InvokeConstrains(){
            rb2D.constraints = RigidbodyConstraints2D.FreezePositionX;
            rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        
        private void InovkeReadyToShoot(){
            readyToShoot = true;
        }
        public void PlayDeathAnimaitons(){
            rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            onDeathEvent?.Invoke();
            Invoke(nameof(DestroyMySelf),0.2f);
        }
        
        
        public void DestroyMySelf(){
            rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
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
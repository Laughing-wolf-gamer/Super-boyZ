using UnityEngine;
using GamerWolf.Utils;

namespace GamerWolf.Super_BoyZ {
    public class EnemyWepon : MonoBehaviour {
        

        [SerializeField] private Transform firePoint;
        [SerializeField] protected EnemyBase enemyBase;
        private ObjectPoolingManager objectPooling;
        private void Start(){
            objectPooling = ObjectPoolingManager.i;
        }

        public void ShootBullet(float _speed){
            GameObject bullet = objectPooling.SpawnFromPool(PoolObjectTag.Bullets,firePoint.position,firePoint.rotation);
            Projectile projectile = bullet.GetComponent<Projectile>();
            if(projectile != null){
                projectile.SetMoveSpeed(_speed);
            }
            enemyBase.AddToCurrentBulletList(projectile);
        }
        

        
    }

}
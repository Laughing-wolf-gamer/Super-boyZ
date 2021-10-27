using UnityEngine;
using GamerWolf.Utils;

namespace GamerWolf.Super_BoyZ {
    public class Projectile : MonoBehaviour,IPooledObject{


        [SerializeField] private float lifeTime = 10f;
        public float moveSpeed;
        private Rigidbody2D rb;
        
        private void Awake(){
            rb = GetComponent<Rigidbody2D>();
        
        }
        public void SetMoveSpeed(float _speed){
            moveSpeed = _speed;
            rb.AddForce(transform.right * moveSpeed);
        }
        private void OnCollisionEnter2D(Collision2D coli){
            EnemyBase enemy = coli.gameObject.GetComponent<EnemyBase>();
            if(enemy != null){

                enemy.TakeHit(20);
                DestroyMySelf();
            }
            
            if(coli.gameObject.CompareTag("Ground")){
                DestroyMySelf();
            }
        }
        

        public void OnObjectReuse(){
            
            Invoke(nameof(DestroyMySelf),lifeTime);
        }

        public void DestroyMySelf(){
            gameObject.SetActive(false);
            
        }
        

    }

}
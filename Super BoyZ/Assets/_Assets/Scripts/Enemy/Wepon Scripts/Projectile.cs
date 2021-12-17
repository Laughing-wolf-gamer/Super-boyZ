using UnityEngine;
using GamerWolf.Utils;
using UnityEngine.Events;

namespace GamerWolf.Super_BoyZ {
    public class Projectile : MonoBehaviour,IPooledObject{


        [SerializeField] private float lifeTime = 10f;
        [SerializeField] private UnityEvent onResue,onDestroy;
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

            EnemyBase eneity = coli.gameObject.GetComponent<EnemyBase>();

            if(eneity != null){
                Debug.Log("Enemy");
                eneity.TakeHit(1);
                DestroyMySelf();
            }
            
            if(coli.gameObject.CompareTag("Ground")){
                DestroyMySelf();
            }
        }
        // private void OnTriggerEnter2D(Collider2D coli){
        //     HealthEntity eneity = coli.gameObject.GetComponent<HealthEntity>();

        //     if(eneity != null){
        //         Debug.Log("Enemy");
        //         eneity.TakeHit(1);
        //         DestroyMySelf();
        //     }
        // }
        

        public void OnObjectReuse(){
            
            Invoke(nameof(DestroyMySelf),lifeTime);
            onResue?.Invoke();
        }

        public void DestroyMySelf(){
            gameObject.SetActive(false);
            onDestroy?.Invoke();
        }
        

    }

}
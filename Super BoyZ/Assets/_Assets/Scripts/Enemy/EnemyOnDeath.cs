using UnityEngine;


namespace GamerWolf.Super_BoyZ{
    public class EnemyOnDeath : MonoBehaviour {
        
        [SerializeField] private EnemyBase enemyBase;

        public void OnDeath(){
            Debug.Log("Call Death Events"+ this.name);
            enemyBase.DestroyMySelf();
            
        }
        
    }

}
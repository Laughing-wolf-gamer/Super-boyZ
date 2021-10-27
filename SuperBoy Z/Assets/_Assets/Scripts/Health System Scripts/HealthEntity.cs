using System;
using UnityEngine;

namespace GamerWolf.Super_BoyZ {
    public class HealthEntity : MonoBehaviour,IDamagable {
        
        public event EventHandler onDead;

        [SerializeField] protected int maxHealth;
        [SerializeField] protected bool isDead;
        [SerializeField] protected int currentHealth;
        private bool canDie;
        protected virtual void Start(){
            RestHealth();   
        }
        public void RestHealth(){
            isDead = false;
            currentHealth = maxHealth;
        }
        public void TakeHit(int damageValue){
            currentHealth -= damageValue;
            
            if(currentHealth <= 0 && !isDead){
                Die();
            }
        }
        protected virtual void Die(){
            
            isDead = true;
            onDead?.Invoke(this,EventArgs.Empty);
        }
        public virtual void SetCanDie(bool _value){
            canDie = _value;
        }
        public bool GetIsDead(){
            return isDead;
        }
    }

}
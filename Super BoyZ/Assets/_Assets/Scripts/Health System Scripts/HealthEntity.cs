using System;
using UnityEngine;
namespace GamerWolf.Super_BoyZ {
    public class HealthEntity : MonoBehaviour,IDamagable {
        
        public event EventHandler onDead;
        public event EventHandler OnHit;

        [SerializeField] protected int maxHealth;
        [SerializeField] protected bool isDead;
        [SerializeField] protected int currentHealth;
        [SerializeField] protected bool canDie;
        [SerializeField] protected bool canHit;
        protected virtual void Start(){
            RestHealth();   
        }
        public void RestHealth(){
            isDead = false;
            currentHealth = maxHealth;
            SetCanDie(true);
        }
        public void TakeHit(int damageValue){
            if(canDie){
                currentHealth -= damageValue;
                OnHit?.Invoke(this,EventArgs.Empty);
                if(currentHealth <= 0 && !isDead){
                    Die();
                }
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
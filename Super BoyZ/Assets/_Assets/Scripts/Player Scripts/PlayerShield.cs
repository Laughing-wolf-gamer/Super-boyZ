using UnityEngine;


namespace GamerWolf.Super_BoyZ {
    public class PlayerShield : MonoBehaviour {
        
        [SerializeField] private PlayerAnimationsHandler animationsHandler;

        private void OnCollisionEnter2D(Collision2D coli){
            animationsHandler.SetCollided();
        }        
    }

}
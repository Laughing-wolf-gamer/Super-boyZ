using UnityEngine;


namespace GamerWolf.Super_BoyZ {
    public enum currentShieldDirection{
        Right,Left,Up,
    }
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationsHandler : MonoBehaviour {


        
        private Animator animator;

        private void Awake(){
            animator = GetComponent<Animator>();
        }
        public void SetJumping(bool isjumping){
            animator.SetBool("Jump",isjumping);
            
            
        }
        
        public void SetShildShowing(currentShieldDirection dir){
            switch(dir){
                // Animate the character according to the directions.....
                case currentShieldDirection.Right: 
                    animator.SetBool("Right",true);
                    animator.SetBool("isUp",false);
                break;
                case currentShieldDirection.Left: 
                    
                    animator.SetBool("Right",true);
                    animator.SetBool("isUp",false);
                break;
                case currentShieldDirection.Up: 
                    
                    animator.SetBool("Right",false);
                    animator.SetBool("isUp",true);
                break;
                
            }
        }
        
        public void SetCollided(){
            // Play animations for the collisions....
            animator.SetTrigger("isCollided");
        }
    }

}
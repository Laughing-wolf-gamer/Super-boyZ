using UnityEngine;

namespace GamerWolf.Super_BoyZ {
    public class MovementControllesUI : MonoBehaviour {
        
        [SerializeField] private Player player;


        public void MoveRight(){
            player.LookRight();
        }
        public void MoveLeft(){
            player.LookLeft();
        }
        public void MoveTop(){
            player.LookUp();
        }
        public void Jump(){
            player.Jump();
        }
        
    }

}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GamerWolf.Super_BoyZ {
    public class PlayerInput : MonoBehaviour {
        
        [Header("keys")]
        [SerializeField] private KeyCode jumpKey;
        [SerializeField] private KeyCode rightShowShildKey,leftShowShildKey,upShowShildKey;
        

        [Header("Ground Check")]
        [SerializeField] private float checkRange = 3f;
        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private Vector2 offset;



        #region Testing.....
        public bool isRightKeyPressing(){
            return Input.GetKeyDown(rightShowShildKey);
        }
        public bool isLeftKeyPressing(){
            return Input.GetKeyDown(leftShowShildKey);
        }
        public bool isUpKeyPressing(){
            return Input.GetKeyDown(upShowShildKey);
        }
        public bool jumpKeyPressed(){
            return Input.GetKeyDown(jumpKey);
        }
        public bool isGrounded(){
            return Physics2D.OverlapCircle((Vector2)transform.position + offset,checkRange,whatIsGround);
        }
        #endregion
        private void OnDrawGizmosSelected(){
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere((Vector2)transform.position + offset,checkRange);
        }
    }

}
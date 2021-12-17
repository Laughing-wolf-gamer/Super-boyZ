using System;
using UnityEngine;
using GamerWolf.Utils;
namespace GamerWolf.Super_BoyZ {
    [RequireComponent(typeof(Rigidbody2D),typeof(PlayerInput))]
    public class Player : HealthEntity {
        
        [Header("Jumping Paramerters")]
        [SerializeField] private float jumpForce = 200f;
        [SerializeField] private float fallMultiplier = 2.5f;
        [SerializeField] private float lowJumpMutlitplier = 2f;
        
        [Header("Externel Referneces")]
        [SerializeField] private Transform topEnemyTargetPoint;
        [SerializeField] private Transform bottomEnemyTargetPoint;
        [SerializeField] private PlayerAnimationsHandler animationsHandler;
        [SerializeField] private Transform shieldHolder;
        
        
        private bool canJump;
        private bool isjumping;

        private Rigidbody2D rb;
        private PlayerInput playerInput;
        private bool enableInputs;

        #region Singelton......
        public static Player current;
        private void Awake(){
            if(current == null){
                current = this;
            }else{
                Destroy(current.gameObject);
            }

            rb = GetComponent<Rigidbody2D>();
            playerInput = GetComponent<PlayerInput>();
            
        }
        

        

        #endregion
        private void Update(){
            if(enableInputs){
                BetterJump();
                if(playerInput.isLeftKeyPressing()){
                    LookLeft();
                    
                }else if(playerInput.isRightKeyPressing()){
                    LookRight();
                    
                }else if(playerInput.isUpKeyPressing()){
                    LookUp();
                    
                }
                if(playerInput.jumpKeyPressed()){
                    Jump();
                
                }
                if(playerInput.isGrounded()){
                    isjumping = false;
                }
                animationsHandler.SetJumping(isjumping);
            }
        }
        public void Jump(){

            if(playerInput.isGrounded() && !canJump){
                if(!isjumping){
                    canJump = true;
                }
                
            }
            
        }
        public void LookRight(){
            shieldHolder.localEulerAngles = Vector3.zero;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,0f,transform.eulerAngles.z);
            animationsHandler.SetShildShowing(currentShieldDirection.Right);
        }
        public void LookLeft(){
            shieldHolder.localEulerAngles = Vector3.zero;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,180f,transform.eulerAngles.z);
            animationsHandler.SetShildShowing(currentShieldDirection.Left);
        }
        public void LookUp(){
            animationsHandler.SetShildShowing(currentShieldDirection.Up);
            shieldHolder.localEulerAngles = new Vector3(shieldHolder.localEulerAngles.x,shieldHolder.localEulerAngles.y,90f);
        }

        private void BetterJump(){
            if(rb.velocity.y < 0f){
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }else if(rb.velocity.y > 0f && !playerInput.jumpKeyPressed()){
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMutlitplier - 1) * Time.deltaTime;
            }

        }
        private void ResetJump(){
            canJump = false;
        }
        private void FixedUpdate(){
            if(canJump){
                rb.velocity = new Vector2(rb.velocity.x,0f);
                rb.AddForce(Vector2.up * jumpForce,ForceMode2D.Impulse);
                isjumping = true;
                Invoke(nameof(ResetJump),0.02f);
            }
        }
        public Transform GetToptargetPoint(){
            return topEnemyTargetPoint;
        }
        public Transform GetBottomtargetPoint(){
            return bottomEnemyTargetPoint;
        }
        private void OnTriggerEnter2D(Collider2D coli){
            Projectile projectile = coli.GetComponent<Projectile>();
            if(projectile != null){
                TakeHit(20);
                projectile.DestroyMySelf();
            }
        }
        public void SetInputsEnableDesable(bool _value){
            enableInputs = _value;
        }
        

    }

}
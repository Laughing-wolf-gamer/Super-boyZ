using UnityEngine;
using UnityEngine.UI;
namespace GamerWolf.Utils {
    public class UIToggleAnimation : MonoBehaviour {
        private Animator animator;
        private Toggle toggle;
        

        private void Awake(){
            toggle = GetComponent<Toggle>();
            animator = GetComponent<Animator>();
        }
        private void Start(){
            OnToggleClicked();
        }
        public void OnToggleClicked(){
            animator.SetBool("isOn",toggle.isOn);
        }





        
    }

}
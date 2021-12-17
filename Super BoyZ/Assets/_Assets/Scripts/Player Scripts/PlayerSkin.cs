using UnityEngine;


namespace GamerWolf.Super_BoyZ {
    public class PlayerSkin : MonoBehaviour {
        [SerializeField] private PlayerDataSO playerData;
        
        [SerializeField] private SpriteRenderer gfx;


        private void Start(){
            SetCurentSkin();
        }

        public void SetCurentSkin(){
            gfx.color = playerData.curentSkin.testGFXColor;
        }
    }
}

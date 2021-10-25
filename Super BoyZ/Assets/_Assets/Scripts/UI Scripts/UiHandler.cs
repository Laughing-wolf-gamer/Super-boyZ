using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GamerWolf.Super_BoyZ {
    public class UiHandler : MonoBehaviour {
        
        [SerializeField] private TextMeshProUGUI killCountText;
        [SerializeField] private GameObject rewardAdsWindow;

        #region  Singleton....
        public static UiHandler current;
        private void Awake(){
            if(current == null){
                current = this;
            }else{
                Destroy(current.gameObject);
            }
        }

        #endregion


        public void SetKillCounts(int _value){
            killCountText.SetText("Kills : " + _value.ToString());
            
        }
        public void ShowAdWindow(bool _Show){
            rewardAdsWindow.SetActive(_Show);
        }
        

        
    }

}
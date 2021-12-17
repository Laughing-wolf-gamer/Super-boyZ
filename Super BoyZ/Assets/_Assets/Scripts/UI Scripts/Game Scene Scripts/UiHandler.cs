using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GamerWolf.Super_BoyZ {
    public class UiHandler : MonoBehaviour {
        
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private TextMeshProUGUI[] currentkillCountText;
        [SerializeField] private TextMeshProUGUI totalKillCountText;
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


        public void SetCoinCounts(int amount){
            for (int i = 0; i < currentkillCountText.Length; i++){
                currentkillCountText[i].SetText(amount.ToString());
            }
        }
        public void ShowTotalKills(){
            totalKillCountText.SetText(playerData.coinAmount.ToString());

        }
        public void ShowAdWindow(bool _Show){
            rewardAdsWindow.SetActive(_Show);
        }
        

        
    }

}
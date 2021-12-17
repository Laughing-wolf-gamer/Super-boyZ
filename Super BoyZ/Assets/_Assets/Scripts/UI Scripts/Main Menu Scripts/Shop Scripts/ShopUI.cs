using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GamerWolf.Super_BoyZ {
    public class ShopUI : MonoBehaviour {
        
        [SerializeField] private PlayerDataSO playerData;
        [SerializeField] private ShopItemSO[] itemSO;
        [SerializeField] private Image itemGFX;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private int currentItemIndex;
        [SerializeField] private TextMeshProUGUI coinTextUI;


        private void Start(){
            
            ChangeItem();
        }
        public void SetCurrentSkin(){
            for (int i = 0; i < itemSO.Length; i++){
                if(itemSO[i].saveData.isBought){
                    if(itemSO[i].saveData.isUsing){
                        playerData.curentSkin = itemSO[i];
                        break;
                    }
                }
            }
        }
        private void ChangeItem(){
            itemGFX.color = itemSO[currentItemIndex].testGFXColor;
            itemName.SetText(itemSO[currentItemIndex].name);
            UpdateCoinUI();
        }
        private void UpdateCoinUI(){
            coinTextUI.SetText(playerData.coinAmount.ToString());
        }


        public void ShowRight(){
            currentItemIndex++;
            if(currentItemIndex > itemSO.Length - 1){
                currentItemIndex = 0;
            }
            ChangeItem();
        }
        public void ShowLeft(){
            currentItemIndex--;
            if(currentItemIndex < 0){
                currentItemIndex = itemSO.Length - 1;
            }
            ChangeItem();
        }
        public void TryPurchase(){
            if(itemSO[currentItemIndex].cost >= playerData.coinAmount){
                itemSO[currentItemIndex].saveData.isBought = true;
                playerData.RemoveCoins(itemSO[currentItemIndex].cost);
                Use();
            }
            UpdateCoinUI();
        }
        public void Use(){
            if(itemSO[currentItemIndex].saveData.isBought){
                for (int i = 0; i < itemSO.Length; i++){
                    if(i != currentItemIndex){
                        itemSO[i].saveData.isUsing = false;
                    }
                }
                itemSO[currentItemIndex].saveData.isUsing = true;
                playerData.curentSkin = itemSO[currentItemIndex];
            }
            UpdateCoinUI();
        }
        
    }

}
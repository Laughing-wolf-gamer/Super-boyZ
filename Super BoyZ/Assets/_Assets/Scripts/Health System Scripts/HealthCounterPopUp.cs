using TMPro;
using UnityEngine;


namespace GamerWolf.Utils {
    public class HealthCounterPopUp : MonoBehaviour{

        [SerializeField] private TextMeshPro popUpText;
        

        public void SetText(string text){
            popUpText.SetText(text);
        }
        
    }
}

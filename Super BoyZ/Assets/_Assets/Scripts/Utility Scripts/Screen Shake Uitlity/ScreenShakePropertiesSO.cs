using UnityEngine;

namespace GamerWolf.Utils {
    [CreateAssetMenu(fileName = "New Screen Shake Propetie",menuName = "ScriptableObject/Screen Shake/Properties")]
    public class ScreenShakePropertiesSO : ScriptableObject {
        
        public float angle;
        public float strength;
        public float speed;
        public float duration;

        [Range(0,1)]
        public float noisePercent;

        [Range(0,1)]
        public float dampingPercent;
        [Range(0,1)]
        public float rotationPercent;
    }
    

}
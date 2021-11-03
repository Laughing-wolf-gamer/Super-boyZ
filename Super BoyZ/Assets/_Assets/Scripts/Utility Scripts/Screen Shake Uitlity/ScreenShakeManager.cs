using UnityEngine;
using System.Collections;


namespace GamerWolf.Utils {
    public class ScreenShakeManager : MonoBehaviour {
        
        [SerializeField] private ScreenShakePropertiesSO shakeProperties;
        [SerializeField] private bool shakeWithRotaion;
        private IEnumerator currentShakeRoutine;
        private const float maxAngle = 10f;

        #region Singleton......
        public static ScreenShakeManager current;
        private void Awake(){
            if(current == null){
                current = this;
            }else
            {
                Destroy(current.gameObject);
            }
        }
        #endregion
        
        public void StartShake(){
            StartShake(shakeProperties);
        }
        public void StartShake(ScreenShakePropertiesSO properties){
            if(currentShakeRoutine != null){

                StopCoroutine(currentShakeRoutine);
            }
            currentShakeRoutine = ShakeRoutine(properties);
            StartCoroutine(currentShakeRoutine);
        }
        private IEnumerator ShakeRoutine(ScreenShakePropertiesSO properties){
            float completePercent = 0f;
            float movePercent = 0f;
            float angle_radius = properties.angle * Mathf.Deg2Rad - Mathf.PI;
            Vector3 previousWaypoint = Vector3.zero;
            Vector3 currentWayPoint = Vector3.zero;
            float moveDistance = 0f;

            Quaternion targetRotation = Quaternion.identity;
            Quaternion previousRotation = Quaternion.identity;
            
            do{
                if(movePercent >= 1 || completePercent == 0f){
                    float dampingFactor = DampingCurve(completePercent,properties.dampingPercent);
                    float noiseAngle = (Random.value - 0.5f) * Mathf.PI;
                    angle_radius += Mathf.PI + noiseAngle * properties.noisePercent;
                    currentWayPoint = new Vector3(Mathf.Cos(angle_radius),Mathf.Sin(angle_radius)) * properties.strength * dampingFactor;
                    previousWaypoint = transform.localPosition;
                    moveDistance = Vector3.Distance(currentWayPoint,previousWaypoint);
                    if(shakeWithRotaion){
                        targetRotation = Quaternion.Euler(new Vector3(currentWayPoint.y,currentWayPoint.x).normalized * properties.rotationPercent * dampingFactor * maxAngle);
                        previousRotation = transform.localRotation;
                    }
                    movePercent = 0f;
                }
                completePercent += Time.deltaTime / properties.duration;
                movePercent += Time.deltaTime / moveDistance * properties.speed;
                transform.localPosition = Vector3.Lerp(previousWaypoint , currentWayPoint,movePercent);
                if(shakeWithRotaion){
                    transform.localRotation = Quaternion.Slerp(previousRotation,targetRotation,movePercent);
                }
                yield return null;
            }while(moveDistance > 0);
        }
        private float DampingCurve(float x,float dampingPercent){
            x = Mathf.Clamp01(x);
            float a = Mathf.Lerp(2,0.25f,dampingPercent);
            float b = 1 - Mathf.Pow(x,a);
            return b * b * b;
        }
    }
    

}
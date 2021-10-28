using System;
using UnityEngine;
using System.Collections.Generic;

namespace GamerWolf.Utils{
    public class TimerTickSystem{
        private static List<TimerTickSystem> activeTimerList;
        private static GameObject initGameObject;
        private static void InitIfNeeded(){
            if(initGameObject == null){
                initGameObject = new GameObject("Timer Tick GameObject");
                activeTimerList = new List<TimerTickSystem>();
            }
        }
        public static TimerTickSystem CreateTimer( Action _action,float _maxtimer,string _timerName = null){
            InitIfNeeded();
            GameObject gameObject = new GameObject("Timer tick"/*,typeof(MonoBehaviourHood)*/);
            gameObject.AddComponent<MonoBehaviourHood>();
            TimerTickSystem timerTickSystem = new TimerTickSystem(_timerName,_action,_maxtimer,gameObject);
            activeTimerList.Add(timerTickSystem);
            gameObject.GetComponent<MonoBehaviourHood>().onUpdateAction = timerTickSystem.Update;
            return timerTickSystem;
        }
        private static void RemoveTimer(TimerTickSystem _timerTickSystem){
            InitIfNeeded();
            activeTimerList.Remove(_timerTickSystem);
        }
        public static void PauseTimers(string _timerName,bool _pause){
            for (int i = 0; i < activeTimerList.Count; i++){
                if(activeTimerList[i].timerName == _timerName){
                    activeTimerList[i].Pause(_pause);
                }
            }
        }
        public static void StopTimer(string _timerName){
            for (int i = 0; i < activeTimerList.Count; i++){
                if(activeTimerList[i].timerName == _timerName){
                    activeTimerList[i].DestroyMySelf();
                    i--;
                }
            }
        }
        private class MonoBehaviourHood: MonoBehaviour{
            public Action onUpdateAction;
            private void Update(){
                if(onUpdateAction != null) onUpdateAction();
            }
        }
        private Action action;
        private float timerMax;
        private float currenttimer;
        private bool isDestoryed;
        private GameObject gameObject1;
        private string timerName;
        private bool pauseTimer;
        private TimerTickSystem(string _timerName,Action _action,float _timerMax,GameObject _gameObject){
            action = _action;
            timerMax = _timerMax;
            currenttimer = 0f;
            isDestoryed = false;
            gameObject1 = _gameObject;
            timerName = _timerName;
            pauseTimer = false;
        }

        public void Update(){
            if(!isDestoryed){
                if(!pauseTimer){
                    if(currenttimer >= timerMax){
                        currenttimer -= timerMax;
                        action();
                    }else{
                        currenttimer += Time.deltaTime;
                    }
                }
            }
        }

        private void Pause(bool isPause){
            pauseTimer = isPause;
        }
        private void DestroyMySelf(){
            isDestoryed = true;
            RemoveTimer(this);
            StopTimer(timerName);
            UnityEngine.Object.Destroy(gameObject1);
        }
        
        
        

    }

}
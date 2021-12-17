using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace GamerWolf.Utils{
    public enum SceneIndex{
        persistantScene = 1,Main_Menu = 2,Game_Scene = 3, /*Level_2 = 3,Level_3 = 4,Level_4 = 5,Level_5 = 6,Level_6 = 7,Level_7 = 8,Level_8 = 9,Level_9 = 10,Level_10 = 11,
        Level_11 = 12,Level_12 = 13,Level_13 = 14,Level_14 = 15,Level_15 = 16,Level_16 = 17,Level_17 = 18,Level_18 = 19,Level_19 = 20,Level_20 = 21
        ,Level_21 = 22,Level_22 = 23,Level_23 = 24,Level_24 = 25, Level_25 = 26, Level_26 = 27, Level_27 = 28,Level_28 = 29,Level_29 = 30,Level_30 = 31,Level_31 = 32,Level_32 = 33,
        Level_33 = 34,Level_34 = 35, Level_35 = 36,Level_36 = 37, Level_37 = 38, Level_38 = 39,Level_39 = 40, Level_40 = 41, Level_41 = 42, Level_42 = 43, Level_43 = 44,
        Level_44 = 45, Leve_45 = 46, Level_46 = 47, Level_47 = 48, Level_48 = 49, Level_49 = 50,Level_50 = 51, Leve_51 = 52, Level_52 = 53, Level_53 = 54, Level_54 = 55, Level_55 = 56,
        Level_56 = 57, Level_57 = 58, Level_58 = 59, Level_59 = 60, Level_60 = 61,Level_61 = 62, Level_62 = 63,Level_63 = 64, Level_64 = 65, Level_65 = 66, Level_66 = 67, Level_67 = 68, Level_68 = 69,
        Level_69 = 70, Level_70 = 71, Level_71 = 72, Level_72 = 71, Level_73 = 74, Level_74 = 75,Level_75 = 76, Level_76 = 77, Level_77 = 78, Level_78 = 79, Level_79 = 80, Level_80 = 81,
        Level_81 = 82, Level_82 = 83, Level_83 = 84, Level_84 = 85, Level_85 = 86, Level_86 = 87,Level_87 = 88, Level_88 = 89, Level_89 = 90, Level_90 = 91, Level_91 = 92, Level_92 = 93,
        Level_93 = 94, Level_94 = 95, Level_95 = 96, Level_96 = 97, Level_97 = 98, Level_98 = 99,Level_99 = 100,Level_100 = 101,*/
    }

    public class LevelLoader : MonoBehaviour{
        
        [SerializeField] private GameObject loadingScreen;
        // [SerializeField] private List<LevelDataSO> levelList;
        [SerializeField] private Image loadingBar;
        [SerializeField] private SceneIndex currentLevel;
        public static LevelLoader current {get;private set;}
        private float totalProgress;
        private void Awake(){
        #if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
        #else
            Debug.unityLogger.logEnabled = false;
        #endif
            DontDestroyOnLoad(this.gameObject);
            if(current == null){
                current = this;
            }else{
                Destroy(current.gameObject);
            }

            if(loadingScreen == null){
                loadingScreen = transform.Find("loadingScreen").gameObject;
            }
            
        }
        
        
        private void Start(){
            PlayLevel(SceneIndex.Main_Menu);
            // for (int i = 0; i < levelList.Count; i++){
            //     if(levelList[i].isCompleted){
            //         levelList.Remove(levelList[i]);
            //         i--;
            //     }else{
            //         break;
            //     }
            // }
            // currentLevel = levelList[0].sceneIndex;
        }
        
        public void PlayLevel(){
            // SceneIndex newLevel = (SceneIndex)UnityEngine.Random.Range(2,Enum.GetValues(typeof(SceneIndex)).Length);
            SwitchScene(currentLevel);
        }
        
        public void PlayLevel(SceneIndex levelIndex){
            SwitchScene(levelIndex);
        }
        // public void UpdateLevelData(LevelDataSO level){
        //     if(level.isCompleted){
        //         level.Save();
        //         levelList.Remove(level);
        //     }
        //     currentLevel = levelList[0].sceneIndex;
        // }
        
        
        
        public void MoveToNextLevel(){
            SwitchScene(currentLevel);
            
        }
        
        
        public void SwitchScene(SceneIndex sceneToLoad){
            StartCoroutine(GetLoadSceneProgress(sceneToLoad));
        }
        
        
        private IEnumerator GetLoadSceneProgress(SceneIndex _sceneToLoad){
            float extraLoading = 2f;
            loadingScreen.SetActive(true);
            AsyncOperation operation = SceneManager.LoadSceneAsync((int)_sceneToLoad);
            totalProgress = 0f;
            while(!operation.isDone){
                totalProgress = Mathf.Clamp01(operation.progress / 0.9f) + extraLoading;
                loadingBar.fillAmount = totalProgress;
                yield return null;
                
            }
            loadingScreen.SetActive(false);
            
        }
    }

}
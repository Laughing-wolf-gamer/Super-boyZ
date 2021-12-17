using System.IO;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace GamerWolf.Super_BoyZ { 
    [CreateAssetMenu(fileName = "New Shop Item",menuName = "ScriptableObject/Shop/Shop Item")]
    public class ShopItemSO : ScriptableObject {
        
        public string itemName;
        public Color testGFXColor;
        public ShopItemSaveData saveData;
        public int cost;

        [ContextMenu("Save")]
        public void Save(){
            string data = JsonUtility.ToJson(saveData,true);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Create(string.Concat(Application.persistentDataPath,"/",name));
            formatter.Serialize(file,data);
            file.Close();
        }

        [ContextMenu("Load")]
        public void Load(){
            if(File.Exists((string.Concat(Application.persistentDataPath,"/",name)))){
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream Stream = File.Open(string.Concat(Application.persistentDataPath,"/",name),FileMode.Open);
                JsonUtility.FromJsonOverwrite(formatter.Deserialize(Stream).ToString(),saveData);
                Stream.Close();
            }
        }
        [ContextMenu("Clear")]
        public void Reset(){
            saveData.isBought = false;
        }
    }
    [System.Serializable]
    public struct ShopItemSaveData{
        public bool isBought;
        public bool isUsing;
        

    }

}
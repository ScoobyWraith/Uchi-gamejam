using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsByProgress", menuName = "Configs/Levels by progress")]
public class LevelsByProgress : ScriptableObject
{
    [System.Serializable]
    public struct LevelByProgress
    {
        public int progress;
        public string sceneName;
    }
    
    public List<LevelByProgress> items = new List<LevelByProgress>();
    
    private Dictionary<int, LevelByProgress> itemDict;
    
    public LevelByProgress GetItem(int progress)
    {
        if (itemDict == null)
        {
            itemDict = new Dictionary<int, LevelByProgress>();

            foreach (var item in items)
                itemDict[item.progress] = item;
        }
        
        return itemDict.TryGetValue(progress, out var data) ? data : default;
    }
}

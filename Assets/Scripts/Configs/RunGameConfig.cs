using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "RunGameConfig", menuName = "Configs/Run Game Config")]
public class RunGameConfig : ScriptableObject
{
    [System.Serializable]
    public struct SettingsByProgress
    {
        public int progress;
        public RunGameSettings settings;
    }
    
    public List<SettingsByProgress> items = new List<SettingsByProgress>();
    
    public RunGameSettings GetSettingsByProgress(int progress)
    {
        Assert.AreNotEqual(0, items.Count, "Список настроек RunGameConfig пуст");
        
        SettingsByProgress result = items[0];

        foreach(SettingsByProgress settingsByProgress in items)
        {
            if (settingsByProgress.progress <= progress)
            {
                result = settingsByProgress;
            } 
            else
            {
                break;
            }
        }

        Debug.Log($"Загружены настройки Run Game для прогресса {result.progress}");

        return result.settings;
    }
}

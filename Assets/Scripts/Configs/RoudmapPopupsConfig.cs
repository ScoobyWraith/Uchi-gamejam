using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


[CreateAssetMenu(fileName = "RoudmapPopupsConfig", menuName = "Configs/Roudmap Popups Config")]
public class RoudmapPopupsConfig : ScriptableObject
{
    [System.Serializable]
    public struct Settings
    {
        public int progress;
        public string popup;
    }
    
    public string folder;
    public List<Settings> items = new List<Settings>();

    public string GetPopupName(int progress)
    {
        Assert.AreNotEqual(0, items.Count, "Список настроек RoudmapPopupsConfig пуст");
        
        Settings result = items[0];

        foreach(Settings settingsByProgress in items)
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

        Debug.Log($"Загружены настройки Roudmap Popup для прогресса {result.progress}");

        return result.popup;
    }
}

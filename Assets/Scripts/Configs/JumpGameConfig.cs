using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpGameConfig", menuName = "Configs/Jump Game Config")]
public class JumpGameConfig : ScriptableObject
{
    [System.Serializable]
    public struct SettingsByProgress
    {
        public int progress;
        public JumpGameSettings settings;
    }
    
    public List<SettingsByProgress> items = new List<SettingsByProgress>();
    
    public JumpGameSettings GetSettingsByProgress(int progress)
    {
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

        Debug.Log($"Загружены настройки Jump Game для прогресса {result.progress}");

        return result.settings;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "OrigamiConfig", menuName = "Configs/Origami")]
public class OrigamiConfig : ScriptableObject
{
    [System.Serializable]
    public struct Settings
    {
        public string fileName;
        
        [TextArea]
        public string text;
    }
    
    public string folder;
    public List<Settings> items = new List<Settings>();
}

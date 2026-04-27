using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IslandsByProgress", menuName = "Configs/Islands by progress")]
public class IslandsByProgress : ScriptableObject
{
    public List<int> complited = new List<int>();
    
    public List<int> GetCompleted(int progress)
    {
        List<int> result = new List<int>();

        for (int i = 0; i < complited.Count; i++)
        {
            if (progress < complited[i])
            {
                break;
            }

            result.Add(i);
        }

        return result;
    }
}

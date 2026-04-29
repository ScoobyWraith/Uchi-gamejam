using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class RoadMap : MonoBehaviour
{
   public IslandsByProgress islandsByProgress;
   public GameObject islandsPool;
   
   private List<IslandButtonUIStates> islands = new List<IslandButtonUIStates>();
   
    public void Start()
    {
        Transform parent = islandsPool.transform;
        
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            islands.Add(child.GetComponentInChildren<IslandButtonUIStates>());
        }
        
        islands.Sort((a, b) => ExtractNumber(a.gameObject.name).CompareTo(ExtractNumber(b.gameObject.name)));
        
        LoadRoadMap();
    }

    public void LoadRoadMap()
    {
        GlobalGame globalGame = GlobalGame.GetInstance();

        int currentProgress = globalGame.GetCurrentProgress();
        List<int> completedIslands = islandsByProgress.GetCompleted(currentProgress);
        int currentIsland = 0;

        if (completedIslands.Count > 0)
        {
            currentIsland = completedIslands[completedIslands.Count - 1] + 1;
        }

        for (int i = 0; i < islands.Count; i++)
        {
            IslandButtonUIStates island = islands[i];
            
            if (i < currentIsland)
            {
                island.ShowCompleteState();
            }
            else if (i == currentIsland)
            {
                island.ShowUncompleteState();
            } 
            else
            {
                island.ShowLockedState();
            }
        }
    }

    private int ExtractNumber(string name)
    {
        Match match = Regex.Match(name, @"island-(\d+)$");
        
        if (match.Success && int.TryParse(match.Groups[1].Value, out int number))
        {
            return number;
        }

        return 0;
    }
}

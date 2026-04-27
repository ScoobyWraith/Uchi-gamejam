using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemCleaner : MonoBehaviour
{
    void Awake()
    {
        EventSystem[] systems = FindObjectsOfType<EventSystem>();
        
        if (systems.Length > 1)
        {
            for (int i = 1; i < systems.Length; i++)
            {
                if (systems[i].gameObject != null)
                {
                    Destroy(systems[i].gameObject);
                }
            }
        }

        AudioListener[] listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        
        if (listeners.Length > 1)
        {
            for (int i = 1; i < listeners.Length; i++)
            {
                if (listeners[i] != null)
                {
                    Destroy(listeners[i]);
                }
            }
        }
    }
}
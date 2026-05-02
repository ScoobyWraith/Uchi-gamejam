using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AnswerButtonStateManager : MonoBehaviour
{
    public GameObject blockimage;
    public GameObject availableBlock;

    private Button button;

    public void Start()
    {
        button = GetComponentInChildren<Button>(true);
    }

    public void ToBlockedState()
    {
        blockimage.SetActive(true);
        availableBlock.SetActive(false);
    }

    public void ToAvailabledState()
    {
        blockimage.SetActive(false);
        availableBlock.SetActive(true);
    }

    public void AddButtonClickListener(UnityAction action)
    {
        button.onClick.AddListener(action);
    }

    public void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
        }
    }
}

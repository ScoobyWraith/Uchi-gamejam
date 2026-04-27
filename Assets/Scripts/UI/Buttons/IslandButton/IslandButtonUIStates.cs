using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class IslandButtonUIStates : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Animator animator;
    public GameObject activeStar;
    public GameObject inactiveStar;

    private Button button;
    private const string isHighlighted = "isHighlighted";
    private const string isPressed = "isPressed";

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void ToCompleteState()
    {
        activeStar.SetActive(true);
        inactiveStar.SetActive(false);
    }

    public void ToUncompleteState()
    {
        activeStar.SetActive(false);
        inactiveStar.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
            animator.SetBool(isHighlighted, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool(isHighlighted, false);
        animator.SetBool(isPressed, false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable)
            animator.SetBool(isPressed, true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        animator.SetBool(isPressed, false);
    }

    public void onClick(int islandNumber)
    {
        LevelsLoader levelsLoader = LevelsLoader.GetInstance();
        levelsLoader.LoadIsland(islandNumber);
    }
}

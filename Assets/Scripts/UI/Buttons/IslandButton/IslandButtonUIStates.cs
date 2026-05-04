using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class IslandButtonUIStates : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Animator animator;
    public GameObject activeStar;
    public GameObject inactiveStar;
    public GameObject round;
    public GameObject keylock;
    public GameObject completeIcon;

    private Button button;
    private bool isInteractable = true;
    private Color[] originalColors;
    private Image[] images;
    private const string isHighlighted = "isHighlighted";
    private const string isPressed = "isPressed";

    private void Start()
    {
        button = GetComponent<Button>();
        SaveOriginalColors();
    }

    public void ShowCompleteState()
    {
        ShowCompleteStar();
        ShowCompleteIcon();
        button.enabled = false;
        isInteractable = false;
        RestoreColorsOfImages();
    }

    public void ShowUncompleteState()
    {
        ShowUnompleteStar();
        ShowRound();
    }

    public void ShowLockedState()
    {
        SetImagesToBlackWhite();
        ShowKeylock();
        HideStar();
        button.interactable = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable && isInteractable)
            animator.SetBool(isHighlighted, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool(isHighlighted, false);
        animator.SetBool(isPressed, false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable && isInteractable)
            animator.SetBool(isPressed, true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        animator.SetBool(isPressed, false);
    }

    public void onClick(int islandNumber)
    {
        AudioManager.GetInstance().PlayClickSound();
        LevelsLoader levelsLoader = LevelsLoader.GetInstance();
        levelsLoader.LoadIsland(islandNumber);
    }

    private void SaveOriginalColors()
    {
        images = GetComponentsInChildren<Image>();
        originalColors = new Color[images.Length];
        int i = 0;

        foreach(Image img in images)
        {
            originalColors[i++] = img.color;
        }
    }

    private void ShowCompleteStar()
    {
        activeStar.SetActive(true);
        inactiveStar.SetActive(false);
    }

    private void ShowUnompleteStar()
    {
        activeStar.SetActive(false);
        inactiveStar.SetActive(true);
    }

    private void HideStar()
    {
        activeStar.SetActive(false);
        inactiveStar.SetActive(false);
    }

    private void ShowRound()
    {
        round.SetActive(true);
        keylock.SetActive(false);
        completeIcon.SetActive(false);
    }

    private void ShowKeylock()
    {
        round.SetActive(false);
        keylock.SetActive(true);
        completeIcon.SetActive(false);
    }

    private void ShowCompleteIcon()
    {
        round.SetActive(false);
        keylock.SetActive(false);
        completeIcon.SetActive(true);
    }

    private void SetImagesToBlackWhite()
    {
        foreach (Image img in images)
        {
            float gray = img.color.grayscale;
            img.color = new Color(gray, gray, gray, img.color.a);
        }
    }

    private void RestoreColorsOfImages()
    {
        int i = 0;
        
        foreach (Image img in images)
        {
            img.color = originalColors[i++];
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class QuizButton : MonoBehaviour
{
    public bool IsCorrect = false;

    public Color GoodColor = Color.green;
    public Color BadColor = Color.red;

    private Image background;
    private Color defaultColor;

    private Button button;

    public void Awake()
    {
        background = GetComponent<Image>();
        defaultColor = background.color;
        button = GetComponentInChildren<Button>();
    }

    public void onClick()
    {
        if (IsCorrect)
        {
            background.color = GoodColor;
        } else
        {
            background.color = BadColor;
        }
    }

    public void reset()
    {
        background.color = defaultColor;
    }

    public Button getButton()
    {
        return button;
    }
}

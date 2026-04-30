using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SliderCarousel : MonoBehaviour
{
    public GameObject slidesObject;
    public GameObject arrowsObject;
    public GameObject stepsObject;
    public bool useOk = true;
    public bool isStepStyle;
    public UnityEvent onOkClick;

    private Button leftButton;
    private Button rightButton;
    private Button okButton;
    private Text middleText;
    private Text leftext;
    private Text rightText;
    private List<GameObject> slides = new List<GameObject>();
    private int currentSlide = 0;

    void Awake()
    {
        CollectSlides();
        InitButtons();
        ToSlide(0);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        ToSlide(0);
        gameObject.SetActive(true);
    }

    private void InitButtons()
    {
        GameObject navigation = null;

        if (isStepStyle)
        {
            arrowsObject.SetActive(false);
            navigation = stepsObject;
        }
        else
        {
            stepsObject.SetActive(false);
            navigation = arrowsObject;
        }

        navigation.SetActive(true);
        Button[] buttons = navigation.GetComponentsInChildren<Button>(true);

        foreach (Button b in buttons)
        {
            if (b.name.Equals("Left"))
            {
                leftButton = b;
            }
            else if (b.name.Equals("Right"))
            {
                rightButton = b;
            }
            else
            {
                okButton = b;
            }
        }

        if (isStepStyle)
        {
            GameObject middle = navigation.transform.Find("Middle").gameObject;
            middleText = middle.GetComponentInChildren<Text>();

            leftext = leftButton.GetComponentInChildren<Text>();
            rightText = rightButton.GetComponentInChildren<Text>();
        }

        leftButton.onClick.AddListener(OnLeftClick);
        rightButton.onClick.AddListener(OnRightClick);

        okButton.onClick.AddListener(Close);
        okButton.onClick.AddListener(() => onOkClick.Invoke());
    }
    
    private void OnLeftClick()
    {
        ToSlide(currentSlide - 1);
    }

    private void OnRightClick()
    {
        ToSlide(currentSlide + 1);
    }

    private void CollectSlides()
    {
        Transform parent = slidesObject.transform;
        
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            slides.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }

        slides.Sort((a, b) => ExtractNumber(a.name).CompareTo(ExtractNumber(b.name)));
    }

    private void ToSlide(int n)
    {
        currentSlide = Mathf.Clamp(n, 0, slides.Count - 1);

        foreach(GameObject g in slides)
        {
            g.SetActive(false);
        }

        slides[currentSlide].SetActive(true);
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        if (slides.Count == 1)
        {
            leftButton.gameObject.SetActive(false);
            rightButton.gameObject.SetActive(false);
            okButton.gameObject.SetActive(false);
            
            if (useOk)
            {
                okButton.gameObject.SetActive(true);
            }


            if (isStepStyle)
            {
                middleText.text = "";
            }

            return;
        }

        if (isStepStyle)
        {
            leftext.text = $"к шагу {currentSlide}";
            middleText.text = $"шаг {currentSlide + 1}";
            rightText.text = $"к шагу {currentSlide + 2}";
        }

        if (currentSlide == 0)
        {
            leftButton.gameObject.SetActive(false);
            rightButton.gameObject.SetActive(true);
            okButton.gameObject.SetActive(false);

            return;
        }

        if (currentSlide == slides.Count - 1)
        {
            leftButton.gameObject.SetActive(true);
            rightButton.gameObject.SetActive(false);
            okButton.gameObject.SetActive(false);
            
            
            if (useOk)
            {
                okButton.gameObject.SetActive(true);
            }

            return;
        }

        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(true);
        okButton.gameObject.SetActive(false);
    }

    private int ExtractNumber(string name)
    {
        Match match = Regex.Match(name, @"Slide-(\d+)$");
        
        if (match.Success && int.TryParse(match.Groups[1].Value, out int number))
        {
            return number;
        }

        return 0;
    }

    void OnDestroy() {
        leftButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();
        okButton.onClick.RemoveAllListeners();
    }
}

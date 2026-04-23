using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SliderCarousel : MonoBehaviour
{
    public GameObject SlidesObject;

    public Button LeftButton;

    public Button RightButton;

    public Button OkButton;

    public bool UseOk = true;

    private List<GameObject> slides = new List<GameObject>();

    private int currentSlide = 0;

    void Awake()
    {
        Transform parent = SlidesObject.transform;
        
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            slides.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }

        slides.Sort((a, b) => ExtractNumber(a.name).CompareTo(ExtractNumber(b.name)));

        ToSlide(0);
    }

    public void onOkClick()
    {
        close();
    }

    public void close()
    {
        gameObject.SetActive(false);
    }

    public void open()
    {
        gameObject.SetActive(true);
    }

    public void onLeftClick()
    {
        ToSlide(currentSlide - 1);
    }

    public void onRightClick()
    {
        ToSlide(currentSlide + 1);
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
            LeftButton.gameObject.SetActive(false);
            RightButton.gameObject.SetActive(false);
            OkButton.gameObject.SetActive(false);
            
            if (UseOk)
            {
                OkButton.gameObject.SetActive(true);
            }

            return;
        }


        if (currentSlide == 0)
        {
            LeftButton.gameObject.SetActive(false);
            RightButton.gameObject.SetActive(true);
            OkButton.gameObject.SetActive(false);

            return;
        }

        if (currentSlide == slides.Count - 1)
        {
            LeftButton.gameObject.SetActive(true);
            RightButton.gameObject.SetActive(false);
            OkButton.gameObject.SetActive(false);
            
            
            if (UseOk)
            {
                OkButton.gameObject.SetActive(true);
            }
            
            return;
        }

        LeftButton.gameObject.SetActive(true);
        RightButton.gameObject.SetActive(true);
        OkButton.gameObject.SetActive(false);
    }
}

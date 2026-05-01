using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    public GameObject answersObject;
    public AnswerButtonStateManager correctAnswer;

    public ModalView wrongAnswerModal;
    public ModalView correctAnswerModal;

    private List<AnswerButtonStateManager> answers = new List<AnswerButtonStateManager>();

    public void Start()
    {
        StartCoroutine(Load());
    }

    public void CompleteQuiz()
    {
        GlobalGame globalGame = GlobalGame.GetInstance();
        LevelsLoader levelsLoader = LevelsLoader.GetInstance();

        globalGame.IncProgress();
        levelsLoader.LoadLevelByProgress();
    }

    public void ShowCorrectDialog()
    {
        correctAnswerModal.OpenModal();
    }

    public void ShowUncorrectDialog()
    {
        wrongAnswerModal.OpenModal();
    }

    public void OpenAllButton()
    {
        Debug.Log(answers.Count);
        
        foreach (AnswerButtonStateManager item in answers)
        {
            item.ToAvailabledState();
        }
    }
    
    private IEnumerator Load()
    {
        yield return null;
        
        LoadButtons();
        ScenesLoader.SceneLoaded();
    }

    private void LoadButtons()
    {
        AnswerButtonStateManager[] ans = answersObject.GetComponentsInChildren<AnswerButtonStateManager>();

        foreach (AnswerButtonStateManager item in ans)
        {
            answers.Add(item);
            
            if (item.Equals(correctAnswer))
            {
                item.AddButtonClickListener(ShowCorrectDialog);
            }
            else
            {
                item.AddButtonClickListener(ShowUncorrectDialog);
            }

            item.ToBlockedState();
        }
    }
}

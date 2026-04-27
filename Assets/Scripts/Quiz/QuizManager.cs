using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    public GameObject Answers;

    public ModalView WrongAnswerModal;
    public ModalView CorrectAnswerModal;

    private List<QuizButton> answers = new List<QuizButton>();

    public void Start()
    {
        Transform parent = Answers.transform;

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            QuizButton quizButton = child.gameObject.GetComponent<QuizButton>();
            answers.Add(quizButton);

            if (!quizButton.IsCorrect)
            {
                quizButton.getButton().onClick.AddListener(() => {
                    WrongAnswerModal.OpenModal();
                });
            } else
            {
                quizButton.getButton().onClick.AddListener(() => {
                    CorrectAnswerModal.OpenModal();
                });
            }
        }
    }

    public void reset()
    {
        foreach(QuizButton button in answers)
        {
            button.reset();
        }
    }

    public void CompleteQuiz()
    {
        GlobalGame globalGame = GlobalGame.GetInstance();
        LevelsLoader levelsLoader = LevelsLoader.GetInstance();

        globalGame.IncProgress();
        levelsLoader.LoadLevelByProgress();
    }
}

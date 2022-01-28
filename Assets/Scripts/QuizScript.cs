using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizScript : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] QuestionSO[] questions;

    [Header("Texts")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] TextMeshProUGUI[] answerButtonsText;
    [SerializeField] TextMeshProUGUI succesRatioText;

    [Header("Button Sprites")]
    [SerializeField] Sprite normalButtonSprite;
    [SerializeField] Sprite selectedButtonSprite;

    [Header("Answer Buttons")]
    [SerializeField] GameObject[] answerButtons;

    TimerScript timerScript;

    int selectedAnswerIndex;
    int questionNumberIndex=0;
    int trueAnswers=0;
    int seenQuestionNumber=0;

    bool isAnswerSelected;
    bool hasGameOver;
 
    void Awake()
    {
        timerScript = GetComponent<TimerScript>();
    }
    void Start()
    {       
        QuestionDisplay();       
    }
    void Update()
    {
        Debug.Log("true:"+trueAnswers+"   seen:"+seenQuestionNumber);  
        if (isAnswerSelected&&!hasGameOver)
        {
            WhenAnswerSelected();           
        }
       

    }   
    void WhenAnswerSelected()
    {
        
        answerButtons[selectedAnswerIndex].GetComponent<Image>().sprite = selectedButtonSprite;
        foreach (var buttons in answerButtons)
        {
            buttons.GetComponent<Button>().interactable = false;
        }
        if (selectedAnswerIndex == questions[questionNumberIndex - 1].GetCorrectAnswerIndex())
        {
            trueAnswers++;
            SuccesRatio();
            questionText.text = "Congratulations! True Answer";      
            if (questionNumberIndex == questions.Length)
            {
                GameOver();
            }
            isAnswerSelected = false;
        }
        else
        {
            SuccesRatio();
            questionText.text = "True Answer Was:" + answerButtonsText[questions[questionNumberIndex - 1].GetCorrectAnswerIndex()].text;
            if (questionNumberIndex == questions.Length)
            {
                GameOver();
            }
            isAnswerSelected = false;
        }
    }
    public void GetNextQuestion()
    {
        if (!hasGameOver)
        {
            QuestionDisplay();
            isAnswerSelected = false;
            foreach (var buttons in answerButtons)
            {
                buttons.GetComponent<Button>().interactable = true;
                buttons.GetComponent<Image>().sprite = normalButtonSprite;
            }
        }           
    }
    public void GetSelectedAnswerIndex(int answerIndex)
    {
        selectedAnswerIndex = answerIndex;
        isAnswerSelected = true;
        timerScript.AnswerSelected();      
    }
    public void QuestionDisplay()
    {
        seenQuestionNumber++;
        SliderControl();
        questionText.text = questions[questionNumberIndex].GetQuestion();
        for (int i = 0; i < 4; i++)
        {
            answerButtonsText[i].text = questions[questionNumberIndex].GetAnswer(i); //Cevapları döndürüyor.         
        }
        questionNumberIndex++;
    }
    void SuccesRatio()
    {
        succesRatioText.text ="Succes:%"+  Mathf.RoundToInt(trueAnswers/(float)seenQuestionNumber*100);
    }
    void GameOver()
    {
        hasGameOver = true;
        timerScript.IsGameOver = true;

        Invoke("EndGameScreen", 3);
    }
    void EndGameScreen()
    {
        questionText.text = "GAME OVER \n " +
                           "Your Score: %"+ Mathf.RoundToInt(trueAnswers / (float)seenQuestionNumber * 100);
        foreach (var button in answerButtons)
        {
            button.SetActive(false);
        }
        FindObjectOfType<Slider>().gameObject.SetActive(false);
    }
    void SliderControl()
    {
        FindObjectOfType<Slider>().GetComponent<Slider>().value = seenQuestionNumber;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    [SerializeField] Image timerImage;
    [SerializeField] float answeringTime=10f;
    [SerializeField] float transitionTime = 5f;

    float time;
    public bool IsGameOver { get; set; }

    QuizScript quizScript;

    void Awake()
    {
        timerImage.fillAmount = 1;
        quizScript = GetComponent<QuizScript>();
        time = answeringTime;
    }
    void Update()
    {
        Timer();
    }
    void Timer()
    {
        if (IsGameOver)
        {
            timerImage.enabled = false;
        }
        timerImage.fillAmount -= 1 / time * Time.deltaTime;
        if (timerImage.fillAmount==0)
        {
          
            quizScript.GetNextQuestion();
            timerImage.fillAmount =1;
            time = answeringTime;
        }
    }
    public void AnswerSelected()
    {
        time = transitionTime;
        timerImage.fillAmount = 1;
    }
}

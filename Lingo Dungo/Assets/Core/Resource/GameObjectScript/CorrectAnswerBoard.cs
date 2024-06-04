using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorrectAnswerBoard : MonoBehaviour
{
    public CorrectAnswerBoardState currentState = CorrectAnswerBoardState.Stay;
    public float mainAlpha = 0f;
    public float fadeSpeed = 6f;

    private void Awake()
    {
        SetAlpha();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case CorrectAnswerBoardState.Stay:
                break;
            case CorrectAnswerBoardState.Fading:
                if (mainAlpha <= 0f)
                {
                    currentState = CorrectAnswerBoardState.Stay;
                    break;
                }    
                mainAlpha -= fadeSpeed * Time.deltaTime;
                mainAlpha = Mathf.Max(mainAlpha, 0f);
                SetAlpha();
                break;
            case CorrectAnswerBoardState.Showing:
                if (mainAlpha >= 1f)
                {
                    currentState = CorrectAnswerBoardState.Stay;
                    break;
                }
                mainAlpha += fadeSpeed * Time.deltaTime;
                mainAlpha = Mathf.Min(mainAlpha, 1f);
                SetAlpha();
                break;
        }
        
    }

    public void Show(string correctAnswerText)
    {
        Text correctAnswer = transform.GetChild(1).GetComponent<Text>();
        correctAnswer.text = correctAnswerText.ToUpper();
        currentState = CorrectAnswerBoardState.Showing;
    }

    public void Hide()
    {
        currentState = CorrectAnswerBoardState.Fading;
    }

    void SetAlpha()
    {
        Image background = GetComponent<Image>();
        Text description = transform.GetChild(0).GetComponent<Text>();
        Text correctAnswer = transform.GetChild(1).GetComponent<Text>();

        var tempColor = background.color;
        tempColor.a = mainAlpha;
        background.color = tempColor;
        description.color = tempColor;
        correctAnswer.color = tempColor;
    }

    public enum CorrectAnswerBoardState
    {
        Fading,
        Showing,
        Stay,
    }
}

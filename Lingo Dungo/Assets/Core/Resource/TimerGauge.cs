using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerGauge : MonoBehaviour
{
    private Image bar;
    private int cValue;
    public bool stop = false;

    public int maxValue;

    private int CurrentValue
    {
        get { return cValue; }
        set
        {
            if (value > maxValue)
            {
                cValue = maxValue;
            }
            else if (value < 0)
            {
                cValue = 0;
            }

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        bar = transform.GetChild(0).gameObject.GetComponent<Image>();
        startTimer();
        StartCoroutine(TimeTickRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        bar.fillAmount = CurrentValue * 1f / maxValue;
    }

    IEnumerator TimeTickRoutine()
    {
        while (true)
        {
            if (stop)
            {
                yield return new WaitForSeconds(0.01f);
            }
            else
            {
                if (cValue > 0)
                {
                    cValue--;
                    yield return new WaitForSeconds(0.01f);
                }
                else
                {
                    stopTimer();
                }
            }

        }
    }

    #region Control
    public void startTimer()
    {
        cValue = maxValue;
        stop = false;
    }
    
    public void stopTimer()
    {
        stop = true;
    }

    #endregion
}

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

    public int CurrentValue
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
            else
            {
                cValue = value;
            }

        }
    }

    public float TimeRate { get { return cValue * 1f / maxValue; } }

    // Start is called before the first frame update
    void Start()
    {
        bar = transform.GetChild(0).gameObject.GetComponent<Image>();
        stop = true;
        StartCoroutine(TimeTickRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        bar.fillAmount = TimeRate;
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
                    StopTimer();
                }
            }

        }
    }

    #region Control
    public void StartTimer()
    {
        cValue = maxValue;
        stop = false;
    }
    
    public void StopTimer()
    {
        stop = true;
    }

    #endregion
}

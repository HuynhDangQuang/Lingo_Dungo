using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceEffect : MonoBehaviour
{
    public Image image;
    private float mainAlpha;
    public Text descriptionText;
    public string description = "";

    public bool canStartCoroutine = true;
    public bool CanStartCoroutine
    {
        get { return canStartCoroutine; }
    }

    // Start is called before the first frame update
    void Start()
    {
        mainAlpha = 1f;
        canStartCoroutine = true;
    }

    // Update is called once per frame
    void Update()
    {
        descriptionText.text = description;
        var tempColor = image.color;
        tempColor.a = mainAlpha;
        image.color = tempColor;

        tempColor = descriptionText.color;
        tempColor.a = mainAlpha;
        descriptionText.color = tempColor;
    }

    public void InstanceFadeOut()
    {
        mainAlpha = 1f;
    }

    public void InstanceFadeIn()
    {
        mainAlpha = 0f;
        description = "";
    }

    public void CombatStartFade()
    {
        if (canStartCoroutine)
        {
            canStartCoroutine = false;
            StartCoroutine(DoCombatFlashRoutine());
        }
    }

    public void FadeIn(float duration, string text = "")
    {
        if (canStartCoroutine)
        {
            canStartCoroutine = false;
            if (text != "")
            {
                description = text;
            }
            StartCoroutine(DoFadeInRountine(duration));
        }
    }

    public void FadeOut(float duration, string text = "")
    {
        if (canStartCoroutine)
        {
            canStartCoroutine = false;
            if (text != "")
            {
                description = text;
            }
            StartCoroutine(DoFadeOutRountine(duration));
        }
    }

    IEnumerator DoCombatFlashRoutine()
    {
        InstanceFadeOut();
        yield return new WaitForSeconds(0.05f);
        InstanceFadeIn();
        yield return new WaitForSeconds(0.05f);
        InstanceFadeOut();
        yield return new WaitForSeconds(0.05f);
        InstanceFadeIn();
        yield return new WaitForSeconds(0.05f);
        InstanceFadeOut();
        yield return new WaitForSeconds(0.05f);
        InstanceFadeIn();
        yield return new WaitForSeconds(0.05f);
        InstanceFadeOut();
        yield return new WaitForSeconds(0.05f);
        InstanceFadeIn();
        yield return new WaitForSeconds(0.05f);
        InstanceFadeOut();
        yield return new WaitForSeconds(0.05f);
        InstanceFadeIn();
        yield return new WaitForSeconds(0.05f);

        while (mainAlpha < 1f)
        {
            mainAlpha += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        mainAlpha = 1f;

        canStartCoroutine = true;
        yield return null;

    }

    IEnumerator DoFadeInRountine(float duration)
    {
        mainAlpha = 1f;
        while (mainAlpha > 0)
        {
            mainAlpha -= 0.01f;
            yield return new WaitForSeconds(duration / 100f);
        }
        mainAlpha = 0f;

        description = "";
        canStartCoroutine = true;
        yield return null;
    }

    IEnumerator DoFadeOutRountine(float duration)
    {
        mainAlpha = 0f;
        while (mainAlpha < 1)
        {
            mainAlpha += 0.01f;
            yield return new WaitForSeconds(duration / 100f);
        }
        mainAlpha = 1f;

        canStartCoroutine = true;
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.UI;

public class AnswerPanel : MonoBehaviour
{
    public const int TextSize = 40;
    public GameObject wordSheet;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearWord()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void SendCharToWordSheet(GameObject g)
    {
        wordSheet.GetComponent<WordPanel>().AddCharFromAnswerSheet(g);
    }
}

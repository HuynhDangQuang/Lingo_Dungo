using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject template = transform.GetChild(0).gameObject;
        GameObject g;

        for (int i = 0; i < 5; i++)
        {
            g = Instantiate(template, transform);
            g.transform.GetChild(0).GetComponent<Text>().text = i.ToString();
        }

        // Delete Button template
        Destroy(template);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

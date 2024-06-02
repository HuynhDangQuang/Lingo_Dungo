using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealSprite : MonoBehaviour
{
    //private Vector3 startPosition;
    private float v0;
    private float g;
    private float minV;
    private float disappearTimer;
    private Color textColor;

    void Awake()
    {
        //startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        v0 = v0 > minV ? v0 + g * Time.deltaTime : minV;
        transform.position += new Vector3(0, v0) * Time.deltaTime;
        disappearTimer -= Time.deltaTime;

        if (disappearTimer < 0)
        {
            TextMeshPro text = GetComponent<TextMeshPro>();
            float disappearSpeed = 7f;
            text.alpha -= disappearSpeed * Time.deltaTime;
            if (text.alpha < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Setup(int damage, bool critical)
    {
        TextMeshPro text = GetComponent<TextMeshPro>();
        text.SetText(critical ? "+ " + damage.ToString() + "!!!" : "+ " + damage.ToString());
        disappearTimer = 1f;
        if (transform.eulerAngles.y == 180f)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        text.fontSize = 8f + Mathf.Clamp(damage / 2f, 10f, 100f) / 100f * 8.0f * (critical ? 1.4f : 1);
        v0 = 6.0f;
        g = -8f;
        minV = 0.5f;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelStatGauge : MonoBehaviour
{
    public GameObject Model;
    public GaugeType Type;

    private Vector3 saveX;

    // Start is called before the first frame update
    void Start()
    {
        if (Model.GetComponent<Transform>().eulerAngles.y == 180)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            RectTransform rectTransform = transform.GetComponent<RectTransform>();
            Vector2 oldPosition = rectTransform.anchoredPosition;
            rectTransform.anchoredPosition = new Vector2(-oldPosition.x, oldPosition.y);
        }
        //saveX = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        Combatant owner = Model.GetComponent<Model>().owner;
        if (owner == null)
        {
            return;
        }

        switch (Type)
        {
            case GaugeType.HP:
                {
                    int shield = owner.HP + owner.Barrier;
                    int realMaxAmount = Mathf.Max(owner.MaxHP, shield);
                    float hpRate = owner.HP * 1f / realMaxAmount;
                    float shieldRate = shield * 1f / realMaxAmount;
                    transform.GetChild(0).GetComponent<Image>().fillAmount = shieldRate;
                    transform.GetChild(1).GetComponent<Image>().fillAmount = hpRate;
                    transform.GetChild(2).GetComponent<Text>().text = owner.HP.ToString() + " / " + owner.MaxHP.ToString();
                    break;
                }
            case GaugeType.MP:
                {
                    float mpRate = owner.MP * 1f / owner.MaxMP;
                    transform.GetChild(0).GetComponent<Image>().fillAmount = mpRate;
                    transform.GetChild(1).GetComponent<Text>().text = owner.MP.ToString() + owner.MaxMP.ToString();
                    break;
                }
        }
    }

    public enum GaugeType
    {
        HP,
        MP
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatGauge : MonoBehaviour
{
    public GameObject CombatManager;
    public GaugeType Type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CombatManager manager = CombatManager.GetComponent<CombatManager>();
        switch (Type)
        {
            case GaugeType.HP:
                {
                    float hpRate = manager.ThisPlayer.HP * 1f / manager.ThisPlayer.MaxHP;
                    transform.GetChild(1).GetComponent<Image>().fillAmount = hpRate;
                    transform.GetChild(2).GetComponent<Text>().text = manager.ThisPlayer.HP.ToString();
                    transform.GetChild(3).GetComponent<Text>().text = manager.ThisPlayer.MaxHP.ToString();
                    break;
                }
            case GaugeType.MP:
                {
                    float mpRate = manager.ThisPlayer.MP * 1f / manager.ThisPlayer.MaxMP;
                    transform.GetChild(1).GetComponent<Image>().fillAmount = mpRate;
                    transform.GetChild(2).GetComponent<Text>().text = manager.ThisPlayer.MP.ToString();
                    transform.GetChild(3).GetComponent<Text>().text = manager.ThisPlayer.MaxMP.ToString();
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

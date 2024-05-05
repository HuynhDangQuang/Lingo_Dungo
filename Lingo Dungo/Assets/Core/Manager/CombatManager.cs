using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public List<GameObject> allies = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();



    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i <= 4; i++)
        {
            allies.Add(GameObject.FindGameObjectWithTag("Actor" + i));
        }
        StartCoroutine(DoDemoRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator DoDemoRoutine()
    {
        Model actor1 = allies[0].GetComponent<Model>();
        while (true)
        {
            actor1.PlayAttack();
            yield return new WaitForSeconds(2f);
        }
    }
}

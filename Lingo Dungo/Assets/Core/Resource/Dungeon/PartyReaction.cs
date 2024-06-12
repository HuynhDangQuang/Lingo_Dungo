using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyReaction : MonoBehaviour
{
    [Header("Animations")]
    public GameObject exclamation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayExclamation()
    {
        Instantiate(exclamation, transform);
    }
}

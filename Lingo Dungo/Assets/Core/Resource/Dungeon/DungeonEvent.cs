using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static MapCell;

public class DungeonEvent : MonoBehaviour
{
    [Serializable]
    public struct EventSprite
    {
        public string name;
        public Sprite sprite;
    }

    public Image image;
    public List<EventSprite> eventSprites;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetupEvent(string eventId)
    {
        EventSprite? src = eventSprites.Find(x => x.name == eventId);
        if (src.Value.name != null)
        {
            image.sprite = src.Value.sprite;
        }
        else
        {
            image.sprite = eventSprites.First().sprite;
        }
    }
}

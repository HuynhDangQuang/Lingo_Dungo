using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PresetModelManager;

public class MapCell : MonoBehaviour
{
    [Serializable]
    public struct RoomIcon
    {
        public string id;
        public Sprite icon;
    }

    #region Sprites
    [Header("Sprites")]
    public Sprite cellUp;
    public Sprite cellDown;
    public Sprite cellLeft;
    public Sprite cellRight;
    public Sprite cellUpLeft;
    public Sprite cellUpRight;
    public Sprite cellDownLeft;
    public Sprite cellDownRight;
    public Sprite cellUpT;
    public Sprite cellDownT;
    public Sprite cellLeftT;
    public Sprite cellRightT;
    public Sprite cellUpDown;
    public Sprite cellLeftRight;
    public Sprite cellAll;
    public Sprite cellEmpty;
    public Sprite cellUnknown;
    #endregion

    [Header("Room Icon")]
    public Image icon;
    public List<RoomIcon> roomIcons;

    public bool up = false;
    public bool down = false;
    public bool left = false;
    public bool right = false;
    public bool unknown = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIcon(string id)
    {
        RoomIcon? src = roomIcons.Find(x => x.id == id);
        if (src.Value.id != null)
        {
            icon.sprite = src.Value.icon;
        }
        else
        {
            icon.sprite = roomIcons[0].icon;
        }
    }

    public void Setup(bool up, bool down, bool left, bool right, bool unknown = false)
    {
        this.up = up;
        this.down = down;
        this.left = left;
        this.right = right;
        this.unknown = unknown;
        SetIcon("empty");

        Image image = GetComponent<Image>();
        if (unknown)
        {
            image.sprite = cellUnknown;
        }
        else if (up && !down && !left && !right)
        {
            image.sprite = cellUp;
        }
        else if (!up & down && !left && !right)
        {
            image.sprite = cellDown;
        }
        else if (!up & !down && left && !right)
        {
            image.sprite = cellLeft;
        }
        else if (!up & !down && !left && right)
        {
            image.sprite = cellRight;
        }
        else if (up & !down && left && !right)
        {
            image.sprite = cellUpLeft;
        }
        else if (up & !down && !left && right)
        {
            image.sprite = cellUpRight;
        }
        else if (!up & down && left && !right)
        {
            image.sprite = cellDownLeft;
        }
        else if (!up & down && !left && right)
        {
            image.sprite = cellDownRight;
        }
        else if (up & !down && left && right)
        {
            image.sprite = cellUpT;
        }
        else if (!up & down && left && right)
        {
            image.sprite = cellDownT;
        }
        else if (up & down && left && !right)
        {
            image.sprite = cellLeftT;
        }
        else if (up & down && !left && right)
        {
            image.sprite = cellRightT;
        }
        else if (up & down && !left && !right)
        {
            image.sprite = cellUpDown;
        }
        else if (!up & !down && left && right)
        {
            image.sprite = cellLeftRight;
        }
        else if (up & down && left && right)
        {
            image.sprite = cellAll;
        }
        else
        {
            image.sprite = cellEmpty;
        }
    }
}

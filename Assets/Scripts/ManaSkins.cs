using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mana Skin", menuName = "ManaCycle/Skins")]
public class ManaSkins : ScriptableObject
{
    public ManaShape[] shapes;
}

// [System.Serializable]
// public class ManaColor
// {
//     public Color color;

//     public Color darkColor;
// }

// TODO Separate color and shape for mana pallete customization
[System.Serializable]
public class ManaShape
{
    public Color color;

    public Color darkColor;

    public Sprite shapeIcon;

    public Vector2 iconPosition;

    public float iconRotation; // on z axis

    public Vector2 iconScale;
}

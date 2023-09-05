using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class GhostTile : ManaTile
{
    [SerializeField] private SpriteRenderer iconCenterSprite;

    public override void SetColor(int c, ManaColor manaColor)
    {
        _color = c;
        sprite.color = Color.black;
        borderSprite.color = manaColor.color;
        iconSprite.sprite = manaColor.shapeIcon;
        iconSprite.color = manaColor.color;
        iconSprite.transform.localPosition = (Vector3)manaColor.iconPosition - Vector3.forward * 0.1f;
        iconSprite.transform.localEulerAngles = new Vector3(0, 0, manaColor.iconRotation);
        iconSprite.transform.localScale = manaColor.iconScale;
        iconCenterSprite.sprite = manaColor.shapeIcon;
        iconCenterSprite.transform.localPosition = (Vector3)manaColor.iconPosition - Vector3.forward * 0.2f;
        iconCenterSprite.transform.localEulerAngles = new Vector3(0, 0, manaColor.iconRotation);
        iconCenterSprite.transform.localScale = manaColor.iconScale*0.667f;
    }

    public override void Update()
    {
        // nothing; should not be lit up if this is a ghost tile
    }
}
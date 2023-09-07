using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class GhostTile : ManaTile
{
    [SerializeField] private SpriteRenderer iconCenterSprite;

    public override void SetColor(int c, ManaShape manaShape)
    {
        _color = c;
        sprite.color = Color.black;
        borderSprite.color = manaShape.color;
        iconSprite.sprite = manaShape.shapeIcon;
        iconSprite.color = manaShape.color;
        iconSprite.transform.localPosition = (Vector3)manaShape.iconPosition - Vector3.forward * 0.1f;
        iconSprite.transform.localEulerAngles = new Vector3(0, 0, manaShape.iconRotation);
        iconSprite.transform.localScale = manaShape.iconScale;
        iconCenterSprite.sprite = manaShape.shapeIcon;
        iconCenterSprite.transform.localPosition = (Vector3)manaShape.iconPosition - Vector3.forward * 0.2f;
        iconCenterSprite.transform.localEulerAngles = new Vector3(0, 0, manaShape.iconRotation);
        iconCenterSprite.transform.localScale = manaShape.iconScale*0.667f;
    }

    public override void Update()
    {
        // nothing; should not be lit up if this is a ghost tile
    }
}
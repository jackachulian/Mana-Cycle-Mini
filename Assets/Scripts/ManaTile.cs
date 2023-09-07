using System.Collections;
using UnityEngine;
public class ManaTile : MonoBehaviour
{
    // While inside of a piece, this represents offset from the center, or rotation axis, of the piece.
    // When placed on the board, this is the X and Y position on the board, or the column and row, respectively.
    [SerializeField] private Vector2Int _pos;
    public Vector2Int pos { get { return _pos; } }

    [SerializeField] protected SpriteRenderer sprite;

    [SerializeField] protected SpriteRenderer borderSprite;

    [SerializeField] protected SpriteRenderer iconSprite;

    private ManaShape manaShape;

    private bool lit;

    public void SetPosition(Vector2Int newPos)
    {
        _pos = newPos;
    }

    public void UpdatePositionOnBoard()
    {
        transform.localPosition = new Vector3(pos.x + 0.5f, pos.y + 0.5f);
    }

    // Color of this mana tile
    [SerializeField] protected int _color;
    public int color { get { return _color; } }

    public virtual void SetColor(int c, ManaShape manaShape)
    {
        this.manaShape = manaShape;
        _color = c;
        sprite.color = manaShape.color;
        borderSprite.color = manaShape.darkColor;
        iconSprite.sprite = manaShape.shapeIcon;
        iconSprite.color = manaShape.darkColor;
        iconSprite.transform.localPosition = (Vector3)manaShape.iconPosition - Vector3.forward*0.1f;
        iconSprite.transform.localEulerAngles = new Vector3(0, 0, manaShape.iconRotation);
        iconSprite.transform.localScale = manaShape.iconScale;
    }

    public virtual void Update()
    {
        if (lit)
        {
            if (manaShape == null) return;
            float lightValue = 0.25f + Mathf.PingPong(Time.time*0.75f, 0.25f);
            sprite.color = Color.Lerp(manaShape.color, Color.white, lightValue);
            borderSprite.color = Color.Lerp(manaShape.darkColor, Color.white, lightValue*0.5f);
            iconSprite.color = Color.Lerp(manaShape.darkColor, Color.white, lightValue * 0.5f);
        }
    }

    public void SetLit(bool lit)
    {
        if (this.lit && !lit)
        {
            sprite.color = manaShape.color;
            borderSprite.color = manaShape.darkColor;
            iconSprite.sprite = manaShape.shapeIcon;
            iconSprite.color = manaShape.darkColor;
        }
        this.lit = lit;
    }
}
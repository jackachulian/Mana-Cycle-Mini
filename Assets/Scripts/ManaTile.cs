using System.Collections;
using UnityEngine;
public class ManaTile : MonoBehaviour
{
    // While inside of a piece, this represents offset from the center, or rotation axis, of the piece.
    // When placed on the board, this is the X and Y position on the board, or the column and row, respectively.
    [SerializeField] private Vector2Int _pos;
    public Vector2Int pos { get { return _pos; } }

    [SerializeField] private SpriteRenderer spriteRenderer;

    public void SetPosition(Vector2Int newPos)
    {
        _pos = newPos;
    }

    public void UpdatePositionOnBoard()
    {
        transform.localPosition = new Vector3(pos.x + 0.5f, pos.y + 0.5f);
    }

    // Color of this mana tile
    [SerializeField] private int _color;
    public int color { get { return _color; } }

    public void SetColor(int c, ManaColor manaColor)
    {
        _color = c;
        spriteRenderer.color = manaColor.color;
    }
}
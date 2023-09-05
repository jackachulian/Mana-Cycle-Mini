using System.Collections;
using UnityEngine;

public class Piece : MonoBehaviour
{
    // X and Y position of this piece's center (rotation center)
    public Vector2Int pos { get; private set; }

    // Current orientation of this piece
    public Orienetation orienetation;

    // All tiles connected to this piece
    [SerializeField] private ManaTile[] _tiles;
    public ManaTile[] tiles { get { return _tiles; } }

    // Offset the piece in the board by the given X and Y values.
    // Note: does not check for overlaps with board!
    // Use MovePiece() in the Board to move the piece correctly.
    public void Move(Vector2Int offset)
    {
        pos += offset;
    }

    // Rotate without verifying that new rotation is valid.
    public void RotateCCW()
    {
        orienetation = (Orienetation)(((int)orienetation + 3) % 4);
    }

    public void RotateCW()
    {
        orienetation = (Orienetation)(((int)orienetation + 1) % 4);
    }

    // Set position without checking that position is valid.
    public void SetPosition(Vector2Int pos)
    {
        this.pos = pos;
    }

    // Convert a contained tile int position from this piece's coordinate space to tile board space using this piece's position..
    public Vector2Int PieceToBoard(Vector2Int pos)
    {
        switch(orienetation)
        {
            case Orienetation.Up:
                return pos + this.pos;
            case Orienetation.Left:
                return new Vector2Int(-pos.y, pos.x) + this.pos;
            case Orienetation.Down:
                return new Vector2Int(-pos.x, -pos.y) + this.pos;
            case Orienetation.Right:
                return new Vector2Int(pos.y, -pos.x) + this.pos;
            default:
                return this.pos;
        }
    }

    // Intended to be called after this piece moves.
    public void UpdatePositions()
    {
        foreach (ManaTile tile in _tiles)
        {
            var pos = PieceToBoard(tile.pos);
            if (tile != null) tile.transform.localPosition = new Vector3(pos.x, pos.y);
        }
    }

    public enum Orienetation
    {
        Up, // x, y
        Left, // -y, x
        Down, // -x, -y
        Right // y, -x
    }
}
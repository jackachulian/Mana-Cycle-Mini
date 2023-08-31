﻿using System.Collections;
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

    public void SetPosition(Vector2Int pos)
    {
        this.pos = pos;
    }

    // Returns true if none of this piece's tiles have the same position as anyy tile in the passed array.
    public bool IsValidPlacement(ManaTile[,] otherTiles)
    {
        foreach (ManaTile tile in _tiles)
        {
            Vector2Int boardPos = PieceToBoard(tile.pos);

            // check for tile OOB
            if (boardPos.x < 0 || boardPos.x >= otherTiles.GetLength(0) || boardPos.y < 0 || boardPos.y >= otherTiles.GetLength(1)) return false;

            // check for overlapping tile
            if (otherTiles[boardPos.x, boardPos.y] != null) return false;
        }

        return true;
    }

    // Convert a contained tile int position from this piece's coordinate space to tile board space using this piece's position..
    public Vector2Int PieceToBoard(Vector2Int pos)
    {
        switch(orienetation)
        {
            case Orienetation.Up:
                return new Vector2Int(pos.x, pos.y) + this.pos;
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
            tile.transform.localPosition = new Vector3(pos.x, pos.y);
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
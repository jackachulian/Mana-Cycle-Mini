using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private IBoardController _controller;
    public IBoardController controller { get { return _controller; } }

    // Tile dimensions of this board.
    private int width = 8;
    private int height = 16;

    // The grid of tiles on this board. Coordinates represent [X, Y] position on the board.
    private Tile[,] tiles;

    // Piece this board is currently dropping
    private Piece piece;

    void Awake()
    {
        tiles = new Tile[width, height];
    }

    void Update()
    {

    }

    // Attempt to move the current piece.
    // Returns true if the piece was successfully moved to the new position.
    // False if a tile is blocking the piece from entering this position.
    public bool MovePiece(Vector2Int offset)
    {
        piece.Move(offset);
        bool valid = piece.IsValidPlacement(tiles);
        if (!valid)
        {
            piece.Move(-offset);
        }

        piece.UpdatePositions();
        return valid;
    }
    
    public bool MovePiece(int x, int y)
    {
        return MovePiece(new Vector2Int(x, y));
    }

    /// <summary>
    /// Place the piece that is currently falling on this board, destroy the piece container object, and spawn the next piece.
    /// </summary>
    public void PlacePiece()
    {
        foreach (Tile tile in piece.tiles)
        {
            var pos = piece.PieceToBoard(tile.pos);
            tile.transform.SetParent(transform);
            tile.transform.localPosition = new Vector3(pos.x, pos.y);
        }
        Destroy(piece.gameObject);
    }
}
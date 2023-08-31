using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject piecePrefab;

    [SerializeField] private Transform manaTileGridTransform;

    // Tile dimensions of this board.
    private int width = 8;
    private int height = 20;

    private static Vector2Int pieceSpawnPos = new Vector2Int(3, 15);

    public PieceMovement pieceMovement { get; private set; }

    // The grid of tiles on this board. Coordinates represent [X, Y] position on the board.
    private ManaTile[,] tiles;

    // Piece this board is currently dropping
    private Piece piece;

    private void Reset()
    {
        pieceMovement = GetComponent<PieceMovement>();
        if (!pieceMovement) pieceMovement = new PieceMovement();
    }

    void Awake()
    {
        tiles = new ManaTile[width, height];
        pieceMovement = GetComponent<PieceMovement>();
    }

    void Start()
    {
        SpawnNextPiece();
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
        foreach (ManaTile tile in piece.tiles)
        {
            var pos = piece.PieceToBoard(tile.pos);

            tile.transform.SetParent(manaTileGridTransform);
            tile.transform.localPosition = new Vector3(pos.x+0.5f, pos.y+0.5f);

            tiles[pos.x, pos.y] = tile;
        }
        Destroy(piece.gameObject);

        SpawnNextPiece();
    }

    public void SpawnNextPiece()
    {
        GameObject pieceObject = Instantiate(piecePrefab, manaTileGridTransform);
        piece = pieceObject.GetComponent<Piece>();
        piece.SetPosition(pieceSpawnPos);
        piece.UpdatePositions();
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject pointerPrefab;

    private GameObject pointer;

    [SerializeField] private Vector3 pointerOffset = new Vector3(-2.25f, 0, 0);

    [SerializeField] private Transform manaTileGridTransform; [SerializeField] private TMP_Text scoreText;

    // Tile dimensions of this board.
    public int width { get; private set; } = 8;
    public int height { get; private set; } = 20;

    private static Vector2Int pieceSpawnPos = new Vector2Int(3, 15);

    public PieceFalling pieceMovement { get; private set; }
    public Spellcasting spellcasting { get; private set; }

    public PieceQueue pieceQueue { get; private set; }

    public BoardUI ui { get; private set; }

    public ManaCycle cycle { get; private set; }

    // The grid of tiles on this board. Coordinates represent [X, Y] position on the board.
    private ManaTile[,] tiles;

    // Piece this board is currently dropping
    private Piece piece;

    // Amount of points currently earned. May be used for HP in pvp mode once implemented
    private int score;

    // When false, will not spawn a new piece or perform piece falling, used for when game has not started or has ended
    public bool active { get; private set; } = false;

    // When true, player has paused the game, active is false temporarily.
    public bool paused { get; private set; }

    void Awake()
    {
        tiles = new ManaTile[width, height];

        pieceMovement = GetComponent<PieceFalling>();
        spellcasting = GetComponent<Spellcasting>();
        pieceQueue = GetComponent<PieceQueue>();
        ui = GetComponent<BoardUI>();

        cycle = FindObjectOfType<ManaCycle>();
    }

    private void Start()
    {
        // UI initialization
        ui.SetScore(score);
    }

    public void InitializeAfterCycle()
    {
        active = true;

        pointer = Instantiate(pointerPrefab, transform);
        StartCoroutine(RepositionPointerNextFrame());

        pieceQueue.InitializeAfterCycle();
        SpawnNextPiece();

    }

    IEnumerator RepositionPointerNextFrame()
    {
        yield return new WaitForEndOfFrame();
        RepositionPointer();
    }

    public void RepositionPointer()
    {
        Transform cycleColorTransform = cycle.cycleColorObjects[spellcasting.cyclePosition].transform;
        pointer.transform.position = cycleColorTransform.position + pointerOffset;
    }

    public ManaTile GetTile(int x, int y)
    {
        return tiles[x, y];
    }

    public int GetTileColor(int x, int y)
    {
        ManaTile tile = GetTile(x, y);
        if (!tile) return -1;
        return tile.color;
    }

    public void ClearTile(int x, int y)
    {
        ManaTile tile = tiles[x, y];
        if (!tile) return;
        tiles[x, y] = null;
        Destroy(tile.gameObject);
    }

    // Add the amount of points earned to total.
    // In PVP matches once implemented, this will deal damage instead of adding to points.
    public void ScorePoints(int points)
    {
        score += points;
        ui.SetScore(score);
    }

    // Returns true if none of the current piece's tiles have the same position as any tile on the board.
    public bool IsValidPlacement()
    {
        foreach (ManaTile tile in piece.tiles)
        {
            Vector2Int boardPos = piece.PieceToBoard(tile.pos);

            // check for tile OOB
            if (boardPos.x < 0 || boardPos.x >= width || boardPos.y < 0 || boardPos.y >= height) return false;

            // check for overlapping tile
            if (tiles[boardPos.x, boardPos.y] != null) return false;
        }

        return true;
    }

    // Attempt to move the current piece.
    // Returns true if the piece was successfully moved to the new position.
    // False if a tile is blocking the piece from entering this position.
    public bool MovePiece(Vector2Int offset)
    {
        piece.Move(offset);
        if (!IsValidPlacement())
        {
            piece.Move(-offset);
            return false;
        }

        piece.UpdatePositions();
        if (offset.y == 0) SoundManager.Instance.PlaySound(SoundManager.sfx.move);
        return true;
    }
    
    public bool MovePiece(int x, int y)
    {
        return MovePiece(new Vector2Int(x, y));
    }

    public void RotatePieceCCW()
    {
        piece.RotateCCW();

        bool valid = IsValidPlacement();

        if (!valid) valid = MovePiece(-1, 0);
        if (!valid) valid = MovePiece(1, 0);
        if (!valid) valid = MovePiece(0, 1);

        if (!valid) piece.RotateCW();
        else
        {
            piece.UpdatePositions();
            SoundManager.Instance.PlaySound(SoundManager.sfx.rotate);
        }
    }

    public void RotatePieceCW()
    {
        piece.RotateCW();

        bool valid = IsValidPlacement();

        if (!valid) valid = MovePiece(1, 0);
        if (!valid) valid = MovePiece(-1, 0);
        if (!valid) valid = MovePiece(0, 1);

        if (!valid) piece.RotateCCW();
        else 
        {
            piece.UpdatePositions();
            SoundManager.Instance.PlaySound(SoundManager.sfx.rotate);
        }
    }

    public void Spellcast()
    {
        spellcasting.Spellcast();
    }

    /// <summary>
    /// Place the piece that is currently falling on this board, destroy the piece container object, and spawn the next piece.
    /// </summary>
    public void PlacePiece()
    {
        // Convert from piece space to board space
        foreach (ManaTile tile in piece.tiles)
        {
            tile.SetPosition(piece.PieceToBoard(tile.pos));
            tile.transform.SetParent(manaTileGridTransform);
        }

        // Sort tiles by height
        Array.Sort(piece.tiles, CompareHeight);

        // Drop and place tiles one by one.
        // since the tiles are sorted lowest to highest, higher tiles should fall on lower tiles
        foreach (ManaTile tile in piece.tiles)
        {
            TileGravity(tile);
            tile.UpdatePositionOnBoard();
        }

        // Destroy piece container and spawn the next one
        Destroy(piece.gameObject);
        SoundManager.Instance.PlaySound(SoundManager.sfx.land);
        SpawnNextPiece();
    }

    public void SpawnNextPiece()
    {
        piece = pieceQueue.GetNextPiece();
        piece.transform.SetParent(manaTileGridTransform, false);
        piece.transform.localPosition = new Vector3(0.5f, 0.5f, -0.5f);
        piece.SetPosition(pieceSpawnPos);

        // If the new piece is blocked by any tiles on the board,
        // player has topped out
        if (!IsValidPlacement())
        {
            Die();
            return;
        }

        piece.UpdatePositions();
    }

    // Perform gravity for a single tile.
    public void TileGravity(ManaTile tile)
    {
        int y = tile.pos.y;

        while (y > 0)
        {
            if (tiles[tile.pos.x, y-1] != null)
            {
                break;
            }
            y--;
        }

        tiles[tile.pos.x, tile.pos.y] = null;
        tiles[tile.pos.x, y] = tile;
        tile.SetPosition(new Vector2Int(tile.pos.x, y));
    }

    // Perform gravity for the entie board.
    public void AllTileGravity()
    {
        for (int x = 0; x < width; x++)
        {
            ColumnGravity(x);
        }
    }

    // Performgravity for a single column.
    public void ColumnGravity(int x)
    {
        // Keep track of the total gap size, to know where to fill in fallen tiles to.
        int gapSize = 0;

        for (int y = 0; y < height; y++)
        {
            // Starting from the bottom, add to gap size if empty,
            // or bring down by total gap size if there is a tile here.
            ManaTile tile = tiles[x, y];
            if (tile == null)
            {
                gapSize++;
            } else
            {
                if (gapSize > 0)
                {
                    tiles[x, y - gapSize] = tile;
                    tiles[x, y] = null;
                    tile.SetPosition(new Vector2Int(x, y - gapSize));
                    tile.UpdatePositionOnBoard();
                }
            }
        }
    }

    // return the difference between the two tile's heights (t1 - t2).
    public static int CompareHeight(ManaTile t1, ManaTile t2)
    {
        return t1.pos.y - t2.pos.y;
    }

    public void Die()
    {
        active = false;
        Destroy(piece.gameObject);
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlaySound(SoundManager.sfx.lose);
        ui.OnDeath();
    }

    public void TogglePause()
    {
        paused = !paused;
        active = !paused;
        Time.timeScale = paused ? 0 : 1;
        ui.TogglePause();
    }
}
using System;
using System.Collections;
using UnityEngine;

public class GhostPiece : MonoBehaviour
{
    [SerializeField] private GhostTile ghostTilePrefab;

    private Board board;
    private void Awake()
    {
        board = GetComponent<Board>();
    }

    private ManaTile[,] ghostBoard;
    private GhostTile[] ghostTiles;

    public void RecalculateGhostPiece()
    {
        if (!gameObject.activeInHierarchy) return;

        DestroyGhostTiles();

        // Refresh ghost mana tile board
        ghostBoard = board.tiles.Clone() as ManaTile[,];
        ghostTiles = new GhostTile[board.piece.tiles.Length];

        // Unlight all lit tiles
        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                var tile = board.tiles[x, y];
                if (tile) tile.SetLit(false);
            }
        }

        // Drop each tile from current piece as a ghost tile
        Array.Sort(board.piece.tiles, board.CompareHeight);
        for (int i = 0; i < board.piece.tiles.Length; i++)
        {
            ghostTiles[i] = AddGhostTile(board.piece.tiles[i]);
            ghostTiles[i].UpdatePositionOnBoard();
        }

        // Light up all tiles connected to ghost tiles
        foreach (ManaTile manaTile in ghostTiles)
        {
            board.spellcasting.ResetCheckArrays();
            board.spellcasting.SearchForConnected(manaTile.pos.x, manaTile.pos.y, manaTile.color, 0, ghostBoard, true);
        }
    }

    public void DestroyGhostTiles()
    {
        // Destroy existing ghost tiles
        if (ghostTiles != null)
        {
            foreach (ManaTile ghostTile in ghostTiles)
            {
                Destroy(ghostTile.gameObject);
            }
        }
    }

    // Drop a ghost tile on the "ghost board".
    GhostTile AddGhostTile(ManaTile realTile)
    {
        GhostTile ghostTile = Instantiate(ghostTilePrefab, board.manaTileGridTransform);
        ghostTile.SetColor(realTile.color, board.cycle.GetManaColor(realTile.color));

        var pos = board.piece.PieceToBoard(realTile.pos);
        int y = pos.y;

        while (y > 0)
        {
            if (ghostBoard[pos.x, y - 1] != null)
            {
                break;
            }
            y--;
        }

        ghostBoard[pos.x, y] = ghostTile;
        ghostTile.SetPosition(new Vector2Int(pos.x, y));

        return ghostTile;
    }
}
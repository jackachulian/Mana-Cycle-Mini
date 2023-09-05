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
        // Destroy existing ghost tiles
        foreach (ManaTile ghostTile in ghostTiles)
        {
            Destroy(ghostTile.gameObject);
        }

        // Refresh ghost mana tile board
        ghostBoard = board.tiles.Clone() as ManaTile[,];
        ghostTiles = new GhostTile[board.piece.tiles.Length];

        // Drop each tile from current piece as a ghost tile
        for (int i = 0; i < board.piece.tiles.Length; i++)
        {
            ghostTiles[i] = AddGhostTile(board.piece.tiles[i]);
            ghostTiles[i].UpdatePositionOnBoard();
        }
    }

    // Drop a ghost tile on the "ghost board".
    GhostTile AddGhostTile(ManaTile realTile)
    {
        GhostTile ghostTile = Instantiate(ghostTilePrefab, board.manaTileGridTransform);
        ghostTile.SetColor(realTile.color, board.cycle.GetManaColor(realTile.color));

        int x = board.piece.pos.x + realTile.pos.x;
        int y = board.piece.pos.y + realTile.pos.y;

        while (y > 0)
        {
            if (ghostBoard[x, y - 1] != null)
            {
                break;
            }
            y--;
        }

        ghostBoard[x, y] = ghostTile;
        ghostTile.SetPosition(new Vector2Int(x, y));

        return ghostTile;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the spellcasting and blob mana clearing on a board.
public class Spellcasting : MonoBehaviour
{
    // The position in the cycle the player is in.
    public int cyclePosition { get; private set; }

    // All tiles that have been proven by the blob algoritm as either clearable or not clearable.
    // Once all of these are true, the state of all tile's clear/unclearability has been found, and searching may end.
    bool[,] checkedManaTiles;

    // All tiles of the current color that will be cleared in the next spellcast.
    bool[,] clearableManaTiles;
    int clearableCount;

    public readonly int minimumBlobSize = 3;

    public Board board { get; private set; }
    private void Awake()
    {
        board = GetComponent<Board>();

        checkedManaTiles = new bool[board.width, board.height];
        clearableManaTiles = new bool[board.width, board.height];
    }
    
    // The currnet mana color the player must clear.
    private int CurrentManaColor()
    {
        return board.cycle.sequence[cyclePosition];
    }

    // Build a list of all discovered blobs of the current color.
    private void CheckConnectedTiles()
    {
        System.Array.Clear(checkedManaTiles, 0, checkedManaTiles.Length);
        System.Array.Clear(clearableManaTiles, 0, clearableManaTiles.Length);
        clearableCount = 0;

        int clearColor = CurrentManaColor();

        // Check all tiles for connection
        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                SearchForConnected(x, y, clearColor, 0);
            }
        }
    }

    public void Spellcast()
    {
        CheckConnectedTiles();

        if (clearableCount == 0) return;

        // Clear all connected tiles
        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                if (clearableManaTiles[x, y]) board.ClearTile(x, y);
            }
        }

        board.AllTileGravity();

        cyclePosition++;
        if (cyclePosition >= board.cycle.cycleLength)
        {
            cyclePosition = 0;
            // TODO: cycle boost
        }

        board.RepositionPointer();
    }

    /// <summary>
    /// For this tile, discover all connected blobs of the correct color and add them to the clearable mana tiles array.
    /// </summary>
    /// <param name="x">tile x pos</param>
    /// <param name="y">tile y pos</param>
    /// <param name="color">Color to search for</param>
    /// <param name="count">The amount of tiles with this color already connected in the current "blob"</param>
    /// <returns></returns>
    private int SearchForConnected(int x, int y, int color, int count)
    {
        // Don't check tile if outside bounds
        if (x < 0 || x >= board.width || y < 0 || y >= board.height) return count;

        // Don't add or check around a tile that was already checked
        if (checkedManaTiles[x, y] == true) return count;

        // Mark as checked, so no future checks will try to check this tile.
        checkedManaTiles[x, y] = true;

        // Don't add if incorrect color
        int tileColor = board.GetTileColor(x, y);
        if (color != tileColor) return count;

        // Add 1 to total blob size for this tile
        count++;

        // Check around all adjacent tiles
        count = SearchForConnected(x - 1, y, color, count);
        count = SearchForConnected(x + 1, y, color, count);
        count = SearchForConnected(x, y - 1, color, count);
        count = SearchForConnected(x, y + 1, color, count);

        // After incrementing count, check if this tile or amount connected exceeds the minimum
        // If so, this should be connected to at least minBlobSize-1 other tiles,
        // so it is clearable
        if (count >= minimumBlobSize)
        {
            clearableManaTiles[x, y] = true;
            clearableCount++;
        }

        // finally, return count so that previous iterations can know how many tiles were connected
        // to one of the four adjacent tiles that was checked
        return count;
    }

    // 
}
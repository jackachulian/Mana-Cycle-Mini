using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the spellcasting and blob mana clearing on a board.
public class Spellcasting : MonoBehaviour
{
    [SerializeField] private ChainPopup chainPopup, cascadePopup;

    // The position in the cycle the player is in.
    public int cyclePosition { get; private set; }

    // All tiles that have been proven by the blob algoritm as either clearable or not clearable.
    // Once all of these are true, the state of all tile's clear/unclearability has been found, and searching may end.
    bool[,] checkedManaTiles;

    // All tiles of the current color that will be cleared in the next spellcast.
    bool[,] clearableManaTiles;
    int clearableCount;

    public readonly int minimumBlobSize = 3;

    // If a spellcast is currently happening. Only one spellcast can happen at once.
    public bool spellcasting { get; private set; } = false;

    // Timer before checking for cascade.
    private float spellcastTimer;

    // True if timer will check for cascade after finishing;
    // false if cascade already checked and no casdade and will now check for next color.
    private bool checkingCascade;

    // Delay between color clear and checking for cascade of the same color.
    private readonly float cascadeDelay = 0.5f;
    // Delay AFTER checking cascade before checking the next color in the cycle for a combo.
    private readonly float nextColorDelay = 0.75f;

    private int chain = 1;
    private int cascade = 1;

    public Board board { get; private set; }
    private void Awake()
    {
        board = GetComponent<Board>();

        checkedManaTiles = new bool[board.width, board.height];
        clearableManaTiles = new bool[board.width, board.height];
    }

    private void Update()
    {
        if (!board.active) spellcasting = false;

        if (spellcasting)
        {
            spellcastTimer += Time.deltaTime;
            float delay = checkingCascade ? cascadeDelay : nextColorDelay;
            if (spellcastTimer >= delay)
            {
                spellcastTimer = 0;

                int tilesCleared = ClearCurrentColor();
                if (tilesCleared > 0)
                {
                    if (!checkingCascade && chain > 1)  chainPopup.Popup(chain);

                    // Add points based on this formula
                    float points = tilesCleared * 10f * (0.5f + 0.5f*chain) * Mathf.Pow(2, cascade-1);
                    board.ScorePoints((int)points);
                    SoundManager.Instance.PlaySound(SoundManager.sfx.cast, pitch: 1f + 0.122f * chain);

                    // If any tiles were cleared, check current color again to see if a cascade occured
                    CheckConnectedTiles(CurrentManaColor());
                    if (clearableCount > 0)
                    {
                        // If yes, next timer tick will check and clear current color
                        if (cascade > 1) cascadePopup.Popup(cascade);
                        cascade++;
                        checkingCascade = true;
                    } else
                    {
                        // If not, advance to the next color to check in the next timer tick
                        cascade = 1;
                        chain++;
                        AdvanceCycle();
                    }
                } else 
                {
                    if (checkingCascade)
                    {
                        cascade = 1;
                        // If the current color cannot be cascaded off, advance to next color to check next
                        chain++;
                        AdvanceCycle();
                        checkingCascade = false;
                    } else
                    {
                        // If not cascading and next color in combo cannot be cleared, the spellcast ends
                        spellcasting = false;
                        chain = 1;
                        cascade = 1;
                    }
                }
            }
        }
    }

    // The currnet mana color the player must clear.
    private int CurrentManaColor()
    {
        return board.cycle.sequence[cyclePosition];
    }

    // Build a list of all discovered blobs of the current color.
    private void CheckConnectedTiles(int clearColor)
    {
        System.Array.Clear(checkedManaTiles, 0, checkedManaTiles.Length);
        System.Array.Clear(clearableManaTiles, 0, clearableManaTiles.Length);

        clearableCount = 0;

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
        // don't start a spellcast if already spellcasting
        if (spellcasting) return;

        // if no connected of current color, don't start a spellcast
        CheckConnectedTiles(CurrentManaColor());
        if (clearableCount == 0) return;

        spellcasting = true;
        checkingCascade = false;
        spellcastTimer = 0;
        SoundManager.Instance.PlaySound(SoundManager.sfx.castStartup);
    }

    // Clear all tiles of the given color.
    // Return the amount of tiles cleared.
    private int ClearColor(int color)
    {
        CheckConnectedTiles(color);
        if (clearableCount == 0) return 0;
        ClearConnected();
        return clearableCount;
    }

    private int ClearCurrentColor()
    {
        return ClearColor(CurrentManaColor());
    }

    private void ClearConnected()
    {
        // Clear all connected tiles
        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                if (clearableManaTiles[x, y]) board.ClearTile(x, y);
            }
        }

        board.AllTileGravity();
    }

    private void AdvanceCycle()
    {
        cyclePosition++;
        if (cyclePosition >= board.cycle.cycleLength)
        {
            cyclePosition = 0;
            // TODO: cycle boost if there's enough time to implement
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
using System.Collections;
using UnityEngine;
public class Tile : MonoBehaviour
{
    // While inside of a piece, this represents offset from the center, or rotation axis, of the piece.
    // When placed on the board, this is the X and Y position on the board, or the column and row, respectively.
    public Vector2Int pos { get; private set; }
}
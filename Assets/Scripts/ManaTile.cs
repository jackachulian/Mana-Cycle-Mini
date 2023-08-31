﻿using System.Collections;
using UnityEngine;
public class ManaTile : MonoBehaviour
{
    // While inside of a piece, this represents offset from the center, or rotation axis, of the piece.
    // When placed on the board, this is the X and Y position on the board, or the column and row, respectively.
    [SerializeField] private Vector2Int _pos;
    public Vector2Int pos { get { return _pos; } }

    // Color of this mana tile
    [SerializeField] private int _color;
    public int color { get { return _color; } }
}
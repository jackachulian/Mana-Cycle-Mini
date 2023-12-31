﻿using System.Collections;
using UnityEngine;

/// <summary>
/// Handles inputs to move the piece on the board.
/// Also responsible for making the piece fall over time, 
/// and placing it when it has not moved for a certain period of time.
/// </summary>
[RequireComponent(typeof(Board))]
public class PieceFalling : MonoBehaviour
{
    [SerializeField] private float fallDelay = 0.8f;
    [SerializeField] private float quickFallDelay = 0.125f;
    [SerializeField] private float slideDelay = 0.65f;

    private float fallTimer;

    private float slideTimer;

    private bool quickFallInputted;

    private bool sliding;

    public Board board { get; private set; }
    private void Awake()
    {
        board = GetComponent<Board>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!board.active) return;

        // this code is a tad bit disgusting, may refactor later
        // Falling
        fallTimer += Time.deltaTime;
        float delay = quickFallInputted ? quickFallDelay : fallDelay;
        if (fallTimer >= delay)
        {
            if (!sliding)
            {
                bool moved = board.MovePiece(0, -1);
                if (moved)
                {
                    fallTimer = 0;
                }
                else {
                    sliding = !quickFallInputted;
                    if (!sliding)
                    {
                        board.PlacePiece();
                        fallTimer = 0;
                    }
                }
            }

            if (sliding)
            {
                slideTimer += Time.deltaTime;
                if (quickFallInputted || slideTimer >= slideDelay)
                {
                    sliding = false;
                    slideTimer = 0;
                    board.PlacePiece();
                }
            }
        }
    }

    public void SetQuickFall(bool quickFall)
    {
        quickFallInputted = quickFall;
    }
}
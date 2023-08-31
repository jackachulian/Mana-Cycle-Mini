using System.Collections;
using UnityEngine;

/// <summary>
/// Handles inputs to move the piece on the board.
/// Also responsible for making the piece fall over time, 
/// and placing it when it has not moved for a certain period of time.
/// </summary>
[RequireComponent(typeof(Board))]
public class PieceMovement : MonoBehaviour
{
    [SerializeField] private float fallDelay = 0.8f;
    [SerializeField] private float quickFallDelay = 0.1f;

    private Board board;

    private float fallTimer;

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
        // Left/right
        if (board.controller.LeftPressed())
        {
            board.MovePiece(-1, 0);
        } else if (board.controller.RightPressed())
        {
            board.MovePiece(1, 0);
        }

        // Falling
        fallTimer += Time.deltaTime;
        float delay = board.controller.IsQuickFalling() ? quickFallDelay : fallDelay;
        if (fallTimer >= delay)
        {
            bool moved = board.MovePiece(0, -1);
            if (!moved)
            {
                board.PlacePiece();
            }
            fallTimer = 0;
        }
    }
}
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

    public Board board { get; private set; }

    private float fallTimer;

    private bool quickFallInputted;

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
        // Falling
        fallTimer += Time.deltaTime;
        float delay = quickFallInputted ? quickFallDelay : fallDelay;
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

    public void SetQuickFall(bool quickFall)
    {
        quickFallInputted = quickFall;
    }
}
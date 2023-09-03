using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceQueue : MonoBehaviour
{
    [SerializeField] private Transform queueTransform;

    [SerializeField] private int queueLength;

    [SerializeField] private GameObject piecePrefab;

    private Piece[] queue;


    private List<int> bag;


    public Board board { get; private set; }
    private void Awake()
    {
        board = GetComponent<Board>();

        bag = new List<int>();
    }


    // Use this for initialization
    public void InitializeAfterCycle()
    {
        foreach(Transform child in queueTransform)
        {
            Destroy(child.gameObject);
        }

        queue = new Piece[queueLength];

        // Fill the queue with random pieces
        for (int i = 0; i < queue.Length; i++)
        {
            queue[i] = GeneratePiece();
        }
    }

    // Return the next piece in the queue, advance the queue, and append a new piece to the queue.
    public Piece GetNextPiece()
    {
        Piece nextPiece = queue[0];

        for (int i = 1; i < queue.Length; i++)
        {
            queue[i - 1] = queue[i];
        }

        queue[queue.Length - 1] = GeneratePiece();

        return nextPiece;
    }

    // Generate a new piece inside the queue transform and randomize its colors.
    private Piece GeneratePiece()
    {
        // Instantiate the piece object from prefab
        GameObject pieceObject = Instantiate(piecePrefab, queueTransform);
        Piece piece = pieceObject.GetComponent<Piece>();

        // randomize all tiles
        foreach (ManaTile tile in piece.tiles)
        {
            int color = BagPull();
            tile.SetColor(color, board.cycle.GetManaColor(color));
        }

        // return to be added to the array in the position decided
        // from enclosing function call of either GetNextPiece() or InitializeAfterCycle()
        return piece;
    }

    // Retrieve the next color in the bag, and refill the bag if needed.
    private int BagPull()
    {
        if (bag.Count == 0)
        {
            // Add two of each color
            for (int i = 0; i < board.cycle.uniqueColorsInCycle; i++)
            {
                bag.Add(i);
                bag.Add(i);
            }

            // Add five random colors from the cycle
            for (int i = 0; i < 5; i++)
            {
                bag.Add(board.cycle.sequence[Random.Range(0, board.cycle.cycleLength)]);
            }

            // Shuffle result
            bag = bag.OrderBy(e => Random.value).ToList();
        }

        int next = bag[bag.Count - 1];
        bag.RemoveAt(bag.Count - 1);
        return next;
    }
}
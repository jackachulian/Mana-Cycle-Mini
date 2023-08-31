using System.Collections;
using UnityEngine;

public class PlayerBoardController : MonoBehaviour
{
    [SerializeField] private Board board;

    [SerializeField] private float repeatStartDelay = 0.4f;
    [SerializeField] private float repeatDelay = 0.1f;

    private enum RepeatMode
    {
        None, Left, Right
    }
    private RepeatMode repeatMode;

    private float repeatStartTimer;
    private float repeatTimer;

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput < -0.5f)
        {
            if (HandleRepeatInput(RepeatMode.Left)) board.MovePiece(-1, 0);
        } else if (horizontalInput > 0.5f)
        {
            if (HandleRepeatInput(RepeatMode.Right)) board.MovePiece(1, 0);
        } else
        {
            repeatMode = RepeatMode.None;
        }

        float verticalInput = Input.GetAxisRaw("Vertical");
        board.pieceMovement.SetQuickFall(verticalInput < -0.5f);
    }

    private bool HandleRepeatInput(RepeatMode handleRepeatMode)
    {
        if (repeatMode != handleRepeatMode)
        {
            repeatMode = handleRepeatMode;
            repeatStartTimer = 0;
            // set to timer end value, so that repeat will happen right when repeatStartTimer finihes, and then use repeatDelay from there
            repeatTimer = repeatDelay;

            return true;
        }

        if (repeatStartTimer < repeatStartDelay)
        {
            repeatStartTimer += Time.deltaTime;
        }
        else
        {
            if (repeatTimer >= repeatDelay)
            {
                repeatTimer = 0;
                return true;
            }

            repeatTimer += Time.deltaTime;
        }

        return false;
    }
}
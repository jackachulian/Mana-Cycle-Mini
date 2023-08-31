using System.Collections;
using UnityEngine;

public class PlayerBoardController : IBoardController
{
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
        if (Mathf.Abs(Input.GetAxisRaw("Vertical")) < 0.5f)
        {
            repeatMode = RepeatMode.None;
        }
    }

    public override bool IsQuickFalling()
    {
        return Input.GetAxisRaw("Vertical") < -0.5f;
    }

    public override bool LeftPressed()
    {
        if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            return HandleRepeatInput(RepeatMode.Left);
        } 
        return false;
    }

    public override bool RightPressed()
    {
        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            return HandleRepeatInput(RepeatMode.Right);
        }
        return false;
    }

    private bool HandleRepeatInput(RepeatMode handleRepeatMode)
    {
        if (repeatMode != handleRepeatMode)
        {
            repeatMode = handleRepeatMode;
            repeatStartTimer = 0;
            // set to timer end value, so that repeat will happen right when repeatStartTimer finihes, and then use repeatDelay from there
            repeatTimer = repeatDelay;
            return true; // input once before starting repeat start timer
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
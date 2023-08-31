using System.Collections;
using UnityEngine;

public interface IBoardController
{
    public bool IsQuickFalling();

    public bool LeftPressed();

    public bool RightPressed();
}
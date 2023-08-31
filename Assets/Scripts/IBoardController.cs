using System.Collections;
using UnityEngine;

public abstract class IBoardController : MonoBehaviour
{
    public abstract bool IsQuickFalling();

    public abstract bool LeftPressed();

    public abstract bool RightPressed();
}
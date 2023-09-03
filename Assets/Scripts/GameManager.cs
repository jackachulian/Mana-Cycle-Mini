using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ManaCycle cycle;

    [SerializeField] private Board[] boards;

    // Use this for initialization
    void Start()
    {
        cycle.InitializeCycle();
        // Canvas.ForceUpdateCanvases();
        foreach (Board board in boards) board.InitializeAfterCycle();
    }
}
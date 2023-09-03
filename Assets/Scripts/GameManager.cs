using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ManaCycle cycle;

    // Use this for initialization
    void Start()
    {
        cycle.InitializeCycle();
    }
}
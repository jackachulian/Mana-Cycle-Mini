using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CycleColor : MonoBehaviour
{
    public int color { get; private set; }

    public void SetManaColor(ManaCycle cycle, int index)
    {
        color = index;
        ManaColor manaColor = cycle.GetManaColor(index);
        GetComponent<SpriteRenderer>().color = manaColor.color;
    }
}
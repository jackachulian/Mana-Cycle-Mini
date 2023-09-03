using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ManaCycle : MonoBehaviour
{
    public int[] sequence { get; private set; }

    [SerializeField] private ManaColor[] manaColors;

    [SerializeField] private int cycleLength = 7;

    [SerializeField] private int uniqueColorsInCycle = 5;


    [SerializeField] private Transform cycleColorTransform;

    [SerializeField] private CycleColor cycleColorPrefab;


    public void InitializeCycle()
    {
        sequence = new int[cycleLength];

        // Get at least 1 of each color
        for (int i = 0; i < uniqueColorsInCycle; i++)
        {
            sequence[i] = i;
        }
        // Fill the remaining space with random colors
        for (int i = uniqueColorsInCycle; i < cycleLength; i++)
        {
            sequence[i] = Random.Range(0, uniqueColorsInCycle);
        }

        // shuffle the whole list
        sequence = sequence.OrderBy(e => Random.value).ToArray();

        // Ensure no two of the same color are adjacent
        // If the color above or below is the same, choose a new random color
        for (int i = 1; i < sequence.Length; i++)
        {
            while (sequence[i] == sequence[i - 1] || sequence[i] == sequence[(i + 1) % cycleLength])
            {
                sequence[i] = Random.Range(0, uniqueColorsInCycle);
            }
        }

        // initialize cycle color objects
        foreach (Transform child in cycleColorTransform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < sequence.Length; i++)
        {
            CycleColor cycleColor = Instantiate(cycleColorPrefab, cycleColorTransform);
            cycleColor.SetManaColor(this, sequence[i]);
        }
    }

    public ManaColor GetManaColor(int index)
    {
        return manaColors[index];
    }
}

[System.Serializable]
public class ManaColor
{
    public Color color;
}
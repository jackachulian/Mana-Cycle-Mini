﻿using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ManaCycle : MonoBehaviour
{
    public int[] sequence { get; private set; }

    [SerializeField] private ManaColor[] manaColors;

    [SerializeField] private int _cycleLength = 7;
    public int cycleLength {  get { return _cycleLength; } }


    [SerializeField] private int _uniqueColorsInCycle = 5;
    public int uniqueColorsInCycle { get { return _uniqueColorsInCycle; } }


    [SerializeField] private RectTransform cycleColorTransform;

    [SerializeField] private CycleColor cycleColorPrefab;

    public CycleColor[] cycleColorObjects { get; private set; }

    public void InitializeCycle()
    {
        sequence = new int[_cycleLength];

        // Get at least 1 of each color
        for (int i = 0; i < uniqueColorsInCycle; i++)
        {
            sequence[i] = i;
        }
        // Fill the remaining space with random colors
        for (int i = uniqueColorsInCycle; i < sequence.Length; i++)
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

        cycleColorObjects = new CycleColor[_cycleLength];
        for (int i = 0; i < sequence.Length; i++)
        {
            cycleColorObjects[i] = Instantiate(cycleColorPrefab, cycleColorTransform);
            cycleColorObjects[i].SetManaColor(this, sequence[i]);
        }

        // Immediately refresh the layout, so that pointer placements later this frame are correct
        // LayoutRebuilder.ForceRebuildLayoutImmediate(cycleColorTransform);
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
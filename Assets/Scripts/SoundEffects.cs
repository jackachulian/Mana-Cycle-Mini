using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    // in the future i plan to make a SoundEffect class that has properties like pitch & volume for mixing but for time i'm just using audio clips
    public AudioClip cast;
    public AudioClip move;
    public AudioClip rotate;
    public AudioClip land;
    public AudioClip lose;
    public AudioClip castStartup;
}

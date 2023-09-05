using System.Collections;
using TMPro;
using UnityEngine;

// this entire cutscene system will definitely be destroyed and redone later down the line after the jam
// this is just to get a "storyline" out there in the game since it's a category
public class Cutscene : MonoBehaviour
{
    [SerializeField] private string[] lines;

    [SerializeField] private float fadeSpeed = 1.25f;

    [SerializeField] private float lineDelay = 5f;

    [SerializeField] private TMP_Text label;

    [SerializeField] private Transitioner transitioner;

    [SerializeField] private AudioClip cutsceneBGM;

    int lineIndex = -1;

    bool fadingIn;

    bool fadingOut;

    float fadeAmount;

    float lineTimer;

    // Use this for initialization
    void Start()
    {
        NextLine();
        fadingIn = true;
        SoundManager.Instance.SetBGM(cutsceneBGM);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Spellcast"))
        {
            if (!fadingIn && !fadingOut)
            {
                fadingOut = true;
            }
        }

        if (fadingIn)
        {
            fadeAmount += fadeSpeed * Time.deltaTime;
            if (fadeAmount >= 1)
            {
                fadeAmount = 1;
                fadingIn = false;
                lineTimer = 0;
            }
        } else if (fadingOut)
        {
            fadeAmount -= fadeSpeed * Time.deltaTime;
            if (fadeAmount <= 0)
            {
                fadeAmount = 0;
                NextLine();
                fadingOut = false;
                fadingIn = true;
            }
        } else
        {
            lineTimer += Time.deltaTime;
            if (lineTimer > lineDelay)
            {
                fadingOut = true;
            }
        }

        label.color = new Color(1, 1, 1, fadeAmount);
    }

    void NextLine()
    {
        lineIndex++;

        if (lineIndex >= lines.Length)
        {
            transitioner.TransitionToScene("ManaCycle");
        }

        string str = lines[lineIndex];
        label.text = str;
    }
}
using System.Collections;
using TMPro;
using UnityEngine;

public class BoardUI : MonoBehaviour
{
    [SerializeField] private BoardFallAnim boardFall;

    [SerializeField] private TMP_Text scoreText;

    public void SetScore(int score)
    {
        scoreText.text = ""+score;
    }

    public void OnDeath()
    {
        boardFall.Fall();
    }
}
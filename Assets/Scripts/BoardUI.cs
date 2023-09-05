using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardUI : MonoBehaviour
{
    [SerializeField] private BoardFallAnim boardFall;

    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private Animator postGameAnimator;
    [SerializeField] private GameObject retryButton;

    public void SetScore(int score)
    {
        scoreText.text = ""+score;
    }

    public void OnDeath()
    {
        boardFall.Fall();
        postGameAnimator.ResetTrigger("Close");
        postGameAnimator.SetTrigger("Open");
        EventSystem.current.SetSelectedGameObject(retryButton);
    }
}
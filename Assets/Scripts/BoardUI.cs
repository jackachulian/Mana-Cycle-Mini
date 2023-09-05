using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardUI : MonoBehaviour
{
    [SerializeField] private BoardFallAnim boardFall;

    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private Animator pauseAnimator;
    [SerializeField] private GameObject resumeButton;

    [SerializeField] private Animator postGameAnimator;
    [SerializeField] private GameObject retryButton;

    private Board board;
    private void Awake()
    {
        board = GetComponent<Board>();
    }


    public void SetScore(int score)
    {
        scoreText.text = ""+score;
    }

    public void OnDeath()
    {
        boardFall.Fall();
        StartCoroutine(PostgameAfterDelay());
    }

    IEnumerator PostgameAfterDelay()
    {
        yield return new WaitForSeconds(0.75f);
        postGameAnimator.ResetTrigger("Close");
        postGameAnimator.SetTrigger("Open");
        EventSystem.current.SetSelectedGameObject(retryButton);
    }

    public void TogglePause()
    {
        if (board.paused)
        {
            pauseAnimator.ResetTrigger("Close");
            pauseAnimator.SetTrigger("Open");
            EventSystem.current.SetSelectedGameObject(resumeButton);
        } else
        {
            pauseAnimator.ResetTrigger("Open");
            pauseAnimator.SetTrigger("Close");
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
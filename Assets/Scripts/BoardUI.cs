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

    [SerializeField] private AudioClip postgameBGM;

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
        yield return new WaitForSeconds(1.5f);

        // after the jam, probably should make postgame its own class, so that one or more boards cna share its fuctionality
        // same for pausing
        postGameAnimator.ResetTrigger("Close");
        postGameAnimator.SetTrigger("Open");
        EventSystem.current.SetSelectedGameObject(retryButton);
        SoundManager.Instance.SetBGM(postgameBGM);
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
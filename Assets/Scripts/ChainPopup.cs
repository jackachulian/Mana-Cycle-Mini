using System.Collections;
using TMPro;
using UnityEngine;

public class ChainPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private Animator animator;
    // Use this for initialization
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Popup(int value)
    {
        text.text = "" + value;
        animator.SetTrigger("popup");
    }
}
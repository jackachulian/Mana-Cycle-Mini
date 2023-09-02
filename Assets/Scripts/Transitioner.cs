using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.Animations;

public class Transitioner : MonoBehaviour
{

    [SerializeField] Animator animator;
    private string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // called by other objects to switch scene smoothly
    public void TransitionToScene(string s)
    {
        nextScene = s;
        animator.SetTrigger("In");
        animator.ResetTrigger("Out");
    }

    // called by an event in the transition animation
    private void SwitchScene()
    {
        // nextScene set by TransitionToScene function when its called
        Debug.Log("Loaded Scene " + nextScene);
        animator.ResetTrigger("In");
        animator.SetTrigger("Out");
        SceneManager.LoadScene(nextScene);
        
    }
}

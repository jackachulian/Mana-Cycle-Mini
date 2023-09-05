using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

public class Transitioner : MonoBehaviour
{

    [SerializeField] Animator animator;
    private string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject.transform.root);
    }

    // called by other objects to switch scene smoothly
    public void TransitionToScene(string s)
    {
        nextScene = s;
        animator.ResetTrigger("Out");
        animator.SetTrigger("In");
        
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

    // After transition is complete, destroy this object that belonged to the scene that was just transitioned from
    // The new scene should have a new transitioner if it needs one, that is referenced by things in that scene
    private void TransitionComplete()
    {
        Destroy(gameObject);
    }

    public void PlayPressed()
    {
        int watchedCutscene = PlayerPrefs.GetInt("watched_cutscene", 0);
        if (watchedCutscene == 0)
        {
            PlayerPrefs.SetInt("watched_cutscene", 1);
            TransitionToScene("Cutscene");
        } else
        {
            TransitionToScene("ManaCycle");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class MusicSetUp : MonoBehaviour
{
    [SerializeField] private AudioClip[] songs;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.SetBGM(songs[Random.Range(0, songs.Length)]);
        SoundManager.Instance.PlayBGM();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    //public string[] _clipNames;     //Keep all audio files inside resources folder  and give the value of path alone in this varible through editor.
    public int soundCount;
    int i = 1;
    private AudioSource source;

    private static MusicPlayer instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
        StartAudio();   // start first time without Delay
    }

    void StartAudio()
    {
        source.Stop();
        AudioClip clip = Resources.Load<AudioClip>("Sounds/GameMusic" + i);
        if(clip == null)
        {
            Debug.Log("anasını sikeyim");
        }
        source.clip = clip;
        source.Play();

        Invoke("StartAudio", (float)clip.length + 0.5f);

        i++;

        if (i > soundCount)
        {
            i = 1;
        }
    }
}

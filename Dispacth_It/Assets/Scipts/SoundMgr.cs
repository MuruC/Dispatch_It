using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeNameSpace;

[RequireComponent(typeof(AudioSource))]
public class SoundMgr : MonoBehaviour
{
    public List<AudioClip> dialogues;
    public List<AudioClip> audios;
    public static SoundMgr Instance = null;
    AudioSource audioSource;

    private float ringClipLength = 3.5f;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayDialogue(string clipName)
    {
        audioSource.PlayOneShot(dialogues.Find(x => x.name.Equals(clipName)));
    }

    public void PlayDialogue(string clipName, AudioSource AS)
    {
        AS.PlayOneShot(dialogues.Find(x => x.name.Equals(clipName)));
    }

    public void PlayAudio(string clipName)
    {
        audioSource.PlayOneShot(audios.Find(x => x.name.Equals(clipName)));
    }

    public void PlayAudio(string clipName, AudioSource AS)
    {
        AS.PlayOneShot(audios.Find(x => x.name.Equals(clipName)));
    }
}

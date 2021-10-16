using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessageTree;
using TimeNameSpace;

public class Telephone : MonoBehaviour
{
    bool PickedUp = false;
    //bool putDown = false;
    int putDown = -1; // -1:initial; 0: not complete but put down; 1:complete intro but not put down; 2: compelete intro and put down; 3: boss interuption came in; 4: ending; 
    bool introClip1Finished = false;
    bool introClip2Finished = false;
    int listenIntroCount = 0;
    PhoneMessage PhoneMessage;
    AudioSource audioSource;
    public AudioSource ringAudioSource;
    bool ringOn = true;
    void Start()
    {
        PhoneMessage = GameObject.Find("PhoneMessage").GetComponent<PhoneMessage>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public void PickUp()
    {
        //print("putDown"+putDown);
        //print("listenIntroCount"+listenIntroCount);
        if (putDown == 2)
        {
            return;
        }
        ringAudioSource.Stop();
        /*
        if (ringOn)
        {
            audioSource.Stop();
            ringOn = false;
        }
        */
        if (putDown == 3)
        {
            //SoundMgr.Instance.PlayDialogue("boss_knob", audioSource);
            putDown = 2;
            PhoneMessage.ContinueConversation(0, false);
            //PlayDialogue(string clipName, audioSource);
            return;
        }
        else if (putDown == 4)
        {
            //SoundMgr.Instance.PlayDialogue("boss_intro", audioSource);
            PhoneMessage.ContinueConversation(0, false);
            putDown = 2;
            return;
        }
        else
        {
            if (listenIntroCount < 2)
            {
                PhoneMessage.PlayCurrentDialogue();
                PhoneMessage.CountIntroCallTime(this);
                listenIntroCount++;
            }
            //PhoneMessage.PlayTelephoneAudio("boss_intro");
            //SoundMgr.Instance.PlayDialogue("boss_intro", audioSource);
        }
    }
    
    public void PlayRingAudio()
    {
        ringOn = true;
        ringAudioSource.Play();
    }

    public void PutDown()
    {
        if (putDown == 2)
        {
            return;
        }
        if (putDown == 1)
        {
            PhoneMessage.StartConversation();
            putDown = 2;
        }
        else if (putDown == -1)
        {
            if (listenIntroCount == 1)
            {
                print(putDown+"  "+ listenIntroCount);
                audioSource.Stop();
                print("put down before boss call ends!");
                putDown = 0;
                PhoneMessage.ChangeIntroAudio();
                float delayedRingTime = 1f;
                TimeMgr.Instance.AddDelayEvent(delayedRingTime, PlayRingAudio);
            }
            else
            {
                putDown = 5;
            }
        }
        else if (putDown == 0 && listenIntroCount == 2)
        {
            if (introClip2Finished)
            {
                PhoneMessage.StartConversation();
                putDown = 2;
            }
            else
            {
                putDown = 5;
            }  
        }
    }

    public void BossPhoneEnd()
    {
        if (!introClip1Finished)
        {
            introClip1Finished = true;
        }
        else 
        {
            introClip2Finished = true;
        }
        if (putDown == 0)
        {
            if (listenIntroCount == 1)
            {
                return;
            }
        }
        else if (putDown == 5)
        {
            PhoneMessage.StartConversation();
            putDown = 2;
        }
        else
        {
            putDown = 1;
        }
    }

    public void StartBossCall(int putDownVal)
    {
        PlayRingAudio();
        //PickedUp = true;
        putDown = putDownVal;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Console;
using TMPro;
using TimeNameSpace;
namespace MessageTree
{
    using DictOptionNodes = Dictionary<phonePlugInEnum, Node>;
    public enum phonePlugInEnum { hospital, police, fireStation, conversation, ring, ending }
    class Node
    {
        public Node(float clipLength, string clipName, int score, AudioSource myAudioSource)
        {
            ClipLength = clipLength;
            ClipName = clipName;
            Score = score;
            audioSource = myAudioSource;
            OptionNodes = new DictOptionNodes();
        }

        public Node addOption(phonePlugInEnum option, Node node)
        {
            OptionNodes[option] = node;
            return this;
        }

        public Node addOption(phonePlugInEnum option, float clipLength, string clipName, int score, AudioSource myAs)
        {
            OptionNodes[option] = new Node(clipLength, clipName, score, myAs);
            return this;
        }

        public Node getOption(params phonePlugInEnum[] options)
        {
            Node result = null;
            DictOptionNodes currentOptionNodes = OptionNodes;
            foreach (var option in options)
            {
                Debug.Log(option);
                if(!currentOptionNodes.ContainsKey(option))
                {
                    Debug.Log("Can't find option " + option.ToString() + " for current node.");
                    break;
                }

                result = currentOptionNodes[option];
                currentOptionNodes = result.OptionNodes;
            }
            return result;
        }

        public void PlayAudio()
        {
            SoundMgr.Instance.PlayDialogue(ClipName);
        }

        public float ClipLength { get; set; }
        public string ClipName { get; set; }
        public int Score { get; set;  }
        public AudioSource audioSource { get; set; }
        public DictOptionNodes OptionNodes { get; set; }
    }
    //public enum phoneMessageEnum { beginning, conversation1, conversation1A, conversation1B,converstation1C,conversation2,conversation2A,conversation2B,conversation2C }
    public enum phoneMessageEnum { beginning, conversation1, conversation2, conversation3, right, wrong}
    public class PhoneMessage : MonoBehaviour
    {
        public TextMeshProUGUI textDisplay;
        public GameObject startButton;
        public AudioSource Telephone;
        public AudioSource Left;
        public AudioSource Right;
        public Telephone telephoneScript;
        public GameObject trophy;
        public GameObject creditUi;

        public List<GameObject> knobs;
        private Node rootNode;
        private Node endNode;
        private Node currentNode;
        private float ringClipLength = 3.5f;
        private Snap snap;
        private bool bStartConversation = false;
        [SerializeField]
        private int scores = 0;
        public void PlayCurrentDialogue()
        {
            PlayDialogueAudio(currentNode);
        }
        void ShowDelayedDialogue(float time)
        {
            Delegate1 handler = PlayCurrentDialogue;
            TimeMgr.Instance.AddDelayEvent(time, handler);
        }
        public void PlayRing()
        {
            SoundMgr.Instance.PlayAudio("beep", Left);
        }
        void PlayDelayedRing(float time)
        {
            Delegate1 handler = PlayRing;
            TimeMgr.Instance.AddDelayEvent(time, handler);
        }
        // Start is called before the first frame updateea11
        void Start()
        {
            snap = GameObject.Find("Plug").GetComponent<Snap>();
            //trophy = GameObject.Find("trophy");
            Node startNode = new Node(21, "boss_intro", 0, Telephone);
            rootNode = startNode;
            currentNode = startNode;
            Node conversation1Node = new Node(16, "bump_head_call", 0, Left);
            Node bossInterlude = new Node(7, "boss_knob", 0, Telephone);
            startNode.addOption(phonePlugInEnum.conversation, conversation1Node);
            Node conversation2Node = new Node(14, "popcorn_explosion_call", 0, Left);
            conversation1Node.addOption(phonePlugInEnum.police, 5, "bump_head_police", -1, Right).addOption(phonePlugInEnum.ring, bossInterlude);
            conversation1Node.addOption(phonePlugInEnum.hospital, 8, "bump_head_hospital", 1, Right);
            conversation1Node.addOption(phonePlugInEnum.fireStation, 8, "bump_head_firestation", -1, Right);
            bossInterlude.addOption(phonePlugInEnum.conversation, conversation2Node);
            Node conversation3Node = new Node(19, "popcorn_crime_call", 0, Left);
            conversation2Node.addOption(phonePlugInEnum.police, 8, "popcorn_explosion_police",-1, Right).addOption(phonePlugInEnum.conversation, conversation3Node);
            conversation2Node.addOption(phonePlugInEnum.hospital, 10, "popcorn_explosion_hospital",-1, Right);
            conversation2Node.addOption(phonePlugInEnum.fireStation, 10, "popcorn_explosion_firestation",1, Right);
            endNode = new Node(26, "boss_end_good", 0, Telephone);
            conversation3Node.addOption(phonePlugInEnum.police, 9, "popcorn_crime_police", 1, Right);
            conversation3Node.addOption(phonePlugInEnum.hospital, 15, "popcorn_crime_hospital", -1, Right);
            conversation3Node.addOption(phonePlugInEnum.fireStation, 14, "popcorn_crime_firestation", -1, Right).addOption(phonePlugInEnum.ending, endNode); 
        }

        void PlayDialogueAudio(Node node)
        {
            if (node == null)
            {
                return;
            }
            SoundMgr.Instance.PlayDialogue(node.ClipName, node.audioSource);
        }

        public void StartConversation()
        {
            if (bStartConversation)
            {
                return;
            }
            bStartConversation = true;
            float delayTime = 2.0f;
            currentNode = rootNode.getOption(phonePlugInEnum.conversation);
            PlayDelayedRing(delayTime);
            ShowDelayedDialogue(ringClipLength + delayTime);
        }

        public void ChooseOption(phonePlugInEnum option)
        {
            print(option);
            Right.Stop();
            Left.Stop();
            Node optionNode = currentNode.getOption(option);
            PlayDialogueAudio(optionNode);
            ContinueConversation(optionNode.ClipLength, true);
            /*
            float delayedRingAudioTime = 3.0f;
            float totalDelayedRingTime = delayedRingAudioTime + optionNode.ClipLength;
            PlayDelayedRing(totalDelayedRingTime);
            ShowDelayedDialogue(totalDelayedRingTime + ringClipLength);
            */
            scores = scores + optionNode.Score;
            
        }

        public void ContinueConversation(float delayedTime, bool ringFirst)
        {
            RestoreSnap(delayedTime);
            float delayedRingAudioTime = 3.0f;
            float totalDelayedRingTime = delayedRingAudioTime + delayedTime;

            DictOptionNodes optionNodes = currentNode.OptionNodes;
            if (optionNodes.ContainsKey(phonePlugInEnum.ring)) {
                if (ringFirst)
                {
                    void startBossCall()
                    {
                        telephoneScript.StartBossCall(3);
                    }
                    TimeMgr.Instance.AddDelayEvent(delayedTime, startBossCall);
                }
                else
                { 
                    currentNode = currentNode.OptionNodes[phonePlugInEnum.ring];
                    PlayDialogueAudio(currentNode);
                    ContinueConversation(currentNode.ClipLength + 1.0f, false);
                }
            }
            else if (optionNodes.ContainsKey(phonePlugInEnum.ending))
            {
                if (ringFirst)
                {
                    void startBossCall()
                    {
                        telephoneScript.StartBossCall(4);
                    }
                    TimeMgr.Instance.AddDelayEvent(delayedTime, startBossCall);
                    if (scores == -3)
                    {
                        endNode.ClipName = "boss_end_bad";
                        endNode.ClipLength = 5;
                        //SoundMgr.Instance.PlayDialogue();
                    }
                }
                else 
                {
                    currentNode = currentNode.OptionNodes[phonePlugInEnum.ending];
                    PlayDialogueAudio(currentNode);
                    if (scores == -3)
                    {
                        return;
                    }
                    void showTrophy()
                    {
                        trophy.SetActive(true);
                        creditUi.SetActive(true);
                        SoundMgr.Instance.PlayAudio("success");
                    }
                    TimeMgr.Instance.AddDelayEvent(currentNode.ClipLength, showTrophy);
                }
            }
            else if (optionNodes.ContainsKey(phonePlugInEnum.conversation))
            {
                randomizeSound();
                currentNode = currentNode.OptionNodes[phonePlugInEnum.conversation];
                PlayDelayedRing(totalDelayedRingTime);
                ShowDelayedDialogue(totalDelayedRingTime + ringClipLength);
            }
        }

        void randomizeSound()
        {
            foreach (GameObject knob in knobs)
            {
                Knob knobScript = knob.GetComponent<Knob>();
                knobScript.Randomize();
            }
        }

        public void PlayTelephoneAudio(string clipName)
        {
            SoundMgr.Instance.PlayDialogue(clipName, Telephone);
        }

        void RestoreSnap(float optionClipLength)
        {
            float delayedTime = 1.0f;
            Delegate1 handler = snap.ResetToDefault;
            TimeMgr.Instance.AddDelayEvent(delayedTime + optionClipLength, handler);
        }

        public void ChangeIntroAudio()
        {
            print("change intro audio");
            rootNode.ClipName = "boss_hangup";
            rootNode.ClipLength = 28f;
        }

        public void CountIntroCallTime(Telephone telephoneScript)
        {
            print("call count intro call time!" + rootNode.ClipLength);
            TimeMgr.Instance.AddDelayEvent(rootNode.ClipLength, telephoneScript.BossPhoneEnd);
        }    
    }
}
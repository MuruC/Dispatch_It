using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeNameSpace {
    public delegate void Delegate1();
    public class Event
    {
        public Event(float startTime, float gapTime, bool isLoop, Delegate1 handler)
        {
            StartTime = startTime;
            GapTime = gapTime;
            IsLoop = isLoop;
            Handler = handler;
        }
        public float StartTime {get;set;}
        public float GapTime { get; set; }
        public bool IsLoop { get; set; }
        public Delegate1 Handler { get; set; }
    }
    public class TimeMgr : MonoBehaviour
    {
        public static TimeMgr Instance = null;
        List<Event> allEventList = new List<Event>();
        List<Event> activeEventList = new List<Event>();
        private float curTime = 0f;
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
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            curTime += Time.deltaTime;
            for (int i = allEventList.Count - 1; i >= 0; i--)
            {
                Event timeEvent = allEventList[i];
                if (timeEvent.StartTime + timeEvent.GapTime <= curTime)
                {
                    activeEventList.Add(timeEvent);
                    if (timeEvent.IsLoop)
                    {
                        timeEvent.StartTime = curTime;
                    }
                    else
                    {
                        allEventList.Remove(timeEvent);
                    }
                }
            }
            for (int i = activeEventList.Count - 1; i >= 0; i--)
            {
                Event activeEvent = activeEventList[i];
                activeEvent.Handler();
                activeEventList.Remove(activeEvent);
            }
        }

        public void AddDelayEvent(float gapTime, Delegate1 handler)
        {
            Event newEvent = new Event(curTime, gapTime, false,handler);
            allEventList.Add(newEvent);
        }

        public void AddRtEvent(float startTime, float delayTime, Delegate1 handler)
        {
            Event newEvent = new Event(startTime, delayTime, true, handler);
            allEventList.Add(newEvent);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Vuforia;
using System;

public class Timer : MonoBehaviour, ITrackableEventHandler
{

    private TrackableBehaviour mTrackableBehaviour;
    // public Transform myModelPrefab;
    public Text timerText;
    private float startTime; //makes one instance of it
    public static bool pauseTimer = false;
    public static bool startTimer = false;

    // define variables
    string minutes;
    string seconds;

    /* Use this for initialization
    void Start () {
         startTime = Time.time; 
    }*/

    //Start Timer
    public void Start()
    {
        //Start Timer Here
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseTimer)
            return;
        if (startTimer)
        {
            float t = Time.time - startTime; //stores time since timer has started
             minutes = ((int)t / 60).ToString();
             seconds = (t % 60).ToString("f2"); //shows ms

            timerText.text = minutes + ":" + seconds;
        }

    }

    //Reset Timer
    public void ResetTimer()
    {
        startTime = 0;
        Debug.Log("Timer Reset");
    }


    //Stop Timer
    public void StopTimer()
    {
        //Stop Timer Here
        pauseTimer = true;
        Debug.Log("Timer Stopped");
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
         newStatus == TrackableBehaviour.Status.TRACKED ||
         newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            OnTrackingFound();
        }
        else
        {
            // pause timer

            StopTimer();
        }
    }

    private void OnTrackingFound()
    {
        if (timerText != null)
        {

            if (mTrackableBehaviour.TrackableName == "africaB0")
            {
                Debug.Log(">>>Start timer");
                startTimer = true;
            }
            Debug.Log("Timer Started");
            //Text newTimer = GameObject.Instantiate(timerText) as Text;
            //newTimer.parent = mTrackableBehaviour.transform;
        }
    }
}

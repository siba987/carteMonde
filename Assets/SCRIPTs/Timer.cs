using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour {

    public Text timerText;
    public static float startTime; //makes one instance of it
    public static bool pauseTimer = false;
    public static bool startTimer = false;

    /* Use this for initialization
    void Start () {
         startTime = Time.time; 
    }*/

    //Start Timer
    public void Start()
    {
        //Start Timer Here
        startTime = Time.time;
        Debug.Log("Timer Started");
    }

    // Update is called once per frame
    void Update () {
        if (pauseTimer)
            return;
        if (startTimer)
        {
            float t = Time.time - startTime; //stores time since timer has started
            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f2"); //shows ms

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

}

/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;
using Vuforia;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// A custom handler that implements the ITrackableEventHandler interface.
///
/// Changes made to this file could be overwritten when upgrading the Vuforia version.
/// When implementing custom event handler behavior, consider inheriting from this class instead.
/// </summary>
public class AfricaTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    #region PROTECTED_MEMBER_VARIABLES

    private Renderer obj;
    public Text timerText;
    public float startTime; //makes one instance of it
    public bool startTimer = false;
    public bool stopTimer = false;
    private bool pauseTimer = false;

    //declare global var
    string minutes;
    string seconds; //shows ms
    //[SerializeField] private GameObject secondObj;

    private IEnumerator coroutine;
    protected TrackableBehaviour mTrackableBehaviour;
    protected TrackableBehaviour.Status m_PreviousStatus;
    protected TrackableBehaviour.Status m_NewStatus;

    #endregion // PROTECTED_MEMBER_VARIABLES

    #region UNITY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
//        timerText.text = "00:00";
        obj = GetComponent<Renderer>();
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    protected virtual void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        m_PreviousStatus = previousStatus;
        m_NewStatus = newStatus;

        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PROTECTED_METHODS

    protected virtual void OnTrackingFound()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents){
            component.enabled = true;
            obj = component;
        }

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;

        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
        if ((mTrackableBehaviour.TrackableName == "africaB0") || 
            (mTrackableBehaviour.TrackableName == "africaB1") ||
            (mTrackableBehaviour.TrackableName == "africaB2") ||
            (mTrackableBehaviour.TrackableName == "africaB3") ||
            (mTrackableBehaviour.TrackableName == "africaB4")){
            Debug.Log(">>> inside B0 ");
            EnableItem();        
        }

        //check for continent outline
        if (mTrackableBehaviour.TrackableName == "africaB0")
        {
            Debug.Log(">>>Start timer");
            startTimer = true;
        }

        // added to stop timer
        if (mTrackableBehaviour.TrackableName == "africaB5")
        {

            Debug.Log(">>>(AT)Stop timer");
            stopTimer = true;

        }
    }


    protected virtual void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;

      //  pauseTimer = true;

    }

    public void EnableItem(){
        if(coroutine != null){ return; }
         Debug.Log(">>> couroutine 0 ");
        coroutine = ShowItem();
        Debug.Log(">>> couroutine 1 ");
        StartCoroutine(coroutine);
      }
      private IEnumerator ShowItem(){
           //firstObj.SetActive(true);
          Debug.Log(">>> ShowItem 0 ");
           yield return new WaitForSeconds(2f);
           Debug.Log(">>> ShowItem 5s ");
           if(obj != null){
                obj.enabled = false;
                Debug.Log(">>> obj false ");
            }
                 StopCoroutine(coroutine);
                 coroutine = null;
           //firstObj.SetActive(false);
           //secondObj.SetActive(true);
      }
      // This is called from the tracking script when losing track
      public void DisableItem(){
            StopCoroutine(coroutine);
            coroutine = null;
            //firstObj.SetActive(false);
            //secondObj.SetActive(false);
      }

    /* added methods*/
    //Start Timer
    public void StartTimer()
    {
        //Start Timer Here
        startTime = Time.time;
        Debug.Log("Timer Started");
    }

// Update is called once per frame
    void Update () {


        if (startTimer)
        {
            Debug.Log("*************");
            float t = Time.time - startTime; //stores time since timer has started
            minutes = ((int)t / 60).ToString();
            seconds = (t % 60).ToString("f2"); //shows ms

            timerText.text = minutes + ":" + seconds;
        }
        else if (stopTimer)
        {
            startTimer = false;
            Debug.Log(">>> FINALEMENT!!! ONA  REUSSI");
            StopTimer();

            stopTimer = false;

        }
        else if (pauseTimer)
        {
            PauseTimer();
            pauseTimer = false;

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
        //to be chanaged later
        Debug.Log("Timer Stopped");
        PauseTimer();


    }

    //Pause timer
    public void PauseTimer()
    {
        startTimer = false;
        Debug.Log("Timer PAUSED");
        // timerText.text = "00:86";
        //        timerText.color = Color.cyan;
    }

    #endregion // PROTECTED_METHODS
 }

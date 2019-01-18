using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTimerStop : MonoBehaviour {

    // Use this for initialization
    public void OnTriggerEnter(Collider other) {
        GameObject.Find("africaB0").SendMessage("StopTimer");
        Debug.Log(">>>>stop timer");
    }

}

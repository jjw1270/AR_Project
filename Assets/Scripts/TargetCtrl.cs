using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCtrl : MonoBehaviour
{
    public int targetNum;
    public void OnTargetFound(){
        GameManager.Instance.targetFoundList[targetNum] = true;
    }

    public void OnTargetLost(){
        GameManager.Instance.targetFoundList[targetNum] = false;
    }
}

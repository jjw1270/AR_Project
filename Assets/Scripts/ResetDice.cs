using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDice : MonoBehaviour
{
    public Vector3 startPos;
    private void Awake() {
        startPos = this.transform.position;
    }
    private void Update() {
        if(this.transform.position.y < -50 && GameManager.Instance.isAllTargetFound){
            Debug.Log("ResetDice");
            this.transform.position = startPos;
        }
    }
}

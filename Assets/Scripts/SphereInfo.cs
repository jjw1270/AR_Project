using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereInfo : MonoBehaviour
{
    public Transform tf;
    public Renderer rderer;
    public Rigidbody rb;
    private Vector3 startPos;
    private void Start() {
        startPos = this.transform.position;
    }

    private void Update() {
        if(this.transform.position.y < -50 && GameManager.Instance.isAllTargetFound){
            Debug.Log("Reset");
            this.transform.position = new Vector3(this.transform.parent.position.x+Random.Range(0f,2f), this.transform.parent.position.y+Random.Range(1f,3f), this.transform.parent.position.z+Random.Range(0f,2f));
        }
    }

}

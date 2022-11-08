using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VirtualBtnCtrl : MonoBehaviour
{
    private VirtualButtonBehaviour vb;
    private void Start() {
        vb = this.GetComponent<VirtualButtonBehaviour>();
        vb.RegisterOnButtonPressed(OnButtonPressed);
        vb.RegisterOnButtonReleased(OnButtonReleased);
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb){
        Debug.Log(this.gameObject.name + " 버튼 눌림");
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb){
        Debug.Log(this.gameObject.name + " 버튼 떼짐");
    }
}

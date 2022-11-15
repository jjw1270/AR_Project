using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDiceNumber : MonoBehaviour
{
    public int diceNumber;
    private void OnTriggerStay(Collider other) {
        if(other.CompareTag("Dice")){
            diceNumber = 7-int.Parse(other.gameObject.name);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Dice")){
            diceNumber = 0;
        }
    }
}

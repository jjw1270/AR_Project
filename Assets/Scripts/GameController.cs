using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [System.Serializable]
    public struct Card{
        public GameObject score;
        public List<GameObject> scoreList;
        public GameObject enemyTargetPos;
    }
    public Card[] card = new Card[2];

    
    public void OnGameStart(){
        card[0].scoreList = new List<GameObject>();
        Transform[] spheres = card[0].score.GetComponentsInChildren<Transform>();
        foreach(Transform sphere in spheres){
            sphere.gameObject.SetActive(true);
            card[0].scoreList.Add(sphere.gameObject);
        }
        
        card[1].scoreList = new List<GameObject>();
        spheres = card[1].score.GetComponentsInChildren<Transform>();
        foreach(Transform sphere in spheres){
            sphere.gameObject.SetActive(true);
            card[1].scoreList.Add(sphere.gameObject);
        }
    }

    public void ScoreCtrl(bool isCard1 ,int count){
        if(isCard1){

        }
        else{

        }
    }
}

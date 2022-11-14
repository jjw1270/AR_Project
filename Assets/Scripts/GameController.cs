using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    [System.Serializable]
    public struct Card{
        public GameObject score;
        public List<GameObject> scoreList;
        public Transform targetPos;
    }
    public Card[] card = new Card[2];

    public GameObject dice;

    [SerializeField]Color black;
    [SerializeField]Color white;
    
    public void OnGameStart(){
        dice.SetActive(true);

        card[0].scoreList = new List<GameObject>();
        card[0].score.SetActive(true);
        Transform[] spheres = card[0].score.GetComponentsInChildren<Transform>();
        foreach(Transform sphere in spheres){
            //sphere.gameObject.SetActive(true);
            card[0].scoreList.Add(sphere.gameObject);
        }
        
        card[1].scoreList = new List<GameObject>();
        card[1].score.SetActive(true);
        spheres = card[1].score.GetComponentsInChildren<Transform>();
        foreach(Transform sphere in spheres){
            if(sphere.gameObject.CompareTag("Score")) continue;
            //sphere.gameObject.SetActive(true);
            card[1].scoreList.Add(sphere.gameObject);
        }
    }

    bool isMoving;
    public void ScoreCtrl(bool card1, int count){   //점수를 얻는 카드, 점수
        Card thisCard = card1 ? card[0] : card[1];
        Card enemyCard = !card1 ? card[0] : card[1];

        //0.2초 간격으로 공 이동
        StartCoroutine(MoveSphere(thisCard, count, enemyCard, card1));
    }

    IEnumerator MoveSphere(Card thisCard, int count, Card enemyCard, bool card1){
        for(int i = 0; i < count; i++){
            SphereInfo thisSphere =  enemyCard.scoreList[0].GetComponent<SphereInfo>();

            Vector3 halfPos = thisSphere.tf.position + ((thisCard.targetPos.position - thisSphere.tf.position)/2);

            Vector3[] movePath = {thisSphere.tf.position, 
                                new Vector3(halfPos.x+Random.Range(0,1f), halfPos.y+Random.Range(0,1f),
                                halfPos.z), thisCard.targetPos.position};

            //Vector3[] movePath = {thisSphere.tf.position, halfPos, thisCard.targetPos.position};

            thisSphere.rb.DOPath(movePath, 0.8f, PathType.CatmullRom, PathMode.Full3D);
            thisSphere.rderer.material.color = card1 ? white : black;
            
            thisCard.scoreList.Add(thisSphere.gameObject);
            thisSphere.tf.parent = thisCard.score.transform;
            enemyCard.scoreList.RemoveAt(0);

            yield return new WaitForSeconds(0.4f);
        }
        yield break;
    }
}

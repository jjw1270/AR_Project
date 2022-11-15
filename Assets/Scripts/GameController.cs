using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [System.Serializable]
    public struct Card{
        public GameObject score;
        public List<GameObject> scoreList;
        public Transform targetPos;
        public ParticleSystem winParticle;
    }
    public Card[] card = new Card[2];
    int card1TotalScore;
    int card2TotalScore;
    int totalSphereCount;

    public GameObject dice;
    private Rigidbody diceRb;
    private float throwForce = 20f;
    private float rotateForce = 900000f;
    [SerializeField]GetDiceNumber getDiceNumber;
    public bool isThrowingDice;
    [SerializeField]ParticleSystem diceParticle;

    [SerializeField]Color black;
    [SerializeField]Color white;

    private bool isCard1FirstStart;

    [SerializeField]GameObject turnCard1Panel;
    [SerializeField]GameObject turnCard2Panel;
    [SerializeField]TextMeshProUGUI textCard1;
    [SerializeField]TextMeshProUGUI textCard2;
    

    private void Awake() {
        diceRb = dice.GetComponent<Rigidbody>();
    }
    
    public void OnGameStart(){
        dice.SetActive(true);

        card[0].scoreList = new List<GameObject>();
        card[0].score.SetActive(true);
        Transform[] spheres = card[0].score.GetComponentsInChildren<Transform>();
        foreach(Transform sphere in spheres){
            if(sphere.gameObject.CompareTag("Score")) continue;
            //sphere.gameObject.SetActive(true);
            card[0].scoreList.Add(sphere.gameObject);
        }
        card1TotalScore = card[0].scoreList.Count;
        
        card[1].scoreList = new List<GameObject>();
        card[1].score.SetActive(true);
        spheres = card[1].score.GetComponentsInChildren<Transform>();
        foreach(Transform sphere in spheres){
            if(sphere.gameObject.CompareTag("Score")) continue;
            //sphere.gameObject.SetActive(true);
            card[1].scoreList.Add(sphere.gameObject);
        }
        card2TotalScore = card[1].scoreList.Count;
        totalSphereCount = card1TotalScore + card2TotalScore;

        isCard1FirstStart = (Random.value > 0.5f);  //플레이어 순서 true : card1, false : card2
        
        ShowThrowPanel();
    }

    public void ThrowDice(){
        isThrowingDice = true;
        SoundManager.Instance.PlaySFXSound("Dice");
        Vector3 randomRotation = new Vector3(Random.Range(-1,1f), Random.Range(-1,1f), Random.Range(-1,1f));
        diceRb.AddForce(Vector3.up * (throwForce+Random.Range(-8f,8f)), ForceMode.Impulse);
        diceRb.AddTorque(randomRotation * rotateForce * 10000, ForceMode.Impulse);

        StartCoroutine(DiceNumber());
    }

    IEnumerator DiceNumber(){
        Vector3 tmpPos;
        while(true){
            tmpPos = dice.transform.position;
            yield return new WaitForSeconds(1f);

            if(tmpPos == dice.transform.position){
                ScoreCtrl(isCard1FirstStart ,getDiceNumber.diceNumber);
                yield break;
            }
        }
    }

    public void ScoreCtrl(bool card1, int count){   //점수를 얻는 카드, 점수
        if(count < 1 || count > 6){  //오류처리
            isThrowingDice = false;
            SoundManager.Instance.textToSpeech.PlayTTS("DiceAgain");
            GameManager.Instance.logText.text = "주사위를 다시 굴려주세요";
            return;
        }
        isCard1FirstStart = !card1;
        Card thisCard = card1 ? card[0] : card[1];
        Card enemyCard = card1 ? card[1] : card[0];

        diceParticle.Play();

        SoundManager.Instance.textToSpeech.PlayTTS(count.ToString());
        StartCoroutine(MoveSphere(thisCard, count, enemyCard, card1));
    }

    IEnumerator MoveSphere(Card thisCard, int count, Card enemyCard, bool card1){
        for(int i = 0; i < count; i++){
            if(enemyCard.scoreList.Count<=0){
                GetTotScore();
                yield break;
            }
            SphereInfo thisSphere =  enemyCard.scoreList[0].GetComponent<SphereInfo>();

            Vector3 halfPos = thisSphere.tf.position + ((thisCard.targetPos.position - thisSphere.tf.position)/2);

            Vector3[] movePath = {thisSphere.tf.position, 
                                  new Vector3(halfPos.x+Random.Range(1f,5f), halfPos.y+Random.Range(1f,5f), halfPos.z+Random.Range(1f,5f)),
                                  thisCard.targetPos.position};

            SoundManager.Instance.PlaySFXSound("SphereMove");
            thisSphere.rb.DOPath(movePath, 0.8f, PathType.CatmullRom, PathMode.Full3D);
            thisSphere.rderer.material.color = card1 ? white : black;
            
            thisCard.scoreList.Add(thisSphere.gameObject);
            thisSphere.tf.parent = thisCard.score.transform;
            enemyCard.scoreList.RemoveAt(0);

            yield return new WaitForSeconds(0.4f);
        }
        GetTotScore();
        yield break;
    }
    
    private void GetTotScore(){
        card1TotalScore = card[0].scoreList.Count;
        card2TotalScore = card[1].scoreList.Count;

        if(card1TotalScore < totalSphereCount && card2TotalScore < totalSphereCount){
            Invoke("ShowThrowPanel", 0.5f);
            return;
        }
        SoundManager.Instance.PlaySFXSound("GameOver");
        if(card1TotalScore == totalSphereCount){
            card[0].winParticle.Play();
            StartCoroutine(BlinkText(turnCard1Panel, textCard1, 8, "승리!", 3f));
        }
        else{
            card[1].winParticle.Play();
            StartCoroutine(BlinkText(turnCard2Panel, textCard2, 8, "승리!", 3f));
        }
    }
    private void ShowThrowPanel(){
        if(isCard1FirstStart){
            StartCoroutine(BlinkText(turnCard1Panel, textCard1, 3, "주사위를 굴리세요", 0.3f));
        }
        else{
            StartCoroutine(BlinkText(turnCard2Panel, textCard2, 3, "주사위를 굴리세요", 0.3f));
        }
    }
    IEnumerator BlinkText(GameObject turnCard, TextMeshProUGUI textCard, int blinkCount, string txt, float disableSec){
        turnCard.SetActive(true);
        for(int i = 0; i < blinkCount; i++){
            textCard.text = "";
            yield return new WaitForSeconds(0.15f);
            textCard.text = txt;
            yield return new WaitForSeconds(0.25f);
        }
        Invoke("DisablePanel", disableSec);
        yield break;
    }

    public void DisablePanel(){  //invoke
        isThrowingDice = false;
        turnCard1Panel.SetActive(false);
        turnCard2Panel.SetActive(false);
        textCard1.text = "";
        textCard2.text = "";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    public GameMap gameMap;
    private bool isGamePlayed;

    public enum enemy{
        default_,
        highDamage,
        increaseEnemyHealth,
        dicreasePlayerDamage,
        target
    }

    [SerializeField]private TextMeshProUGUI hpTxt;
    [SerializeField]private TextMeshProUGUI DamageTxt;
    [SerializeField]private Slider hpBar;
    [SerializeField]private Slider DamageBar;

    private int playerHp = 100;
    private int playerDamage = 30;

    public bool isIncreaseEnemyHealth;

    private void Start() {   //test
        GameStart();
    }

    public void GameStart(){
        if(isGamePlayed){
            //이미 게임을 한번 실행한 적이 있으면 리셋
            gameMap.Reset();
        }
        isGamePlayed = true;

        gameMap.CreateMap();  //맵 생성

        PlayerStatus();
    }

    private void PlayerStatus(){
        hpTxt.text = playerHp.ToString();
        DamageTxt.text = playerDamage.ToString();

        hpBar.value = playerHp/100f;
        DamageBar.value = playerDamage/30f;

        if(playerHp<=0){
            GameOver();
        }
    }

    public void DicreasePlayerDamage(bool enable){
        if(enable){
            playerDamage = 15;
        }
        else{
            playerDamage = 30;
        }
        PlayerStatus();
    }

    private void IncreaseEnemyHealth(){
        if(isIncreaseEnemyHealth){
            foreach(var enemyInfo in gameMap.currentSpawnEnemyList){
                enemyInfo.hp += 15;
                enemyInfo.ShowHp();
            }
        }
    }

    public void DotOnClick(){
        GameObject thisDot = EventSystem.current.currentSelectedGameObject;

        EnemyInfo thisEnemy = thisDot.GetComponentInChildren<EnemyInfo>();
        //해당 점에 적이 존재할 때
        if(thisEnemy != null && thisEnemy.gameObject.activeSelf){
            playerHp -= thisEnemy.damage;
            thisEnemy.hp -= playerDamage;

            PlayerStatus();
            thisEnemy.ShowHp();

            if(thisEnemy.hp<=0){
                gameMap.currentSpawnEnemyList.Remove(thisEnemy);
                Destroy(thisEnemy.gameObject);
            }
            else{
                return;
            }
        }

        foreach(EnemyInfo enemyInfo in gameMap.currentSpawnEnemyList){
            if(enemyInfo.enemyType == GameController.enemy.increaseEnemyHealth){
                isIncreaseEnemyHealth = true;
            }
        }

        IncreaseEnemyHealth();

        //해당 점에 아이템이 존재할 때




        //해당 점이 비었을 때
        int a = -99;
        for(int i = 1; i <= 6; i++){
            if(thisDot.transform.parent.name.Equals("Layout"+i)){
                a = i-1;
                break;
            }
        }
        int b = thisDot.transform.GetSiblingIndex();
        Debug.Log(a + ", " + b);
        int[] tmpPlayerPos = new int[] {a, b};

        gameMap.ShowPlayerPosAndLine(tmpPlayerPos);

        
    }

    private void GameOver(){

    }
}

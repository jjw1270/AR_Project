using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [SerializeField]GameMap gameMap;
    private bool isGamePlayed;

    public enum enemy{
        default_,
        highDamage,
        increaseEnemyHealth,
        dicreasePlayerDamage,
        target
    }

    private int playerHp = 100;
    private int playerDamage = 30;

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

        //플레이어, 적 정보 초기화
    }

    private void PlayerInfo(){

    }
}

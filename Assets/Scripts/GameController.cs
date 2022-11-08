using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dots{  //가로 행
    public DotInfo[] dot = new DotInfo[8];
}
[System.Serializable]
public struct DotInfo{
    public GameObject dot;
    public GameObject[] line;
}
public class GameController : MonoBehaviour
{
    [SerializeField]private GameObject startPoint;
    private int[] startPointPos;
    [SerializeField]private GameObject targetPoint;
    private int[] targetPointPos;
    public Dots[] array = new Dots[6];

    private void Start() {
        DisableRandomDots();
        DisableLine();
        SetStartPoint();
        SetTargetPoint();
        AddEnemys();
        AddItems();
        RandomEnemyOrItem();
    }

    public void DisableRandomDots(){
        //각 행마다 하나의 랜덤한 점 제거(or not)
        for(int i = 0; i < 6; i++){
            if(Random.Range(0,2) == 1){
                array[i].dot[Random.Range(0,8)].dot.SetActive(false);
            }
        }
    }

    public void DisableLine(){
        //점이 없으면 연결된 선 disable
        for(int i = 0; i < 6; i++){
            switch(i){
                case 0:
                    for(int j = 0; j < 7; j++){
                        if(!array[i].dot[j].dot.activeSelf){
                            if(j==0) continue;

                            array[i].dot[j-1].line[1].SetActive(false);
                            array[i+1].dot[j-1].line[0].SetActive(false);
                        }
                    }
                    break;
                case 1:
                case 3:
                    for(int j = 0; j < 7; j++){
                        if(!array[i].dot[j].dot.activeSelf){
                            if(j==0){
                                array[i-1].dot[j].line[2].SetActive(false);
                                array[i+1].dot[j].line[0].SetActive(false);
                                continue;
                            }
                            array[i-1].dot[j].line[2].SetActive(false);
                            array[i].dot[j-1].line[1].SetActive(false);
                            array[i+1].dot[j].line[0].SetActive(false);
                        }
                    }
                    break;
                case 2:
                case 4:
                    for(int j = 0; j < 7; j++){
                        if(!array[i].dot[j].dot.activeSelf){
                            if(j==0) continue;

                            array[i-1].dot[j-1].line[2].SetActive(false);
                            array[i].dot[j-1].line[1].SetActive(false);
                            array[i+1].dot[j-1].line[0].SetActive(false);
                        }
                    }
                    break;
                case 5:
                    for(int j = 0; j < 7; j++){
                        if(!array[i].dot[j].dot.activeSelf){
                            if(j==0){
                                array[i-1].dot[j].line[2].SetActive(false);
                                continue;
                            }

                            array[i-1].dot[j].line[2].SetActive(false);
                            array[i].dot[j-1].line[1].SetActive(false);
                        }
                    }
                    break;
            }
        }
        
    }

    private void SetStartPoint(){
        //네 모퉁이 중 한곳 랜덤
        int randomPoint = Random.Range(0,4);  //0 오른쪽위 부터 시계방향 부터 4 좌측 위 까지

        switch(randomPoint){
            case 0:
                if(!array[0].dot[7].dot.activeSelf){
                    startPoint = array[0].dot[6].dot;
                    startPointPos = new int[]{0,6};
                    break;
                }
                startPoint = array[0].dot[7].dot;
                startPointPos = new int[]{0,7};
                break;
            case 1:
                if(!array[5].dot[7].dot.activeSelf){
                    startPoint = array[5].dot[6].dot;
                    startPointPos = new int[]{5,6};
                    break;
                }
                startPoint = array[5].dot[7].dot;
                startPointPos = new int[]{5,7};
                break;
            case 2:
                if(!array[5].dot[0].dot.activeSelf){
                    startPoint = array[5].dot[1].dot;
                    startPointPos = new int[]{5,1};
                    break;
                }
                startPoint = array[5].dot[0].dot;
                startPointPos = new int[]{5,0};
                break;
            case 3:
                if(!array[0].dot[0].dot.activeSelf){
                    startPoint = array[0].dot[1].dot;
                    startPointPos = new int[]{0,1};
                    break;
                }
                startPoint = array[0].dot[0].dot;
                startPointPos = new int[]{0,0};
                break;
        }
        
    }

    private void SetTargetPoint(){
        //startPoint 반경 4~5칸 뒤에서 랜덤
        //목표의 체력 80 공격력 10
        //목표 제거시 보상 및 해당 게임 종료
        if(startPointPos[0] == 0){  //0 or 5
            targetPointPos = new int[]{Random.Range(4,6), Random.Range(0, 7)};
        }
        else{
            targetPointPos = new int[]{Random.Range(0,2), Random.Range(0, 7)};
        }

        if(!array[targetPointPos[0]].dot[targetPointPos[1]].dot.activeSelf){  //해당 점이 없으면 재귀
            SetStartPoint();
        }

        targetPoint = array[targetPointPos[0]].dot[targetPointPos[1]].dot;
    }

    private void AddEnemys(){
        //startPoint, targetPoint, 비활성화된 점을 제외하고 랜덤생성.
        //적의 종류는 4가지 : 체력90 공격력20(기본), 체력50 공격력 40(기본),
        //체력80 공격력 10(자신을 제외한 현재 존재하는 적의 체력 턴마다 +20),
        //체력40 공격력 15(존재하는 동안 플레이어의 공격력이 15로 고정됨)
        //적은 최소 한 종류씩 모조건 생성, (기본)이 아닌 적은 최대 2개, 총 수량 10개(테스트 후 수정 필요)
    }

    private void AddItems(){
        //startPoint, targetPoint, enemy위치, 비활성화된 점을 제외하고 랜덤생성.
        //아이템 종류는 4가지 : hp회복, 받는 데미지 2회 감소, 사용시 적 체력 50% 감소, 사용시 적 체력 3턴동안 30씩 감소
        //아이템은 최소 한 종류씩 무조건 생성, 총 수량 6개(테스트 후 수정 필요)

    }

    private void RandomEnemyOrItem(){
        //해당 지역으로 이동 시 적 또는 아이템이 3:7의 확률로 생성됨
        //총 수량 4개
    }

    private void Reset(){
        for(int i = 0; i < 6; i++){
            switch(i){
                    case 0:
                        for(int j = 0; j < 7; j++){
                            array[i].dot[j].line[1].SetActive(true);
                            array[i].dot[j].line[2].SetActive(true);
                        }
                        break;
                    case 1:
                    case 3:
                        for(int j = 0; j < 7; j++){
                            if(j==6){
                                array[i].dot[j].line[0].SetActive(true);
                                array[i].dot[j].line[2].SetActive(true);
                                continue;
                            }
                            array[i].dot[j].line[0].SetActive(true);
                            array[i].dot[j].line[1].SetActive(true);
                            array[i].dot[j].line[2].SetActive(true);
                        }
                        break;
                    case 2:
                    case 4:
                        for(int j = 0; j < 7; j++){
                            array[i].dot[j].line[0].SetActive(true);
                            array[i].dot[j].line[1].SetActive(true);
                            array[i].dot[j].line[2].SetActive(true);
                        }
                        break;
                    case 5:
                        for(int j = 0; j < 7; j++){
                            if(j==6){
                                array[i].dot[j].line[0].SetActive(true);
                                continue;
                            }
                            array[i].dot[j].line[0].SetActive(true);
                            array[i].dot[j].line[1].SetActive(true);
                        }
                        break;
                }
        }
    }
}

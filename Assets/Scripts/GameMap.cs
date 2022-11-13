using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*미완*/
[System.Serializable]
public class Dots{  //가로 행
    public DotInfo[] dot = new DotInfo[8];
}
[System.Serializable]
public struct DotInfo{
    public Button dot;
    public Image[] line;
}
public class GameMap : MonoBehaviour
{    
    public Dots[] array = new Dots[6];

    [SerializeField]private Sprite selectedDot;
    [SerializeField]private Color defaultLineColor;
    [SerializeField]private Color selectedColor;
    [SerializeField]private Color highLightColor;

    [SerializeField]private GameObject enemy_target;
    [SerializeField]private GameObject enemy_default;
    [SerializeField]private GameObject enemy_highDamage;
    [SerializeField]private GameObject enemy_increaseEnemyHealth;
    [SerializeField]private GameObject enemy_DecreasePlayerDamage;

    public int[] playerPos;

    public int[] enemy_target_Pos;

    public List<EnemyInfo> currentSpawnEnemyList = new List<EnemyInfo>();

    public void CreateMap(){
        DisableRandomDots();
        DisableLine();
        SetStartPoint();
        SetTargetPoint();
        AddEnemys();
        AddItems();
        ShowPlayerPosAndLine(playerPos);
    }
    public void Reset(){
        
    }



    private void DisableRandomDots(){
        //각 행마다 하나의 랜덤한 점 제거(or not)
        for(int i = 0; i < 6; i++){
            if(Random.Range(0,2) == 1){
                array[i].dot[Random.Range(0,7)].dot.gameObject.SetActive(false);
            }
        }
    }

    private void DisableLine(){
        //점이 없으면 연결된 선 disable
        for(int i = 0; i < 6; i++){
            switch(i){
                case 0:
                    for(int j = 0; j < 7; j++){
                        if(!array[i].dot[j].dot.gameObject.activeSelf){
                            if(j==0) continue;

                            array[i].dot[j-1].line[1].gameObject.SetActive(false);
                            array[i+1].dot[j-1].line[0].gameObject.SetActive(false);
                        }
                    }
                    break;
                case 1:
                case 3:
                    for(int j = 0; j < 7; j++){
                        if(!array[i].dot[j].dot.gameObject.activeSelf){
                            if(j==0){
                                array[i-1].dot[j].line[2].gameObject.SetActive(false);
                                array[i+1].dot[j].line[0].gameObject.SetActive(false);
                                continue;
                            }
                            array[i-1].dot[j].line[2].gameObject.SetActive(false);
                            array[i].dot[j-1].line[1].gameObject.SetActive(false);
                            array[i+1].dot[j].line[0].gameObject.SetActive(false);
                        }
                    }
                    break;
                case 2:
                case 4:
                    for(int j = 0; j < 7; j++){
                        if(!array[i].dot[j].dot.gameObject.activeSelf){
                            if(j==0) continue;

                            array[i-1].dot[j-1].line[2].gameObject.SetActive(false);
                            array[i].dot[j-1].line[1].gameObject.SetActive(false);
                            array[i+1].dot[j-1].line[0].gameObject.SetActive(false);
                        }
                    }
                    break;
                case 5:
                    for(int j = 0; j < 7; j++){
                        if(!array[i].dot[j].dot.gameObject.activeSelf){
                            if(j==0){
                                array[i-1].dot[j].line[2].gameObject.SetActive(false);
                                continue;
                            }

                            array[i-1].dot[j].line[2].gameObject.SetActive(false);
                            array[i].dot[j-1].line[1].gameObject.SetActive(false);
                        }
                    }
                    break;
            }
        }
        
    }

    private void SetStartPoint(){
        //네 모퉁이 중 한곳 랜덤
        int randomPoint = Random.Range(0,4);  //0 오른쪽위 부터 시계방향 부터 4 좌측 위 까지
        int[] tmpPlayerPos = new int[2];

        switch(randomPoint){
            case 0:
                if(!array[0].dot[7].dot.gameObject.activeSelf){
                    tmpPlayerPos = new int[]{0,6};
                    break;
                }
                tmpPlayerPos = new int[]{0,7};
                break;
            case 1:
                if(!array[5].dot[7].dot.gameObject.activeSelf){
                    tmpPlayerPos = new int[]{5,6};
                    break;
                }
                tmpPlayerPos = new int[]{5,7};
                break;
            case 2:
                if(!array[5].dot[0].dot.gameObject.activeSelf){
                    tmpPlayerPos = new int[]{5,1};
                    break;
                }
                tmpPlayerPos = new int[]{5,0};
                break;
            case 3:
                if(!array[0].dot[0].dot.gameObject.activeSelf){
                    tmpPlayerPos = new int[]{0,1};
                    break;
                }
                tmpPlayerPos = new int[]{0,0};
                break;
        }

        playerPos = tmpPlayerPos;
        Debug.Log("PlayerPOS : " + playerPos[0] + playerPos[1]);
    }

    private void SetTargetPoint(){
        //startPoint 반경 4~5칸 뒤에서 랜덤
        //목표의 체력 80 공격력 10
        //목표 제거시 보상 및 해당 게임 종료
        if(playerPos[0] == 0){  //0 or 5
            enemy_target_Pos = new int[]{Random.Range(4,6), Random.Range(0, 7)};
        }
        else{
            enemy_target_Pos = new int[]{Random.Range(0,2), Random.Range(0, 7)};
        }

        if(!array[enemy_target_Pos[0]].dot[enemy_target_Pos[1]].dot.gameObject.activeSelf){  //해당 점이 없으면 재귀
            SetTargetPoint();
        }

        GameObject targetPoint = array[enemy_target_Pos[0]].dot[enemy_target_Pos[1]].dot.gameObject;

        GameObject spawned_enemy_target = Instantiate(enemy_target, (Vector2)targetPoint.transform.position, Quaternion.identity, targetPoint.transform);
        spawned_enemy_target.SetActive(false);
    }

    private void AddEnemys(){
        //startPoint, targetPoint, 비활성화된 점을 제외하고 랜덤생성.
        //적의 종류는 4가지 : 체력90 공격력20(기본), 체력50 공격력 40(기본),
        //체력80 공격력 10(자신을 제외한 현재 존재하는 적의 체력 턴마다 +20),
        //체력40 공격력 15(존재하는 동안 플레이어의 공격력이 15로 고정됨)
        //적은 최소 한 종류씩 모조건 생성, 최소 6개(enemy_default*2, enemy_highDamage*2), 최대 10개(테스트 후 수정 필요)

        //적이 발견되면 적이 생성된 점을 둘러싼 선과 점 접근 불가능 표시
        //적을 파괴하면 다시 원래상태로

        for(int i = 0; i < Random.Range(1,3);){  //enemy_DecreasePlayerDamage
            i++;
            int[] tmpPos = new int[]{Random.Range(0,6), Random.Range(0, 8)};
            if((tmpPos[0] % 2 != 0 && tmpPos[1] > 5) || !array[tmpPos[0]].dot[tmpPos[1]].dot.gameObject.activeSelf ||
                tmpPos == playerPos || tmpPos == enemy_target_Pos || (array[tmpPos[0]].dot[tmpPos[1]].dot.transform.childCount > 3)){
                i--;
                continue;
            }

            GameObject enemyPoint = array[tmpPos[0]].dot[tmpPos[1]].dot.gameObject;


            GameObject spawned_enemy = Instantiate(enemy_DecreasePlayerDamage, (Vector2)enemyPoint.transform.position, Quaternion.identity, enemyPoint.transform);
            spawned_enemy.SetActive(false);Debug.Log("TMPPOS : " + tmpPos[0] + tmpPos[1]);
        }
        for(int i = 0; i < Random.Range(1,3);){  //enemy_increaseEnemyHealth
            i++;
            int[] tmpPos = new int[]{Random.Range(0,6), Random.Range(0, 8)};
            if((tmpPos[0] % 2 != 0 && tmpPos[1] > 5) || !array[tmpPos[0]].dot[tmpPos[1]].dot.gameObject.activeSelf ||
                tmpPos == playerPos || tmpPos == enemy_target_Pos || (array[tmpPos[0]].dot[tmpPos[1]].dot.transform.childCount > 3)){
                i--;
                continue;
            }

            GameObject enemyPoint = array[tmpPos[0]].dot[tmpPos[1]].dot.gameObject;


            GameObject spawned_enemy = Instantiate(enemy_increaseEnemyHealth, (Vector2)enemyPoint.transform.position, Quaternion.identity, enemyPoint.transform);
            spawned_enemy.SetActive(false);Debug.Log("TMPPOS : " + tmpPos[0] + tmpPos[1]);
        }

        for(int i = 0; i < Random.Range(2,4);){  //enemy_highDamage
            i++;
            int[] tmpPos = new int[]{Random.Range(0,6), Random.Range(0, 8)};
            if((tmpPos[0] % 2 != 0 && tmpPos[1] > 5) || !array[tmpPos[0]].dot[tmpPos[1]].dot.gameObject.activeSelf ||
                tmpPos == playerPos || tmpPos == enemy_target_Pos || (array[tmpPos[0]].dot[tmpPos[1]].dot.transform.childCount > 3)){
                i--;
                continue;
            }
            
            GameObject enemyPoint = array[tmpPos[0]].dot[tmpPos[1]].dot.gameObject;


            GameObject spawned_enemy = Instantiate(enemy_highDamage, (Vector2)enemyPoint.transform.position, Quaternion.identity, enemyPoint.transform);
            spawned_enemy.SetActive(false);Debug.Log("TMPPOS : " + tmpPos[0] + tmpPos[1]);
        }
        for(int i = 0; i < Random.Range(2,4);){  //enemy_default
            i++;
            int[] tmpPos = new int[]{Random.Range(0,6), Random.Range(0, 8)};
            if((tmpPos[0] % 2 != 0 && tmpPos[1] > 5) || !array[tmpPos[0]].dot[tmpPos[1]].dot.gameObject.activeSelf ||
                tmpPos == playerPos || tmpPos == enemy_target_Pos || (array[tmpPos[0]].dot[tmpPos[1]].dot.transform.childCount > 3)){
                i--;
                continue;
            }

            GameObject enemyPoint = array[tmpPos[0]].dot[tmpPos[1]].dot.gameObject;


            GameObject spawned_enemy = Instantiate(enemy_default, (Vector2)enemyPoint.transform.position, Quaternion.identity, enemyPoint.transform);
            spawned_enemy.SetActive(false);Debug.Log("TMPPOS : " + tmpPos[0] + tmpPos[1]);
        }

    }

    private void AddItems(){
        //startPoint, targetPoint, enemy위치, 비활성화된 점을 제외하고 랜덤생성.
        //아이템 종류는 4가지 : hp회복, 받는 데미지 2회 감소, 사용시 적 체력 50% 감소, 사용시 적 체력 3턴동안 30씩 감소
        //아이템은 최소 한 종류씩 무조건 생성, 총 수량 6개(테스트 후 수정 필요)

    }

    public void ShowPlayerPosAndLine(int[] tmpPlayerPos){
        //현재 플레이어의 위치 표시
        //현재 플레이어의 위치에서 나아갈 수 있는 선과 점 하이라이트
        //나아갈 수 있는 점에 적이 존재하면, 그 점의 적 활성화, 주변의 선에 못간다고 표시

///////////////////////////
//지나갈 수 있는 경로 및 점 표시
        //해당 점에서 라인
        foreach(var line in array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]].line){
            if(!line.gameObject.activeSelf) continue;
            if(line.color.Equals(selectedColor)) continue;
            line.color = highLightColor;
        }
        //갈 수 있는 점에서 라인
        if(tmpPlayerPos[0] % 2 == 0){  //짝수 행
            if(tmpPlayerPos[1] > 0){
                if(tmpPlayerPos[0] > 0){
                    array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]-1].line[2].color = highLightColor;
                }
                array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]-1].line[1].color = highLightColor;
                array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]-1].line[0].color = highLightColor;
            }
        }
        else{    //홀수 행
            if(tmpPlayerPos[1] > 0){
                array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]-1].line[1].color = highLightColor;
            }
            if(tmpPlayerPos[0] < 5){
                array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]].line[0].color = highLightColor;
            }
            array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]].line[2].color = highLightColor;
        }

        //점
        //지나갈 수 있는 점에 적이 있으면 적 활성화
        if(tmpPlayerPos[0] % 2 == 0){  //짝수 행
            if(tmpPlayerPos[1]<7){
                if(tmpPlayerPos[0]>0){
                    array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]].dot.interactable = true;
                    ShowEnemyOrItem(new int[]{tmpPlayerPos[0]-1, tmpPlayerPos[1]});
                }
                array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]+1].dot.interactable = true;
                ShowEnemyOrItem(new int[]{tmpPlayerPos[0], tmpPlayerPos[1]+1});
                array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]].dot.interactable = true;
                ShowEnemyOrItem(new int[]{tmpPlayerPos[0]+1, tmpPlayerPos[1]});
            }
            if(tmpPlayerPos[1]>0){
                array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]-1].dot.interactable = true;
                ShowEnemyOrItem(new int[]{tmpPlayerPos[0]+1, tmpPlayerPos[1]-1});
                array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]-1].dot.interactable = true;
                ShowEnemyOrItem(new int[]{tmpPlayerPos[0], tmpPlayerPos[1]-1});
                if(tmpPlayerPos[0]>0){
                    array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]-1].dot.interactable = true;
                    ShowEnemyOrItem(new int[]{tmpPlayerPos[0]-1, tmpPlayerPos[1]-1});
                }
            }
        }
        else{    //홀수 행
            array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]+1].dot.interactable = true;
            ShowEnemyOrItem(new int[]{tmpPlayerPos[0]-1, tmpPlayerPos[1]+1});
            if(tmpPlayerPos[1]<6){
                array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]+1].dot.interactable = true;
                ShowEnemyOrItem(new int[]{tmpPlayerPos[0], tmpPlayerPos[1]+1});
            }
            if(tmpPlayerPos[0]<5){
                array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]+1].dot.interactable = true;
                ShowEnemyOrItem(new int[]{tmpPlayerPos[0]+1, tmpPlayerPos[1]+1});
                array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]].dot.interactable = true;
                ShowEnemyOrItem(new int[]{tmpPlayerPos[0]+1, tmpPlayerPos[1]});
            }
            if(tmpPlayerPos[1]>0){
                array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]-1].dot.interactable = true;
                ShowEnemyOrItem(new int[]{tmpPlayerPos[0], tmpPlayerPos[1]-1});
            }
            array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]].dot.interactable = true;
            ShowEnemyOrItem(new int[]{tmpPlayerPos[0]-1, tmpPlayerPos[1]});
        }


///////////////////////////
//지나간 경로 및 점 표시
        //주변에 이미 선택된 점이 있으면 두 점사이를 연결
        if(tmpPlayerPos[0] % 2 == 0){  //짝수 행
            if(tmpPlayerPos[1]<7){
                if(tmpPlayerPos[0]>0 && array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]].dot.gameObject.activeSelf && !array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]].dot.enabled){
                    array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]].line[0].color = selectedColor;
                }
                if(array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]+1].dot.gameObject.activeSelf && !array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]+1].dot.enabled){
                    array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]].line[1].color = selectedColor;
                }
                if(array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]].dot.gameObject.activeSelf && !array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]].dot.enabled){
                    array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]].line[2].color = selectedColor;
                }
            }
            if(tmpPlayerPos[1]>0){
                if(array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]-1].dot.gameObject.activeSelf && !array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]-1].dot.enabled){
                    array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]-1].line[0].color = selectedColor;
                }
                if(array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]-1].dot.gameObject.activeSelf && !array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]-1].dot.enabled){
                    array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]-1].line[1].color = selectedColor;
                }
                if(tmpPlayerPos[0]>0 && array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]-1].dot.gameObject.activeSelf && !array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]-1].dot.enabled){
                    array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]-1].line[2].color = selectedColor;
                }
            }
        }
        else{
            if(array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]+1].dot.gameObject.activeSelf && !array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]+1].dot.enabled){
                array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]].line[0].color = selectedColor;
            }
            if(tmpPlayerPos[1]<6 && array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]+1].dot.gameObject.activeSelf && !array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]+1].dot.enabled){
                array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]].line[1].color = selectedColor;
            }
            if(tmpPlayerPos[0]<5){
                if(array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]+1].dot.gameObject.activeSelf && !array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]+1].dot.enabled){
                    array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]].line[2].color = selectedColor;
                }

                if(array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]].dot.gameObject.activeSelf && !array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]].dot.enabled){
                    array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]].line[0].color = selectedColor;
                }
            }
            if(tmpPlayerPos[1]>0 && array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]-1].dot.gameObject.activeSelf && !array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]-1].dot.enabled){
                array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]-1].line[1].color = selectedColor;
            }
            if(array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]].dot.gameObject.activeSelf && !array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]].dot.enabled){
                    array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]].line[2].color = selectedColor;
            }
        }

        //선택한 점 표시
        array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]].dot.enabled = false;
        Image thisDotImage = array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]].dot.GetComponent<Image>();
        thisDotImage.sprite = selectedDot;
        thisDotImage.color = selectedColor;
       
        
        playerPos = tmpPlayerPos;
        Debug.Log("선택한 점 : " + playerPos[0] + ", " + playerPos[1]);
    }

    private void ShowEnemyOrItem(int[] tmpPlayerPos){
        if(array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]].dot.transform.childCount <= 3){
            return;
        }

        GameObject addedObj = array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]].dot.transform.GetChild(3).gameObject;
        EnemyInfo thisEnemy = addedObj.transform.GetComponent<EnemyInfo>();
        if(thisEnemy != null){
            
            thisEnemy.gameObject.SetActive(true);

            //선
            foreach(var line in array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]].line){
                line.color = defaultLineColor;
            }
            if(tmpPlayerPos[0] % 2 == 0){  //짝수 행
                if(tmpPlayerPos[1] > 0){
                    if(tmpPlayerPos[0] > 0){
                        array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]-1].line[2].color = defaultLineColor;
                    }
                    array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]-1].line[1].color = defaultLineColor;
                    array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]-1].line[0].color = defaultLineColor;
                }
            }
            else{    //홀수 행
                if(tmpPlayerPos[1] > 0){
                    array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]-1].line[1].color = defaultLineColor;
                }
                if(tmpPlayerPos[0] < 5){
                    array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]].line[0].color = defaultLineColor;
                }
                array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]].line[2].color = defaultLineColor;
            }

            //점
            //적 반경 1칸은 playerpos 기준으로 라인 비활성화
            if(tmpPlayerPos[0] % 2 == 0){  //짝수 행
                if(tmpPlayerPos[1]<7){
                    if(tmpPlayerPos[0]>0){
                        array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]].dot.interactable = false;
                    }
                    array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]+1].dot.interactable = false;
                    array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]].dot.interactable = false;
                }
                if(tmpPlayerPos[1]>0){
                    array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]-1].dot.interactable = false;
                    array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]-1].dot.interactable = false;
                    if(tmpPlayerPos[0]>0){
                        array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]-1].dot.interactable = false;
                    }
                }
            }
            else{    //홀수 행
                array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]+1].dot.interactable = false;
                if(tmpPlayerPos[1]<6){
                    array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]+1].dot.interactable = false;
                }
                if(tmpPlayerPos[0]<5){
                    array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]+1].dot.interactable = false;
                    array[tmpPlayerPos[0]+1].dot[tmpPlayerPos[1]].dot.interactable = false;
                }
                if(tmpPlayerPos[1]>0){
                    array[tmpPlayerPos[0]].dot[tmpPlayerPos[1]-1].dot.interactable = false;
                }
                array[tmpPlayerPos[0]-1].dot[tmpPlayerPos[1]].dot.interactable = false;
            }



            return;
        }
        else{
            ItemInfo thisItem = addedObj.transform.GetComponent<ItemInfo>();
            /////
            return;
        }
    }

}

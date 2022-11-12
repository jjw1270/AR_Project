using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyInfo : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI hpTxt;
    [SerializeField]private TextMeshProUGUI DamageTxt;
    
    public int hp;
    public int damage;
    public GameController.enemy enemyType;

    private void Start() {
        hpTxt.text = hp.ToString();
        DamageTxt.text = damage.ToString();
    }

    public void ShowHp(){
        hpTxt.text = hp.ToString();
    }

    private void OnEnable() {
        if(enemyType == GameController.enemy.dicreasePlayerDamage){
            GameController.Instance.DicreasePlayerDamage(true);
        }
    }

    private void OnDisable() {
        if(enemyType == GameController.enemy.dicreasePlayerDamage){
            GameController.Instance.DicreasePlayerDamage(false);
        }
        else if(enemyType == GameController.enemy.increaseEnemyHealth){
            GameController.Instance.isIncreaseEnemyHealth = false;
        }
    }


}

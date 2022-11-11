using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyInfo : MonoBehaviour
{
    public int hp;
    public int damage;
    [SerializeField]private TextMeshProUGUI hpTxt;
    [SerializeField]private TextMeshProUGUI DamageTxt;
    public GameController.enemy enemyType;

    private void Start() {
        hpTxt.text = hp.ToString();
        DamageTxt.text = damage.ToString();
    }

    
}

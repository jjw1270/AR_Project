using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public SpeechToText speechToText;
    public GameController gameController;
    [SerializeField]VirtualBtnCtrl virtualBtnCtrl;

    public TextMeshProUGUI logText;

    public bool[] targetFoundList = new bool[3];
    public bool isAllTargetFound;
    bool isGameStart;

    private void Start() {
        StartCoroutine(ShowLogo());
    }

    IEnumerator ShowLogo(){
        
        yield return new WaitForSeconds(2f);
    }

    public void Stt(string command){
        virtualBtnCtrl.ChangeBtnColor(false);
        switch(command){
            case "게임 설명":
                if(!isGameStart)
                    SoundManager.Instance.textToSpeech.PlayTTS("GameInfo");
                else
                    logText.text = "게임 도중 게임 설명을 들을 수 없습니다.";
                break;
            case "게임 시작":
                //모든 마커가 인식완료된 상태에서만 가능
                if(isAllTargetFound){
                    if(!isGameStart){
                        isGameStart = true;
                        SoundManager.Instance.PlayBGMSound();
                        gameController.OnGameStart();
                    }
                }
                else{
                    SoundManager.Instance.textToSpeech.PlayTTS("MarkerFirst");
                    logText.text = "모든 마커를 인식해 주세요.";
                    Invoke("HelpLogText", 2f);
                }
                break;
            case "어플 종료":
                SoundManager.Instance.textToSpeech.PlayTTS("QuitApp");
                Invoke("QuitApplication", 2f);
                break;
            case "던져":
                if(isGameStart){
                    if(!gameController.isThrowingDice){
                        gameController.ThrowDice();
                    }
                    else{
                        logText.text = "조금 뒤에 던져주세요";
                        Invoke("HelpLogText", 2f);
                    }
                }
                else{
                    SoundManager.Instance.textToSpeech.PlayTTS("StartGameFirst");
                    logText.text = "게임을 시작해 주세요";
                    Invoke("HelpLogText", 2f);
                }
                
                break;
            case "게임 재시작":
                SoundManager.Instance.textToSpeech.PlayTTS("RestartGame");
                Invoke("RestartGame", 2f);
                break;
            default:
                logText.text = "잘못된 명령입니다.";
                Invoke("HelpLogText", 2f);
                break;
        }
    }

    private void Update() {
#if UNITY_EDITOR
        if(Input.GetMouseButtonDown(0)){
            Stt("던져");
        }
        if(Input.GetMouseButtonDown(1)){
            Stt("게임 설명");
        }
#endif
        if(targetFoundList[0] && targetFoundList[1] && targetFoundList[2]){
            isAllTargetFound = true;
        }
        else{
            isAllTargetFound = false;
        }
    }

    private void HelpLogText(){
        logText.text = "음성인식 명령어 : '게임 설명', '게임 시작', '던져', '게임 재시작', '어플 종료'";
    }

    private void RestartGame(){
        SceneManager.LoadScene("MainScene");
    }

    private void QuitApplication(){
        Application.Quit();
    }
}

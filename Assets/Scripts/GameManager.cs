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
    bool onMarkerDownload;

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
                onMarkerDownload = false;
                if(!isGameStart)
                    SoundManager.Instance.textToSpeech.PlayTTS("GameInfo");
                else
                    logText.text = "게임 도중 게임 설명을 들을 수 없습니다.";
                    Invoke("HelpLogText", 2f);
                break;
            case "게임 시작":
                onMarkerDownload = false;
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
            case "주사위 굴려":
            case "굴려":
            case "주사위":
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
            // case "게임 재시작":   //초기화 해줘야함. 안할래
            //     SoundManager.Instance.textToSpeech.PlayTTS("RestartGame");
            //     Invoke("RestartGame", 2f);
            //     break;
            case "마커 다운로드":
            case "마크 다운로드":
                if(!isGameStart){
                    onMarkerDownload = true;
                    SoundManager.Instance.textToSpeech.PlayTTS("MarkerDown");
                    Invoke("MarkerRecording", 2f);
                }
                else{
                    logText.text = "게임 도중 실행할 수 없습니다.";
                    Invoke("HelpLogText", 2f);
                }
                break;
            case "예":
            case "네":
            case "응":
            case "어":
                if(onMarkerDownload)
                    Application.OpenURL("https://drive.google.com/file/d/17hSE8wAjlWecN9X243PcrHSfO61GYnyk/view?usp=sharing");
                break;
            case "아니요":
            case "아니":
            case "싫어":
                if(onMarkerDownload)
                    onMarkerDownload = false;
                break;
            default:
                logText.text = "잘못된 명령입니다.";
                Invoke("HelpLogText", 2f);
                break;
        }
    }

    private void MarkerRecording(){  //invoke
        speechToText.StartRecording();
    }

    public void MarkerDownBtn(){
        Application.OpenURL("https://drive.google.com/file/d/17hSE8wAjlWecN9X243PcrHSfO61GYnyk/view?usp=sharing");
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

    private void HelpLogText(){   //invoke
        logText.text = "음성인식 명령어 : '게임 설명', '마커 다운로드', '게임 시작', '주사위 굴려', '어플 종료'";
        speechToText.resultText.text = "";
    }

    private void RestartGame(){
        SceneManager.LoadScene("MainScene");
    }

    private void QuitApplication(){
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]TextToSpeech textToSpeech;
    void Start()
    {

    }

    void Update()
    {
        
    }

    public void CommandExecution(string result){
        switch(result){
            case "안녕":
                textToSpeech.PlayTTS("");
                break;
            case "도움말":
                textToSpeech.PlayTTS("");
                break;
        }
    }
}

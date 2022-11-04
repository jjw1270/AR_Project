using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
엄밀히 말하면 tts는 아니지만,
typecast 에서 tts로 변환한 wav파일을 가져와 사용한다.
audio clip을 구조체로 저장하고, 필요에 맞게 불러와 사용한다.
오딘 인스펙터로 딕셔너리를 seriallize하여 사용하고 싶었지만 유료라 기각..
*/
public class TextToSpeech : MonoBehaviour
{
    public AudioSource ttsAudioSource;
    [System.Serializable]
    public struct TTS{
        public string name;
        public AudioClip clip;
    }
    public TTS[] ttsList;

    public void PlayTTS(string ttsName){
        foreach(TTS thisTTS in ttsList){
            if(thisTTS.name.Equals(ttsName)){
                ttsAudioSource.clip = thisTTS.clip;
                ttsAudioSource.Play();
                break;
            }
        }
    }

}

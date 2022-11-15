using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] GameObject BGM;
    [SerializeField] GameObject SFX;
    public TextToSpeech textToSpeech;
    private AudioSource bgmPlayer;
    private AudioSource sfxPlayer;

    public float masterVolumeBGM = 1f;
    public float masterVolumeSFX = 1f;

    [SerializeField]
    private AudioClip mainBgmAudioClip;   //mainScene bgm
    
    
    [SerializeField]
    private AudioClip[] sfxAudioClips;    //soundEffect 
    //soundEffect Dictionary
    Dictionary<string, AudioClip> sfxAudioClipsDic = new Dictionary<string, AudioClip>();


    protected override void Awake() {
        bgmPlayer = BGM.GetComponent<AudioSource>();
        
        sfxPlayer = SFX.GetComponent<AudioSource>();
        foreach(AudioClip audioClip in sfxAudioClips){
            sfxAudioClipsDic.Add(audioClip.name, audioClip);
        }

    }

    public void PlayBGMSound(float volume = 0.5f){
        bgmPlayer.loop = true;  //loop
        bgmPlayer.clip = mainBgmAudioClip;
        bgmPlayer.Play();
        bgmPlayer.volume = volume * masterVolumeBGM;
    }//bgm(volume(option))
    
    public void PlaySFXSound(string name, float volume = 1f){
        if(sfxAudioClipsDic.ContainsKey(name) == false){
            Debug.Log(name + " is not contained.");
            return;
        }
        sfxPlayer.PlayOneShot(sfxAudioClipsDic[name], volume * masterVolumeSFX);
    }//sfx(name, volume(option))
}

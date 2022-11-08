using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] GameObject BGM;
    [SerializeField] GameObject SFX;
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

    // public void PlayBGMSound(float volume = 0.8f){
    //     bgmPlayer.loop = true;  //loop
        
    //     if(SceneManager.GetActiveScene().name == "MainScene"){
    //         if(bgmPlayer.clip.name != mainBgmAudioClip.name){
    //             bgmPlayer.clip = mainBgmAudioClip;
    //             bgmPlayer.Play();
    //         }
    //     }
    //     else if(SceneManager.GetActiveScene().name == "GameScene"){
    //         volume = 0.2f;
    //         if(GameManager.instance.gameData.isBossStage){   //boss
    //             volume = 0.3f;
    //             bgmPlayer.clip = BossBgmAudioClip;
    //             bgmPlayer.Play();
    //         }
    //         else{
    //             //desert normal
    //             if(bgmPlayer.clip.name != DesertNormalBgmAudioClip.name){
    //                 bgmPlayer.clip = DesertNormalBgmAudioClip;
    //                 bgmPlayer.Play();
    //             }
    //         }
            
    //     }

    //     bgmPlayer.volume = volume * masterVolumeBGM;
    // }//bgm(name, volume(option))
    
    public void PlaySFXSound(string name, float volume = 1f){
        if(sfxAudioClipsDic.ContainsKey(name) == false){
            Debug.Log(name + " is not contained.");
            return;
        }
        sfxPlayer.PlayOneShot(sfxAudioClipsDic[name], volume * masterVolumeSFX);
    }//soundEffect(name, volume(option))

    // void OnEnable(){ //델리게이트 체인 추가
    //     SceneManager.sceneLoaded += OnSceneLoaded;
    // }
    // void OnSceneLoaded(Scene scene, LoadSceneMode mode){
    //     Debug.Log("OnSceneLoaded: " + scene.name);
    //     //Debug.Log(mode);
    //     //PlayBGMSound();
    // }
    // void OnDisable(){  //델리게이트 체인 제거
    //     Debug.Log("OnDisable");
    //     SceneManager.sceneLoaded -= OnSceneLoaded;
    // }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    [HideInInspector] public FMOD.Studio.EventInstance musicInstance;

    private void Awake() {
        if (!instance) {
            instance = this;
        } else {
            GameObject.DestroyImmediate(this.gameObject);
        }
        GameObject.DontDestroyOnLoad(this);
    }

    private void Start() {
        PlayMusic(GlobalVariables.MUSIC_MAIN_MENU);
    }

    public static void PlayOneShot(string eventName, GameObject obj) {
        FMOD.Studio.EventInstance eventInstance = FMODUnity.RuntimeManager.CreateInstance(eventName);
        if (obj != null) {
            eventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(obj));
        }
        eventInstance.start();
        eventInstance.release();
    }

    public static void PlayOneShot(string eventName) {
        AudioManager.PlayOneShot(eventName, null);
    }

    public static void PlayMusic(string eventName) {
        if (AudioManager.IsPlaying(AudioManager.instance.musicInstance)) {
            Debug.LogWarning("Music is already playing");
            return;
        }

        AudioManager.instance.musicInstance = FMODUnity.RuntimeManager.CreateInstance(eventName);
        AudioManager.instance.musicInstance.start();
    }

    public static void StopMusic() {
        if (!AudioManager.IsPlaying(AudioManager.instance.musicInstance)) {
            Debug.LogWarning("Music is not playing");
            return;
        }

        AudioManager.instance.musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AudioManager.instance.musicInstance.release();
    }

    public static bool IsPlaying(FMOD.Studio.EventInstance eventInstance) {
	FMOD.Studio.PLAYBACK_STATE state;   
	eventInstance.getPlaybackState(out state);
	return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
}
}

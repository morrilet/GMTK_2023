using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    [HideInInspector] public Dictionary<string, FMOD.Studio.EventInstance> soundInstances;
    [HideInInspector] public Dictionary<string, FMOD.Studio.EventInstance> musicInstances;

    private void Awake() {
        if (!instance) {
            instance = this;
        } else {
            GameObject.DestroyImmediate(this.gameObject);
        }
        GameObject.DontDestroyOnLoad(this);

        musicInstances = new Dictionary<string, FMOD.Studio.EventInstance>() {};
    }

    private void Start() {
        PlayMusic(GlobalVariables.MUSIC_MAIN_MENU);
        PlayOneShot(GlobalVariables.SOUNDSCAPE_PARK_AMBIENCE);
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
        AudioManager.instance.musicInstances[eventName] = FMODUnity.RuntimeManager.CreateInstance(eventName);

        if (AudioManager.IsPlaying(eventName)) {
            Debug.LogWarning("Music clip is already playing");
            return;
        }

        AudioManager.instance.musicInstances[eventName] = FMODUnity.RuntimeManager.CreateInstance(eventName);
        AudioManager.instance.musicInstances[eventName].start();
    }

    public static void StopMusic(string eventName) {
        if (!AudioManager.IsPlaying(eventName)) {
            Debug.LogWarning("Music clip is not playing");
            return;
        }

        AudioManager.instance.musicInstances[eventName].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AudioManager.instance.musicInstances[eventName].release();
        AudioManager.instance.musicInstances.Remove(eventName);
    }

    public static void ToggleSnapshot(string eventName) {
        
    }

    public static bool IsPlaying(string eventName) {
        if (!AudioManager.instance.musicInstances.ContainsKey(eventName)) return false;
        return AudioManager.IsPlaying(AudioManager.instance.musicInstances[eventName]);
    }

    public static bool IsPlaying(FMOD.Studio.EventInstance eventInstance) {
        FMOD.Studio.PLAYBACK_STATE state;   
        eventInstance.getPlaybackState(out state);
        return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
    }
}

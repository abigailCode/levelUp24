using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoBehaviour {
    public static AudioManager instance;
    public AudioSource sfxSource;
    public AudioSource musicSource;

    public Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> musicClips = new Dictionary<string, AudioClip>();

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSFXClips();
            LoadMusicClips();
        } else Destroy(gameObject);
    }

    void LoadSFXClips() {
        sfxClips["GetGem"] = Resources.Load<AudioClip>("SFX/getGem");
    }

    void LoadMusicClips() {
        musicClips["0"] = Resources.Load<AudioClip>("Music/BackgroundMusic_1");
        musicClips["1"] = Resources.Load<AudioClip>("Music/BackgroundMusic_2");
        musicClips["2"] = Resources.Load<AudioClip>("Music/BackgroundMusic_3");
        musicClips["3"] = Resources.Load<AudioClip>("Music/BackgroundMusic_4");
        musicClips["4"] = Resources.Load<AudioClip>("Music/BackgroundMusic_5");
    }

    public void PlaySFX(string clipName) {
        if (sfxClips.ContainsKey(clipName)) {
            sfxSource.clip = sfxClips[clipName];
            sfxSource.Play();
        } else Debug.LogWarning("El AudioClip " + clipName + " no se encontró en el diccionario de sfxClips.");
    }

    public void PlayMusic(string clipName) {
        if (musicClips.ContainsKey(clipName)) {
            musicSource.clip = musicClips[clipName];
            musicSource.Play();
        } else Debug.LogWarning("El AudioClip " + clipName + " no se encontró en el diccionario de musicClips.");
    }

    public void ChangeVolume(float value) {
        sfxSource.volume = value;
        musicSource.volume = value;
    }
}
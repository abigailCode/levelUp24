using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoBehaviour {
    public static AudioManager instance;
    [SerializeField] AudioSource _sfxSource;
    [SerializeField] AudioSource _musicSource;

    Dictionary<string, AudioClip> _sfxClips = new();
    Dictionary<string, AudioClip> _musicClips = new();

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSFXClips();
            LoadMusicClips();
        } else Destroy(gameObject);
    }

    void LoadSFXClips() {
        _sfxClips["GetGem"] = Resources.Load<AudioClip>("SFX/getGem");
    }

    void LoadMusicClips() {
        _musicClips["0"] = Resources.Load<AudioClip>("Music/BackgroundMusic_1");
        _musicClips["1"] = Resources.Load<AudioClip>("Music/BackgroundMusic_2");
        _musicClips["2"] = Resources.Load<AudioClip>("Music/BackgroundMusic_3");
        _musicClips["3"] = Resources.Load<AudioClip>("Music/BackgroundMusic_4");
        _musicClips["4"] = Resources.Load<AudioClip>("Music/BackgroundMusic_5");
    }

    public void PlaySFX(string clipName) {
        if (_sfxClips.ContainsKey(clipName)) {
            _sfxSource.clip = _sfxClips[clipName];
            _sfxSource.Play();
        } else Debug.LogWarning("El AudioClip " + clipName + " no se encontró en el diccionario de sfxClips.");
    }

    public void PlayMusic(string clipName) {
        if (_musicClips.ContainsKey(clipName)) {
            _musicSource.clip = _musicClips[clipName];
            _musicSource.Play();
        } else Debug.LogWarning("El AudioClip " + clipName + " no se encontró en el diccionario de musicClips.");
    }

    public void ChangeVolume(float value) {
        _sfxSource.volume = value;
        _musicSource.volume = value;
    }
}
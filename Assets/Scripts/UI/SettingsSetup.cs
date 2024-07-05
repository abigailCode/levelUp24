using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSetup : MonoBehaviour {
     void Start() {
        
        float savedVolume = PlayerPrefs.GetFloat("BackgroundMusicVolume", 0.72f);
        
        AudioManager.Instance.ChangeVolume(savedVolume);
        
        float savedSFX = PlayerPrefs.GetFloat("SFXVolume", 0.72f);
        AudioManager.Instance.ChangeSFXVolume(savedSFX);

        string savedLanguage = PlayerPrefs.GetString("Language", "en");
       // LocalizationManager.Instance.LoadLocalizedText(savedLanguage);

        Debug.Log("volume: " + savedVolume + " sfx: " + savedSFX + " language: " + savedLanguage);
    }
}

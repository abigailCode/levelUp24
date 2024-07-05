using TMPro;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.Core.Parsing;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {
    [SerializeField] Slider volumeSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] TMP_Dropdown languageSelector;


    private float savedVolume;
    private float selectedVolume;

    private float savedSFX;
    private float selectedSFX;

    private string selectedLanguage;
    private string savedLanguage;

    void Start() {
        savedVolume = PlayerPrefs.GetFloat("BackgroundMusicVolume", 0.72f);
        savedSFX = PlayerPrefs.GetFloat("SFXVolume", 0.72f);
        savedLanguage = PlayerPrefs.GetString("Language", "English");

        selectedVolume = savedVolume;
        selectedSFX = savedSFX;
        selectedLanguage = savedLanguage;

        LoadVolume(savedVolume);
        LoadSFX(savedSFX);
        LoadLanguage(savedLanguage);
    }
    void LoadVolume(float volume) {
        Debug.Log("volume: " + volume);
        volumeSlider.value = volume;
    }

    void LoadSFX(float sfx) {
        Debug.Log("sfx: " + sfx);
        sfxSlider.value = sfx;
    }

    void LoadLanguage(string language) {
        Debug.Log("language: " + language);
        languageSelector.value = languageSelector.options.FindIndex(option => option.text == language);
    }
  

    public void ChangeVolume() {
        selectedVolume = volumeSlider.value;
        AudioManager.Instance.ChangeVolume(volumeSlider.value);
        PlayerPrefs.SetFloat("BackgroundMusicVolume", selectedVolume);
    }

    public void ChangeSFX() {
        selectedSFX = sfxSlider.value;
        AudioManager.Instance.ChangeSFXVolume(sfxSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", selectedSFX);
    }

    public void ChangeLanguage() {
        selectedLanguage = languageSelector.options[languageSelector.value].text;
        PlayerPrefs.SetString("Language", selectedLanguage);
    }

    public void SaveSettings() {
        PlayerPrefs.Save();
    }
}
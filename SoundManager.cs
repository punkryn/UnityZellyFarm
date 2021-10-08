using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgmPlayer;
    public AudioSource sfxPlayer;
    public AudioClip[] audioClips;

    // Slider
    public Slider sfxSlider;
    public Slider bgmSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("SfxSound"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SfxSound");
        }

        if (PlayerPrefs.HasKey("BgmSound"))
        {
            bgmSlider.value = PlayerPrefs.GetFloat("BgmSound");
        }
    }

    public void OnSliderValueChanged()
    {
        PlayerPrefs.SetFloat("SfxSound", sfxSlider.value);
        sfxPlayer.volume = sfxSlider.value;
    }

    public void OnBgmSliderValueChanged()
    {
        PlayerPrefs.SetFloat("BgmSound", bgmSlider.value);
        bgmPlayer.volume = bgmSlider.value;
    }

    public void PlayClip(string type)
    {
        int idx;
        switch (type)
        {
            case "Touch":
                idx = 8;
                break;
            case "Grow":
                idx = 4;
                break;
            case "Sell":
                idx = 7;
                break;
            case "Buy":
                idx = 1;
                break;
            case "Unlock":
                idx = 9;
                break;
            case "Fail":
                idx = 3;
                break;
            case "Button":
                idx = 0;
                break;
            case "Pause In":
                idx = 5;
                break;
            case "Pause Out":
                idx = 6;
                break;
            case "Clear":
                idx = 2;
                break;
            default:
                idx = -1;
                break;
        }

        sfxPlayer.clip = audioClips[idx];
        sfxPlayer.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Для работы со слайдерами

public class AudioManager : MonoBehaviour
{
    public AudioSource[] sfx; // Массив звуковых эффектов
    public AudioSource[] bgm; // Массив фоновой музыки

    public static AudioManager instance;

    [Header("Volume Controls")]
    public Slider sfxSlider; // Слайдер для управления громкостью эффектов
    public Slider bgmSlider; // Слайдер для управления громкостью музыки

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(this.gameObject);

        // Устанавливаем начальные значения громкости
        if (sfxSlider != null)
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        if (bgmSlider != null)
            bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);

        UpdateSFXVolume();
        UpdateBGMVolume();
    }
    void Update()
    {
        // Обновляем громкость в реальном времени, если слайдеры изменяются
        if (sfxSlider != null)
            UpdateSFXVolume();

        if (bgmSlider != null)
            UpdateBGMVolume();
    }

    public void PlaySFX(int soundToPlay)
    {
        if (soundToPlay < sfx.Length)
        {
            sfx[soundToPlay].Play();
        }
    }

    public void PlayBGM(int musicToPlay)
    {
        if (!bgm[musicToPlay].isPlaying)
        {
            StopMusic();

            if (musicToPlay < bgm.Length)
            {
                bgm[musicToPlay].Play();
            }
        }
    }

    public void StopMusic()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    // Обновляет громкость звуковых эффектов
    public void UpdateSFXVolume()
    {
        float volume = sfxSlider.value;
        for (int i = 0; i < sfx.Length; i++)
        {
            sfx[i].volume = volume;
        }
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    // Обновляет громкость фоновой музыки
    public void UpdateBGMVolume()
    {
        float volume = bgmSlider.value;
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].volume = volume;
        }
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }
}

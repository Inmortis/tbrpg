using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    public Dropdown resolutionDropdown; // Ссылка на Dropdown
    private List<Resolution> popularResolutions; // Список популярных разрешений

    void Start()
    {
        // Задаём список популярных разрешений
        popularResolutions = new List<Resolution>
        {
            new Resolution { width = 1280, height = 720, refreshRate = 60 },   // HD
            new Resolution { width = 1920, height = 1080, refreshRate = 60 },  // Full HD
            new Resolution { width = 2560, height = 1440, refreshRate = 60 },  // QHD
            new Resolution { width = 3840, height = 2160, refreshRate = 60 }   // 4K
        };

        // Очищаем старые варианты в Dropdown
        resolutionDropdown.ClearOptions();

        // Создаём список строк для отображения в Dropdown
        List<string> options = new List<string>();

        for (int i = 0; i < popularResolutions.Count; i++)
        {
            Resolution res = popularResolutions[i];
            // Формируем строку вида: "1920 x 1080 @ 60Hz"
            string option = $"{res.width} x {res.height} @ {res.refreshRate}Hz";
            options.Add(option);
        }

        // Добавляем варианты в Dropdown
        resolutionDropdown.AddOptions(options);

        // Устанавливаем текущий выбор (первый элемент)
        resolutionDropdown.value = 0;
        UpdateDropdownLabel(resolutionDropdown.value);

        // Добавляем слушатель для изменения выбора
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetResolution(int resolutionIndex)
    {
        // Устанавливаем выбранное разрешение
        Resolution selectedResolution = popularResolutions[resolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);

        // Обновляем текст Label в Dropdown
        UpdateDropdownLabel(resolutionIndex);
    }

    private void UpdateDropdownLabel(int resolutionIndex)
    {
        if (resolutionIndex >= 0 && resolutionIndex < popularResolutions.Count)
        {
            Resolution res = popularResolutions[resolutionIndex];
            // Обновляем текст в самом Dropdown
            resolutionDropdown.captionText.text = $"{res.width} x {res.height} @ {res.refreshRate}Hz";
        }
    }
}

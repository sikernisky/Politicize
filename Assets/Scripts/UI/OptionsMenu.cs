using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    ///<summary>Slider that controls music volume.</summary>
    private Slider musicVolume;

    [SerializeField]
    ///<summary>Slider that controls SFX volume.</summary>
    private Slider sfxVolume;

    [SerializeField]
    ///<summary>Sprite to represent the main menu button on this menu. </summary>
    private Sprite mainMenuSprite;

    [SerializeField]
    ///<summary>Sprite to represent the exit button on this menu. </summary>
    private Sprite exitSprite;

    [SerializeField]
    ///<summary>Image component of the exit button. </summary>
    private Image exitButtonImage;


    private void Start()
    {
        SetupSoundVolume();
        HideOptionsMenu();
        SetExitButton();
    }

    private void Update()
    {
        UpdateMusicVolume();
        UpdateSFXVolume();
    }

    /// <summary>
    /// Updates the music volume according to its slider value.
    /// </summary>
    private void UpdateMusicVolume()
    {
        FindObjectOfType<AudioManager>().ChangeMusicVolume(musicVolume.value);
        SaveManager.data.musicVol = musicVolume.value;
    }

    /// <summary>
    /// Updates the SFX volume according to its slider value.
    /// </summary>
    private void UpdateSFXVolume()
    {
        FindObjectOfType<AudioManager>().ChangeSFXVolume(sfxVolume.value);
        SaveManager.data.sfxVol = sfxVolume.value;
    }

    /// <summary>
    /// Returns the player back to LevelSelect.
    /// </summary>
    public void ExitGame()
    {
        if (SceneManager.GetActiveScene().name == "LevelSelect")
        {
            FindObjectOfType<SceneChangeButton>().ChangeScene("MainMenu", true);
        }
        else if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            Application.Quit();
        }
        else FindObjectOfType<SceneChangeButton>().ChangeScene("LevelSelect", true);
    }

    /// <summary>
    /// Toggles Full Screen.
    /// </summary>
    public void ToggleFullScreen()
    {
        if (Screen.fullScreen) Screen.fullScreen = false;
        else Screen.fullScreen = true;
    }

    /// <summary>
    /// Hides the options menu.
    /// </summary>
    public void HideOptionsMenu()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Hides the options menu.
    /// </summary>
    public void ShowOptionsMenu()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Configures the sound volume based on player settings.
    /// </summary>
    public void SetupSoundVolume()
    {
        musicVolume.value = SaveManager.data.musicVol;
        sfxVolume.value = SaveManager.data.sfxVol;
        UpdateMusicVolume();
        UpdateSFXVolume();
    }

    /// <summary>
    /// Sets the image of the exit button depending on the scene.
    /// </summary>
    private void SetExitButton()
    {
        if (SceneManager.GetActiveScene().name == "LevelSelect") exitButtonImage.sprite = mainMenuSprite;
        else exitButtonImage.sprite = exitSprite;
    }




}

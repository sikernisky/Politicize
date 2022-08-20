using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// A class to represent buttons on menus.
/// </summary>
public class SceneChangeButton: MonoBehaviour
{
    ///<summary>The Animator for the Image that fades in and out.</summary>
    private Animator fader;

    [SerializeField]
    ///<summary>Whether this SceneChangeButton should cause fading or not.</summary>
    private bool fade = true;


    private void Start()
    {
        fader = FindObjectOfType<Fader>().GetComponent<Animator>();
    }

    /// <summary>
    /// Changes the scene to <c>sceneName</c>.
    /// </summary>
    public void ChangeScene(string sceneName, bool quick = false)
    {
        if (fade && !quick) StartCoroutine(ChangeSceneFade(sceneName));
        else if (fade && quick) StartCoroutine(QuickChangeSceneFade(sceneName));
        else SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Takes the player to LevelSelect.
    /// </summary>
    public void LevelSelect()
    {
        ChangeScene("LevelSelect", true);
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator ChangeSceneFade(string sceneName)
    {
        yield return new WaitForSeconds(1f);
        fader.SetTrigger("fade");
        yield return new WaitForSeconds(1.75f);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator QuickChangeSceneFade(string sceneName)
    {
        fader.SetTrigger("fade");
        yield return new WaitForSeconds(1.75f);
        SceneManager.LoadScene(sceneName);
    }
}



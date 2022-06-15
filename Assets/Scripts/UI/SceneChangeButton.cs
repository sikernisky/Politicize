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
    [SerializeField]
    ///<summary>The Animator for the Image that fades in and out.</summary>
    private Animator fader;

    [SerializeField]
    ///<summary>Whether this SceneChangeButton should cause fading or not.</summary>
    private bool fade = true;


    /// <summary>
    /// Changes the scene to <c>sceneName</c>.
    /// </summary>
    public void ChangeScene(string sceneName)
    {
        if (fade) StartCoroutine(ChangeSceneFade(sceneName));
        else SceneManager.LoadScene(sceneName);
    }

    IEnumerator ChangeSceneFade(string sceneName)
    {
        fader.SetTrigger("fade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
}

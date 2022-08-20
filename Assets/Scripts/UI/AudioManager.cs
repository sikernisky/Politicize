using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;


/// <summary>
/// Class to handle sounds.
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    ///<summary>The sounds to play. </summary>
    private Sound[] sounds;

    [SerializeField]
    ///<summary>The music tracks to play.</summary>
    private Song[] mainMenuMusic;

    [SerializeField]
    ///<summary>The music tracks to play.</summary>
    private Song[] arnolicaMusic;

    [SerializeField]
    ///<summary>The music tracks to play.</summary>
    private Song[] foliardMusic;

    [SerializeField]
    ///<summary>The music tracks to play.</summary>
    private Song[] xatesMusic;

    /// <summary>All music tracks. </summary>
    private List<Song[]> allMusic;

    /// <summary>The last faction the player was in.</summary>
    private string lastFaction;

    /// <summary>The AudioManager across all scenes.</summary>
    private static IEnumerator musicCoro;

    /// <summary>The Song currently playing.</summary>
    private Song songPlaying;




    private void Awake()
    {
        SetupMusic();
        foreach(Song[] musicTrack in allMusic) { AddSources(musicTrack); }
        AddSources(sounds);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Adds all music to a list.
    /// </summary>
    private void SetupMusic()
    {
        allMusic = new List<Song[]>();
        allMusic.Add(mainMenuMusic);
        allMusic.Add(arnolicaMusic);
        allMusic.Add(foliardMusic);
        allMusic.Add(xatesMusic);
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode m)
    {
        PlayFactionMusic();
    }

    /// <summary>
    /// Adds an AudioSource component to each Sound in <c>clips</c>.
    /// </summary>
    /// <param name="clips">The array of Sounds to add AudioSources to.</param>
    private void AddSources(Sound[] clips)
    {
        foreach(Sound s in clips)
        {
            s.SetupSound(gameObject.AddComponent<AudioSource>());
        }
    }

    /// <summary>
    /// Plays the current faction's music.
    /// </summary>
    public void PlayFactionMusic()
    {
        if (GetFaction() == lastFaction) return;
        if (musicCoro != null) StopCoroutine(musicCoro);
        if (songPlaying != null) songPlaying.Source().Stop();
        musicCoro = StartMusicRotation();
        StartCoroutine(musicCoro);
        lastFaction = GetFaction();
    }


    private IEnumerator StartMusicRotation()
    {
        Queue<Song> tracksToPlay = new Queue<Song>(MusicToPlay(GetFaction()));
        while (true)
        {
            Song currentSong = tracksToPlay.Dequeue();
            songPlaying = currentSong;
            currentSong.Source().Play();
            yield return new WaitForSeconds(currentSong.Length());
            tracksToPlay.Enqueue(currentSong);
            yield return null;
        }
    }

    /// <summary>
    /// Plays a sound.
    /// </summary>
    /// <param name="soundName">The name of the sound to play.</param>
    public void Play(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.SoundName() == soundName);
        if (s == null) return;
        s.Source().Play();
    }

    /// <summary>
    /// Returns true if a sound is playing.
    /// </summary>
    /// <param name="soundName">The name of the sound to check.</param>
    /// <returns>true if the sound is playing, false otherwise.</returns>
    public bool Playing(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.SoundName() == soundName);
        return s == null || s.Source().isPlaying;
    }

    private Song[] MusicToPlay(string faction)
    {
        if (faction == "MainMenu") return mainMenuMusic;
        if (faction == "Arnolica") return arnolicaMusic;
        if (faction == "Foliard") return foliardMusic;
        if (faction == "Xates") return xatesMusic;
        return arnolicaMusic; //backup
    }

    /// <summary>
    /// Changes music volume.
    /// </summary>
    /// <param name="val">The value (0 <= val <= 1) to change the music volume to.</param>
    public void ChangeMusicVolume(float val)
    {
        foreach(Song[] musicTrack in allMusic)
        {
            foreach(Song s in musicTrack)
            {
                s.AdjustVolume(val);
            }
        }
    }

    /// <summary>
    /// Changes SFX volume.
    /// </summary>
    /// <param name="val">The value (0 <= val <= 1) to change the SFX volume to.</param>
    public void ChangeSFXVolume(float val)
    {
        foreach(Sound s in sounds)
        {
            s.AdjustVolume(val);
        }
    }

    /// <summary>
    /// Gets the current faction.
    /// </summary>
    private string GetFaction()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "MainMenu" || sceneName == "LevelSelect") return "MainMenu";
        else if (sceneName.Contains("Arnolica")) return "Arnolica";
        else if (sceneName.Contains("Foliard")) return "Foliard";
        else if (sceneName.Contains("Xates")) return "Xates";
        else if (sceneName.Contains("Gorneo")) return "Gorneo";
        return "MainMenu";
    }


}

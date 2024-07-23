using System;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManger : MonoBehaviour
{
    public static SoundManger Instance {  get; private set; }

    public bool IsSoundEnabled { get; private set; }
    public bool IsMusicEnabled { get; private set; }

    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private GameObject _soundPrefab;

    public AudioClip Kick;
    public AudioClip MissionComplete;
    public AudioClip Saved;
    public AudioClip Missed;
    public AudioClip Upgrade;
    public AudioClip Defender;

    private void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        IsSoundEnabled = Convert.ToBoolean(PlayerPrefs.GetInt("IsSoundEnabled", 1));
        IsMusicEnabled = Convert.ToBoolean(PlayerPrefs.GetInt("IsMusicEnabled", 0));
        UpdateSound();
    }
    private void UpdateSound()
    {
        _audioMixer.SetFloat("Music", -80 * Convert.ToInt32(!IsMusicEnabled));
        _audioMixer.SetFloat("Sound", -80 * Convert.ToInt32(!IsSoundEnabled));
    }
    public void SoundToggle()
    {
        IsSoundEnabled = !IsSoundEnabled;
        UpdateSound();
        Save();
    }
    public void MusicToggle()
    { 
        IsMusicEnabled = !IsMusicEnabled;
        UpdateSound();
        Save();
    }
    public void PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        var sound = Instantiate(_soundPrefab).GetComponent<Sound>();
        sound.Play(clip, volume, pitch);
        Destroy(sound.gameObject, clip.length + 0.1f);

    }
    private void Save()
    {
        PlayerPrefs.SetInt("IsSoundEnabled", Convert.ToInt32(IsSoundEnabled));
        PlayerPrefs.SetInt("IsMusicEnabled", Convert.ToInt32(IsMusicEnabled));
    }
}

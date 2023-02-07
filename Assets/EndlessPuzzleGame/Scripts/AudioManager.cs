using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance = null;

    [Header ("Audio Sources")]
    public AudioSource efxSource;
    public AudioSource musicSource;

    [Header ("Background Music")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    [Header("Sound Effects")]
    public AudioClip buttonClick;
    public AudioClip gameOver;
    public AudioClip jump;

    private bool muteMusic;
    private bool muteEfx;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start ()
    {
        muteMusic = PlayerPrefs.GetInt("MuteMusic") == 1 ? true : false;
        muteEfx = PlayerPrefs.GetInt("MuteEfx") == 1 ? true : false;

        PlayMusic(menuMusic);
    }
	
    public void PlayMusic(AudioClip clip)
    {
        if (muteMusic)
            return;

        musicSource.clip = clip;
        if (!musicSource.isPlaying)
            musicSource.Play();
    }

    private void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayEffects(AudioClip clip)
    {
        if (muteEfx)
            return;
        
        efxSource.PlayOneShot(clip);
    }

    public void MuteMusic()
    {
        if (muteMusic)
        {
            muteMusic = false;
            PlayMusic(menuMusic);
            PlayerPrefs.SetInt("MuteMusic", 0);
        }
        else
        {
            muteMusic = true;
            StopMusic();
            PlayerPrefs.SetInt("MuteMusic", 1);
        }
    }

    public void MuteEfx()
    {
        if (muteEfx)
            PlayerPrefs.SetInt("MuteEfx", 0);
        else
            PlayerPrefs.SetInt("MuteEfx", 1);

        muteEfx = !muteEfx;
    }

    public bool IsMusicMute()
    {
        return muteMusic;
    }

    public bool IsEfxMute()
    {
        return muteEfx;
    }
}

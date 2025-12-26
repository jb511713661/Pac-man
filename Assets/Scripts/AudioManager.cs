using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("BGM")]
    public AudioSource musicSource;       // An AudioSource bound to the same object
    public AudioClip bgmIntro;            // Level opening BGM
    public AudioClip bgmGhostsNormal;     // Normal status BGM
    public AudioClip bgmGhostsScared;     // Scared Status BGM
    public AudioClip bgmGhostsDead;       // Dead Status BGM

    [Header("SFX")]
    public AudioSource sfxSource;         // Create a second AudioSource or reuse a 3D/2D SFX channel
    public AudioClip sfxMove;             // Move (no beans)
    public AudioClip sfxEatPellet;        // Eat common beans
    public AudioClip sfxHitWall;          // Hitting the wall
    public AudioClip sfxDeath;            // PacStudent Death

    void Reset()
    {
        // Automatically tries to grab the first AudioSource on the same object
        if (!musicSource) musicSource = GetComponent<AudioSource>();
        if (!sfxSource)
        {
            // Create a separate AudioSource for SFX (same object)
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
            sfxSource.loop = false;
        }
    }

    void Start()
    {
        if (!musicSource || !bgmIntro || !bgmGhostsNormal)
        {
            Debug.LogError("[AudioManager] Missing references (musicSource / bgmIntro / bgmGhostsNormal).");
            return;
        }

        // Play the intro first, then switch to normal background music after the intro ends or 3 seconds later (whichever comes first)
        musicSource.loop = false;
        musicSource.clip = bgmIntro;
        musicSource.Play();

        float wait = Mathf.Min(bgmIntro.length, 3f);
        Invoke(nameof(PlayGhostsNormalBGM), wait);
    }

    // BGM switching API (can be called later by game status)
    public void PlayGhostsNormalBGM()
    {
        if (!bgmGhostsNormal) return;
        musicSource.loop = true;
        musicSource.clip = bgmGhostsNormal;
        musicSource.Play();
    }

    public void PlayGhostsScaredBGM()
    {
        if (!bgmGhostsScared) return;
        musicSource.loop = true;
        musicSource.clip = bgmGhostsScared;
        musicSource.Play();
    }

    public void PlayGhostsDeadBGM()
    {
        if (!bgmGhostsDead) return;
        musicSource.loop = true;
        musicSource.clip = bgmGhostsDead;
        musicSource.Play();
    }

    // SFX trigger API (can be directly called in subsequent collision/pac-a-dot scripts)
    public void PlayMoveSFX() { if (sfxMove) sfxSource.PlayOneShot(sfxMove); }
    public void PlayEatPelletSFX() { if (sfxEatPellet) sfxSource.PlayOneShot(sfxEatPellet); }
    public void PlayHitWallSFX() { if (sfxHitWall) sfxSource.PlayOneShot(sfxHitWall); }
    public void PlayDeathSFX() { if (sfxDeath) sfxSource.PlayOneShot(sfxDeath); }
}


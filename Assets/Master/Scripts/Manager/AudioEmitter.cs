using UnityEngine;

public class AudioEmitter : MonoBehaviour
{
    private static AudioEmitter instance;

    [Tooltip("Audio Source for BGM")]
    [SerializeField] private AudioSource bgm;
    [Tooltip("Audio Source for SFX")]
    [SerializeField] private AudioSource sfx;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Play one shot clip, use this for SFX
    /// </summary>
    /// <param name="clip"></param>
    public static void PlayOneShot(AudioClip clip)
    {
        if (clip == null) return;

        instance.sfx.PlayOneShot(clip);
    }

    /// <summary>
    /// Stop the current music and play the new one
    /// </summary>
    /// <param name="clip"></param>
    public static void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        instance.bgm.Stop();
        instance.bgm.clip = clip;
        instance.bgm.Play();
    }
}
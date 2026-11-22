using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource musicSource, sfxSource;

    [SerializeField] private AudioClip timerAlert, footSteps, deliverToTruck, pickUpBox;

    private void Awake()
    {
        musicSource = GameObject.Find("MusicSource").GetComponent<AudioSource>();
        sfxSource = GameObject.Find("SFXSource").GetComponent<AudioSource>();
    }

    public void StopMusicAudio()
    {
        musicSource.Stop();
    }

    public void PlayMusicAudio()
    {
        musicSource.Play();
    }

    public void PlaySFXAudio(AudioClip audioClip)
    {
        sfxSource.PlayOneShot(audioClip);
    }

    public AudioClip GetAudioClip(AudioType audioType)
    {
        return audioType switch
        {
            AudioType.DeliverToTruck => deliverToTruck,
            AudioType.FootSteps => footSteps,
            AudioType.PickUpBox => pickUpBox,
            _ => timerAlert
        };
    }
}

public enum AudioType
{
    Timer,
    FootSteps,
    DeliverToTruck,
    PickUpBox
}
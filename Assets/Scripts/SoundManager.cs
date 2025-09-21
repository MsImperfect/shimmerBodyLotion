using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //SoundManager.Instance.PlaySound(SoundManager.Sound.<soundname>,transform.position)
    public enum Sound
    {
        LaserShot,
        PowerUp,
        Explosion,
    }
    public static SoundManager Instance { get; private set; }
    public SoundAudioClip[] soundAudioClips;

    private void Awake()
    {
        if (Instance != null && Instance == this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(Sound sound, Vector3 position)
    {
        GameObject soundGameObject = new GameObject("Sound");
        soundGameObject.transform.position = position;
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        AudioSource.PlayClipAtPoint(GetAudioClip(sound), position);
        Destroy(soundGameObject);
    }

    public void PlaySound(Sound sound)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    public static AudioClip GetAudioClip(Sound sound)
    {
        foreach (SoundAudioClip clip in SoundManager.Instance.soundAudioClips)
        {
            if (clip.sound == sound)
            {
                return clip.audioClip;
            }
        }
        return null;
    }

    [System.Serializable]
    public class SoundAudioClip
    {
        public Sound sound;
        public AudioClip audioClip;
    }
}

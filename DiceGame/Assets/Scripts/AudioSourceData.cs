using UnityEngine;

[CreateAssetMenu(fileName = "New AudioSource Data", menuName = "AudioSource Data", order = 51)]
public class AudioSourceData : ScriptableObject
{
    [System.Serializable]
    public class AudioClipEntry
    {
        public string clipName;
        public AudioClip clip;
    }

    public AudioClipEntry[] audioClips;
    public float volume = 1.0f;
    public float pitch = 1.0f;
    public bool loop = false;
    public bool playOnAwake = false;

    public void ApplyTo(AudioSource audioSource, string clipName)
    {
        AudioClip clipToPlay = FindClip(clipName);
        if (clipToPlay != null)
        {
            audioSource.clip = clipToPlay;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.loop = loop;
            audioSource.playOnAwake = playOnAwake;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Clip not found: " + clipName);
        }
    }

    private AudioClip FindClip(string clipName)
    {
        foreach (var entry in audioClips)
        {
            if (entry.clipName == clipName)
            {
                return entry.clip;
            }
        }
        return null;
    }
}
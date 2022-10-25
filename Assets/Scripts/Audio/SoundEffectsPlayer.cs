using UnityEngine;

public class SoundEffectsPlayer : MonoBehaviour
{
    public static AudioSource AudioSource { get; private set; }

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
        AudioSource.playOnAwake = false;
    }
}

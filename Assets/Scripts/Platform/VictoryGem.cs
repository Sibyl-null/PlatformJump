using UnityEngine;

public class VictoryGem : MonoBehaviour
{
    [SerializeField] private AudioClip pickUpSFX;
    [SerializeField] private ParticleSystem pickUpVFX;
    [SerializeField] private EventCenterVoid eventCenter;

    private void OnTriggerEnter(Collider other)
    {
        eventCenter.EventTrigger();
        SoundEffectsPlayer.AudioSource.PlayOneShot(pickUpSFX);
        Instantiate(pickUpVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

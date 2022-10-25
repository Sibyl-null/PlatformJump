using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGem : MonoBehaviour
{
    private Collider _collider;
    private MeshRenderer _meshRenderer;
    
    [SerializeField] private float resetTime = 5f;   //宝石重生时间
    [SerializeField] private AudioClip pickUpSFX;
    [SerializeField] private ParticleSystem pickUpVFX;
    
    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            controller.CanAirJump = true;

            DestroyGem();
            StartCoroutine(ResetCoroutine());
        }
    }

    private IEnumerator ResetCoroutine()
    {
        yield return YieldHelper.WaitForSeconds(resetTime);
        ResetGem();
    }

    private void DestroyGem()   //销毁宝石
    {
        _collider.enabled = false;
        _meshRenderer.enabled = false;
        
        SoundEffectsPlayer.AudioSource.PlayOneShot(pickUpSFX);
        Instantiate(pickUpVFX, transform.position, Quaternion.identity);
    }

    private void ResetGem()   //宝石重生
    {
        _collider.enabled = true;
        _meshRenderer.enabled = true;
    }
}

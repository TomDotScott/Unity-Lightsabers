using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsaber : MonoBehaviour
{
    private enum EState
    {
        activating, active, deactivating, deactive
    }

    [SerializeField] private AudioClip[] humSounds;

    [SerializeField] private AudioClip saberOn, saberOff;

    private AudioSource _audioSource;

    [SerializeField] EState currentState;

    [SerializeField] private float activationSpeed, deactivationSpeed;

    float scale = 0;

    GameObject blade;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        blade = transform.GetChild(0).gameObject;
        currentState = EState.deactive;
        blade.transform.localScale = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        switch (currentState)
        {
            case EState.activating:
                Activate();
                break;
            case EState.deactivating:
                Deactivate();
                break;
        }
        PlaySFX();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (currentState == EState.deactive)
            {
                currentState = EState.activating;
            }
            else if (currentState == EState.active)
            {
                currentState = EState.deactivating;
            }
        }
    }

    private void Activate()
    {
        blade.SetActive(true);
        if (blade.transform.localScale.x < 1)
        {
            scale += Time.deltaTime; 
            blade.transform.localScale = new Vector3(Mathf.Clamp(activationSpeed * scale, 0, 1),
                                                     Mathf.Clamp(activationSpeed * scale, 0, 1),
                                                     Mathf.Clamp(activationSpeed * scale, 0, 1));
        }
        else
        {
            currentState = EState.active;
        }
    }

    private void Deactivate()
    {
        if (blade.transform.localScale.x > 0)
        {
            scale -= Time.deltaTime;
            blade.transform.localScale = new Vector3(Mathf.Clamp(deactivationSpeed * scale, 0, 1),
                                                     Mathf.Clamp(deactivationSpeed * scale, 0, 1),
                                                     Mathf.Clamp(deactivationSpeed * scale, 0, 1));
        }
        else
        {
            currentState = EState.deactive;
            blade.SetActive(false);
        }
    }

    private void PlaySFX()
    {
        if (!_audioSource.isPlaying)
        {
            switch (currentState)
            {
                case EState.activating:
                    _audioSource.clip = saberOn;
                    _audioSource.Play();
                    break;
                case EState.active:
                    //_audioSource.clip = humSounds[Random.Range(0, humSounds.Length)];
                    //_audioSource.Play();
                    break;
                case EState.deactivating:
                    _audioSource.clip = saberOff;
                    _audioSource.Play();
                    break;
                case EState.deactive:
                    break;
            }
        }
    }
}
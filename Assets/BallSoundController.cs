using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://andrewmushel.com/articles/sound-effect-variation-in-unity/
public class BallSoundController : MonoBehaviour
{
    public AudioClip ballRollingLoop;

    [SerializeField]
    private float pitchMin = 0.8f;
    [SerializeField]
    private float pitchMax = 1.2f;
    [SerializeField]
    private float volumeMin = 0.8f;
    [SerializeField]
    private float volumeMax = 1.2f;
    [SerializeField]
    private float minVelocityMagnitude = 0.2f;

    private AudioSource _audioSource;
    private string laneTag = "Lane";
    private Rigidbody _rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = ballRollingLoop;

        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // TODO volume related to velocity magnitude
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag(laneTag))
        {
            if(_rigidbody.velocity.magnitude < minVelocityMagnitude)
            {
                if(_audioSource.isPlaying)
                {
                    _audioSource.Stop();
                }
                else
                {
                    return;
                }
            }

            if(!_audioSource.isPlaying)
            {
                _audioSource.pitch = Random.Range(pitchMin, pitchMax);
                // _audioSource.volume = Random.Range(volumeMin, volumeMax);
                _audioSource.volume = _rigidbody.velocity.magnitude;
                _audioSource.Play();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag(laneTag))
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
        }
    }
}

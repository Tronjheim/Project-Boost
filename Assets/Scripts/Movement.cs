using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 100;
    [SerializeField] float rotationThrust = 1f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem mainParticles;

    Rigidbody rb;
    AudioSource audio;

    void Start()
    {
       rb = GetComponent<Rigidbody>(); 
       audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }
    void ProcessThrust()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }

    }
    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audio.isPlaying)
        {
            audio.PlayOneShot(mainEngine);
        }
        if (!mainParticles.isPlaying)
        {
            mainParticles.Play();
        }
    }
    void StopThrusting()
    {
        audio.Stop();
        mainParticles.Stop();
    }

    void ProcessRotation()
    {
        if(Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rotationThrust);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rotationThrust);
        }
    }
    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freez to manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay =2f;
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audio;

    bool isTransitioning = false;
    bool collisionDisabled = false;
     void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    void Update()
    {
        DebugKey();
    }
    void DebugKey()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;//toggle collision
        }
    }
    private void OnCollisionEnter(Collision col)
    {
        if(isTransitioning || collisionDisabled)
        {
            return;
        }
        else
        {
            switch (col.gameObject.tag)
            {
                case "Friendly":
                    Debug.Log("is friendly");
                    break;
                case "Finish":
                    StartSuccessSequence();
                    break;
                default:
                    StartCrashSequence();
                    break;
            }
        }     
    }
    void StartSuccessSequence()
    {
        isTransitioning = true;
        audio.Stop();
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", levelLoadDelay);
        audio.PlayOneShot(success);
        successParticles.Play();
    }
    void StartCrashSequence()
    {
        isTransitioning = true;
        audio.Stop();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel",levelLoadDelay);
        audio.PlayOneShot(crash);
        crashParticles.Play();
    }
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

    }
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}

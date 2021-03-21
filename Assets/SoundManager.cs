using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioMixer mixer;

    public AudioClip[] ambients;

    public AudioClip dialogue;

    public AudioClip logic;

    public float volume = 0.1f;

    public float minVolume = 0.05f;

    private AudioSource audio;

    public static SoundManager Instance { get; private set; }

    private Coroutine coroutine;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.volume = volume;

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void PlayClip(AudioClip clip)
    {
        audio.Stop();
        audio.clip = clip;
        audio.Play();
    }

    public void PlayAmbient(int index)
    {
        PlayClip(ambients[index]);
    }

    public void PlayDialogue()
    {
        PlayClip (dialogue);
    }

    public void PlayLogic()
    {
        PlayClip (logic);
    }

    public void LowerVolumeForDialogue(float dialogueTime)
    {
        coroutine = StartCoroutine(TempLowerVolume(dialogueTime));
    }

    private IEnumerator TempLowerVolume(float duration)
    {
        audio.volume = minVolume;
        yield return new WaitForSeconds(duration + 0.5f);
        audio.volume = volume;
    }

    public void CancelVolumeCoroutine()
    {
        StopCoroutine (coroutine);
    }
}

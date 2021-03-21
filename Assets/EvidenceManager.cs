using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Valve.VR;

public enum Item
{
    Null,
    FirstLanaTalk,
    Drugs,
    Poison,
    Knife,
    Clipboard,
    Food
}

public class EvidenceManager : MonoBehaviour
{
    public AudioSource audio;

    public AudioClip missingEvidenceClip;

    public List<Item> items;

    public static EvidenceManager Instance { get; private set; }

    public UnityEvent[] triggers;

    // Start is called before the first frame update
    void Start()
    {
        items = new List<Item> { Item.Null };
        audio = GetComponent<AudioSource>();
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddEvidence(Item item)
    {
        if (!items.Contains(item))
        {
            items.Add (item);
            UpdateTriggers (item);
        }
    }

    public void OnPickup(EvidenceController evidence)
    {
        StartCoroutine(PickupEvidence(evidence));
    }

    IEnumerator PickupEvidence(EvidenceController evidence)
    {
        const int DELAY = 1;

        AddEvidence(evidence.item);

        audio.Stop();
        audio.clip = evidence.clip;
        audio.Play();

        SoundManager
            .Instance
            .LowerVolumeForDialogue(evidence.clip.length + DELAY);
        yield return new WaitForSeconds(evidence.clip.length + DELAY);
    }

    private void UpdateTriggers(Item justAdded)
    {
        switch (justAdded)
        {
            case Item.Knife:
                break;
        }
    }

    public void PlayMissingEvidence()
    {
        audio.Stop();
        audio.clip = missingEvidenceClip;
        audio.Play();

        SoundManager.Instance.LowerVolumeForDialogue(audio.clip.length);
    }

    public void EndGame(string accused)
    {
        //setAccused(accused); //TODO Implement accuse
        StartCoroutine(EndGame());
    }

    private IEnumerator EndGame()
    {
        SteamVR_Fade.View(Color.black, 4);

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene("CreditsScene");
        SteamVR_Fade.View(Color.clear, 4);
    }
}

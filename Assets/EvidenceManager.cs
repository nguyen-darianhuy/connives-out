using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum Item
{
    Null,
    FirstLanaTalk,
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

        audio.Stop();
        audio.clip = evidence.clip;
        audio.Play();

        yield return new WaitForSeconds(evidence.clip.length + DELAY);

        AddEvidence(evidence.item);
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
}

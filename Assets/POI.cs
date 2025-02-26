using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Valve.VR;

public class POI : MonoBehaviour
{
    public SteamVR_Action_Boolean talkAction;

    public SteamVR_Input_Sources handType;

    public AudioSource audio;

    public AudioClip[] dialogues;

    public string[] dialoguePrompts = { "Talk (A)" };

    public Item[] requiredTriggers;

    public Item[] unlockTriggers;

    public int[] nervousTriggers;

    public List<int> dialogueCheckpoints;

    public UnityEvent endEvent;

    private TextMesh text;

    private int dialogueCount;

    private int lastDialogueCheckpoint;

    private Animator anim;

    private bool playerInRange;

    private bool isTalking;

    private Coroutine coroutine;

    private float nervous;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMesh>();
        anim = GetComponent<Animator>();

        dialogueCount = 0;
        playerInRange = false;
        isTalking = false;
        lastDialogueCheckpoint = 0;
        nervous = 0f;

        text.gameObject.SetActive(false);
        if (dialogues.Length != dialoguePrompts.Length)
        {
            Debug
                .LogError("Dialogue lines & prompts not synced for " +
                gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && !isTalking && talkAction.GetState(handType))
        {
            isTalking = true;
            text.gameObject.SetActive(false);
            AdvanceDialogue();

            if (dialogueCount < nervousTriggers.Length)
            {
                nervous += nervousTriggers[dialogueCount] / 100f;
                nervousTriggers[dialogueCount] = 0;
            }
        }

        TurnTextToPlayer();
        anim.SetFloat("Nervous", nervous, 2f, Time.deltaTime);
    }

    private void AdvanceDialogue()
    {
        audio.clip = dialogues[dialogueCount];
        audio.Play();
        coroutine = StartCoroutine(WaitForDialogue());

        SoundManager.Instance.LowerVolumeForDialogue(audio.clip.length);
    }

    IEnumerator WaitForDialogue()
    {
        yield return new WaitForSeconds(dialogues[dialogueCount].length);

        if (
            dialogueCount < unlockTriggers.Count() &&
            unlockTriggers[dialogueCount] != Item.Null
        )
        {
            EvidenceManager.Instance.AddEvidence(unlockTriggers[dialogueCount]);
        }

        dialogueCount++;

        if (
            dialogueCount < requiredTriggers.Count() &&
            !EvidenceManager
                .Instance
                .items
                .Contains(requiredTriggers[dialogueCount])
        )
        {
            EvidenceManager.Instance.PlayMissingEvidence();
            ResetDialogue();
            yield break;
        }

        isTalking = false;

        if (dialogueCount >= dialogues.Length)
        {
            if (endEvent != null)
            {
                endEvent.Invoke();
            }
            ResetDialogue();
            yield break;
        }

        text.text = dialoguePrompts[dialogueCount] + " (A)";
        text.gameObject.SetActive(true);

        if (
            dialogueCheckpoints.Any() &&
            dialogueCount - 1 == dialogueCheckpoints[0]
        )
        {
            lastDialogueCheckpoint = dialogueCheckpoints[0];
            dialogueCheckpoints.RemoveAt(0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        text.gameObject.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (isTalking)
        {
            isTalking = false;
            audio.Stop();
            StopCoroutine (coroutine);
            SoundManager.Instance.CancelVolumeCoroutine();
        }
        playerInRange = false;
        text.gameObject.SetActive(false);
        ResetDialogue();
    }

    private void ResetDialogue()
    {
        dialogueCount = lastDialogueCheckpoint;
        text.text = dialoguePrompts[lastDialogueCheckpoint];
    }

    private void TurnTextToPlayer()
    {
        Transform target = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 targetPosition =
            new Vector3(target.position.x,
                text.transform.position.y,
                target.position.z);
        text.transform.LookAt(2 * text.transform.position - targetPosition);
    }
}

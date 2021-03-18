using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceController : MonoBehaviour
{
    public AudioClip clip;

    public Item item;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnPickup()
    {
        EvidenceManager.Instance.OnPickup(this);
    }
}

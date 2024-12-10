using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLine {
    public string key;
    public AudioClip value;
}

public class Announcer : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> clips;

    private Dictionary<string, AudioClip> voiceLines = new();

    private Queue<AudioClip> clipQueue = new();
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        if (source == null)
        {
            enabled = false;
        }

        foreach (AudioClip clip in clips)
        {
            voiceLines.Add(clip.name, clip);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (clipQueue.Count > 0 && !source.isPlaying)
        {
            var clip = clipQueue.Dequeue();
            source.PlayOneShot(clip);
        }
    }

    public void QueueVoiceLine(string lineName)
    {
        if (voiceLines.TryGetValue(lineName, out AudioClip clip))
        {
            clipQueue.Enqueue(clip);
        } else {
            Debug.LogWarning(string.Format("Tried to play Announcer voice line {0}, but it doesn't exist in the prefab mapping!", lineName));
        }
    }
}

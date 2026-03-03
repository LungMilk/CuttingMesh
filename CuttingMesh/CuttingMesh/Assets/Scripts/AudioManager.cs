using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class AudioEvent
{
    public string name;
    public SoundEffectSO sound;
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private int initialPoolSize = 10;

    private Queue<AudioSource> pool = new();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        CreatePool();

    }
    private void CreatePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            var source = CreateNewSource();
            pool.Enqueue(source);
        }
    }

    private AudioSource CreateNewSource()
    {
        GameObject go = new GameObject("PooledAudioSource");
        go.transform.parent = transform;

        AudioSource source = go.AddComponent<AudioSource>();
        source.playOnAwake = false;

        return source;
    }

    private AudioSource GetSource()
    {
        if (pool.Count > 0)
        {
            return pool.Dequeue();
        }
        return CreateNewSource();
    }

    private void ReturnSource(AudioSource source)
    {
        source.Stop();
        source.clip = null;
        pool.Enqueue(source);
    }


    public void Play(SoundEffectSO sound, Vector3 position)
    {
        if (sound == null) { return; }

        AudioClip clip = sound.GetRandomClip();
        if (clip == null) { return; }

        AudioSource source = GetSource();

        source.transform.position = position;
        source.clip = clip;
        source.volume = sound.volume;
        source.pitch = sound.pitch;

        source.Play();
        if (!sound.loop)
            StartCoroutine(ReturnAfterPlayback(source, clip.length / Mathf.Abs(source.pitch)));
    }

    private System.Collections.IEnumerator ReturnAfterPlayback(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnSource(source);
    }
}

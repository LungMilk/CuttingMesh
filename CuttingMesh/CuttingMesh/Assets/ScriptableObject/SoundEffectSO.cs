using UnityEngine;

[CreateAssetMenu(fileName = "New SoundSOAsset.asset", menuName = "SoundEffectSO")]
public class SoundEffectSO : ScriptableObject
{
    public AudioClip[] clips;

    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;

    public bool randomizePitch = false;
    public float pitchVariation = 0.1f;

    public bool loop = false;

    public AudioClip GetRandomClip()
    {
        if (clips == null || clips.Length == 0)
        {
            return null;
        }
        return clips[Random.Range(0, clips.Length)];
    }

    public float GetRandomPitch()
    {
        if (!randomizePitch) { return 1f; }
        return 1f + Random.Range(-pitchVariation, pitchVariation);
    }
}
using UnityEngine;
public enum FrictionTypes
{
    LowFriction,
    MediumFriction,
    HighFriction
}
public class CuttableObject : MonoBehaviour
{
    public FrictionTypes type;
    public float frictionValue;
    public SoundEffectSO cuttingSoundEffect;
    public ParticleSystem cuttingVisualEffect;
    public CuttableObject GetCuttableData()
    {
        return this;
    }

    public void SetCuttableData(CuttableObject input)
    {
        type = input.type;
        frictionValue = input.frictionValue;
        if(input.cuttingSoundEffect != null)
        {
            cuttingSoundEffect = input.cuttingSoundEffect;
        }
        if(input.cuttingVisualEffect != null)
        {
            cuttingVisualEffect = input.cuttingVisualEffect;
        }
    }
}

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
}

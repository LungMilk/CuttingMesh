using UnityEngine;
using UnityEngine.Events;

public class EventTest : MonoBehaviour
{
    Animator anim;
    public UnityEvent CutFriction;
    //public class IntEvent : UnityEvent<int> { }
    //public IntEvent CutFriction;
    public UnityEvent cutfinished;


    public void called()
    {
        anim = gameObject.GetComponent<Animator>();
        //anim.speed = CutFriction.Invoke();
        //Debug.Log("Cutting finished");
        cutfinished?.Invoke();
    }

    public void Frictionator()
    {
        CutFriction?.Invoke();
    }

}


using UnityEngine;
using UnityEngine.Events;

public class EventTest : MonoBehaviour
{
    Animator anim;
    public UnityEvent startCut;
    //public class IntEvent : UnityEvent<int> { }
    //public IntEvent CutFriction;
    public UnityEvent endCut;

    public void StartCut()
    {
        startCut?.Invoke();
    }
    public void EndCut()
    {
        anim = gameObject.GetComponent<Animator>();
        //anim.speed = CutFriction.Invoke();
        //Debug.Log("Cutting finished");
        endCut?.Invoke();
    }
}


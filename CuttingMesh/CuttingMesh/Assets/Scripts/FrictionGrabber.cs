using UnityEngine;
using UnityEngine.Events;


public class FrictionGrabber : MonoBehaviour
{
    public MeshCollider meshCollider;
    public float frictionValue;
    public LayerMask layerMask;
    public FrictionTypes type;
    public float frictValue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision Detected");
        //if (collision.gameObject.layer == 9)
        //{
        //    if(collision.gameObject.tag == "LowFriction")
        //    {
        //        Debug.Log("low friction target");
        //        frictionValue = 1;
        //    }
        //    else if(collision.gameObject.tag == "Medium Friction")
        //    {
        //        Debug.Log("medium friction target");
        //        frictionValue = .75f;
        //    }
        //    else if(collision.gameObject.tag == "HighFriction")
        //    {
        //        Debug.Log("High friction target");
        //        frictionValue = .5f;
        //    }
        //    else
        //    {
        //        frictionValue = 1;
        //        Debug.Log("Nothing found with a tag");
        //    }
        //}
        //else
        //{
        //    Debug.Log("Nothing found with a layer of 9");
        //}

    }

    public void GetFriction()
    {
        //return 0;
        Debug.Log("Friction Grabbed");
    }

    public void FrictionTest(Animator anim)
    {
        Debug.Log("First");


        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f, layerMask))
        {
            Debug.Log("Second");
            //Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
            string tag = hit.collider.gameObject.tag;
            frictionValue = EvaluateTag(tag);
            Debug.Log("Fourth");
            anim.speed = frictionValue;

        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 10f, Color.red, 2f);
            Debug.Log("Raycast did not hit any object.");
        }

    }

    public void FrictionController(FrictionTypes amt)
    {
        switch (amt)
        {             case FrictionTypes.LowFriction:
                frictValue = 1;
                break;
            case FrictionTypes.MediumFriction:
                frictValue = .75f;
                break;
            case FrictionTypes.HighFriction:
                frictValue = .05f;
                break;
            default:
                frictValue = 1;
                break;
        }
    }

    public float EvaluateTag(string tag)
    {
        Debug.Log("Third");
        if (tag == "LowFriction")
        {
            return 1;
        }
        else if(tag == "Medium Friction")
        {
            return .75f;
        }
        else if(tag == "HighFriction")
        {
            return .05f;
        }
        else
        {
            return 1;
        }
    }
}

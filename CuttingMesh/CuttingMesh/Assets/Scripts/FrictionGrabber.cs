using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Events;


public class FrictionGrabber : MonoBehaviour
{
    //public MeshCollider meshCollider;
    public Animator animator;
    public float frictionValue;
    public LayerMask layerMask;
    public FrictionTypes type;
    public float frictValue;

    public Transform detectionBox;
    public float boxWidth = 2f;
    public FrictionTypes foundType;
    public ParticleSystem foundParticle;
    public SoundEffectSO foundSound;

    [Header("StartCut events being called")]
    public UnityEvent NoCuttableFound;
    public UnityEvent YesCuttableFound;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            print(FindCuttableObjects()[0].name);
        }
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


    public void StartCut()
    {
        var foundObjects = FindCuttableObjects();


        if (foundObjects.Length > 0)
        {
            SetCuttableObjectData(foundObjects[0]);
            YesCuttableFound.Invoke();
        }

        if (foundObjects.Length == 0)
        {
            NoCuttableFound.Invoke();
        }
    }
    /// <summary>
    /// Returns an array of cuttable object scripts, reference their game object or mesh by having its parent reference or something
    /// </summary>
    /// <returns></returns>
    public CuttableObject[] FindCuttableObjects()
    {
        Collider[] hits = Physics.OverlapBox(
            detectionBox.position, 
            new Vector3(boxWidth, boxWidth, boxWidth),
            transform.rotation,
            layerMask
            );

        CuttableObject[] objects = new CuttableObject[hits.Length];
        for (int i = 0; i < hits.Length; i++)
        {
            var cuttableObject = (hits[i].GetComponent<CuttableObject>());
            if (cuttableObject != null)
            {
                objects[i] = cuttableObject;
            }
        }

        return objects;
    }

    public void SetCuttableObjectData(CuttableObject cuttable)
    {
        foundType = cuttable.type;
        frictionValue = cuttable.frictionValue;
        animator.speed = frictionValue;
        print($"Friction value on {cuttable.name}:{frictionValue}");


        if (cuttable.cuttingSoundEffect != null)
        {
            foundSound = cuttable.cuttingSoundEffect;
        }
        else
        {
            //print($"No sound effect found on {cuttable.gameObject.name}");
        }

        if (cuttable.cuttingVisualEffect != null)
        {
            cuttable.cuttingVisualEffect = null;
        }
        else
        {
            //print($"no particle found on {cuttable.gameObject.name}");
        }
    }

    public void SetFrictionValue(float input)
    {
        frictionValue = input;
        animator.speed = frictionValue;

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

    //public void FrictionController(FrictionTypes amt)
    //{
    //    switch (amt)
    //    {             case FrictionTypes.LowFriction:
    //            frictValue = 1;
    //            break;
    //        case FrictionTypes.MediumFriction:
    //            frictValue = .75f;
    //            break;
    //        case FrictionTypes.HighFriction:
    //            frictValue = .05f;
    //            break;
    //        default:
    //            frictValue = 1;
    //            break;
    //    }
    //}

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

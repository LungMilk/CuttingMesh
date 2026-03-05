using EzySlice;
using StarterAssets;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Collections;
using System.ComponentModel.Design;
public class CuttingMode : MonoBehaviour
{
    bool isCutting = false;
    public Transform cuttingPlane;
    public FirstPersonController firstPersonController;
    public LayerMask layerMask;
    public RenderingLayerMask renderingLayer;

    public Transform chainsawTransform;

    public float rotationAngle = 25;

    public float sawAngleStep = 15;
    public float cuttingAngleMod = 1f;
    public float sawMaxAngle = 180;
    public float sawMinAngle = 0;
    private float currentAngle = 0;
    private float targetAngle = 0;

    public float rotateDuration = 2f;
    private bool rotateSaw = false;
    //private Coroutine rotatingSawCorout;


    private float currentRotation;
    public Material cutMaterial;

    public SoundEffectSO cuttingSoundEffect;
    bool cut;
    public float explosiveCuttingForce = 50f;

    public UnityEvent cuttingEvent;
    public UnityEvent finishCut;
    public FrictionGrabber frictionGrab;
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    firstPersonController.moveCamera = !firstPersonController.moveCamera;
        //    firstPersonController.Inputs.SetCursorState(firstPersonController.moveCamera);
            
        //}
        if (Input.GetMouseButtonDown(0))
        {
            if (!isCutting) { isCutting = true; cuttingEvent.Invoke(); }
            print("cutting");
            RotateSaw(sawAngleStep);
            //if (!rotateSaw) {
            //    currentAngle = chainsawTransform.localEulerAngles.y;
            //    targetAngle = currentAngle + sawAngleStep;
            //    print(currentAngle.ToString());
            //    elapsedTime = 0f;
            //}
            //rotateSaw = true;
            //Slice();
        }
        //rotation locks do not work yet
        currentRotation = cuttingPlane.rotation.z;
        if (Input.GetKey(KeyCode.E))
        {
            RotatePlane(+rotationAngle * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            RotatePlane(-rotationAngle * Time.deltaTime);
        }

        //if (rotateSaw)
        //{
        //    RotateSaw(sawAngleStep);
        //}
    }
    public void RotatePlane()
    {
        cuttingPlane.eulerAngles += new Vector3(0, 0, -Input.GetAxis("Mouse X") * 5);
    }
    public void RotateSaw(float angle)
    {
        //float yAngle = chainsawTransform.localEulerAngles.y;
        chainsawTransform.localEulerAngles += new Vector3(0, (angle * cuttingAngleMod), 0);

        if (chainsawTransform.localEulerAngles.y >= sawMaxAngle)
        {
            print("passed threshold");
            chainsawTransform.localEulerAngles = new Vector3(0, sawMinAngle, 0);
            isCutting = false;
            finishCut?.Invoke();
        }
    }
    public void RotateSawTimer(float duration)
    {
        if (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float value = Mathf.Lerp(currentAngle,targetAngle, t);
            //print(value + ", " + t);
            chainsawTransform.localEulerAngles = new Vector3(0, -value, 0);
            elapsedTime += Time.deltaTime;
        }
        if (elapsedTime >= duration)
        {
            print("finished duration");
            rotateSaw = false;
        }

        //if (rotatingSawCorout == null)
        //{
        //    //rotatingSawCorout = StartCoroutine(RotateSawCoroutine(angle, duration));
        //}
    }
    private float elapsedTime;
    //IEnumerator RotateSawCoroutine(float angle, float duration)
    //{
    //    print("coroutine running");
    //    float initialStart = currentAngle;

    //    while (elapsedTime < duration)
    //    {
    //        float t = elapsedTime / duration;

    //        float sineT = Mathf.Sin(t * Mathf.PI * 0* 5f);
    //        currentAngle = Mathf.Lerp(initialStart, initialStart + angle, sineT);

    //        chainsawTransform.localEulerAngles += new Vector3(0, -currentAngle * 5, 0);

    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }
        //float timer = duration;

        //while (timer > 0)
        //{
        //    yield return new WaitForSeconds(1f);
        //    timer -= 1f;
        //}

        //yield return null;
    //}

    public void RotatePlane(float angle)
    {
        cuttingPlane.eulerAngles += new Vector3(0, 0, -angle * 5);
    }
    public CuttableObject[] FindCuttableObjects()
    {
        if (frictionGrab.FindCuttableObjects() == null) { return null; }
        return frictionGrab.FindCuttableObjects();
    }

    public void PrepareToCut()
    {
        var objs = FindCuttableObjects();
        if (objs != null || objs.Length >0)
        {
            cuttingAngleMod = objs[0].frictionValue;
        }
        else
        {
            print($"no friction found on cutting start of {objs[0].name}");
            cuttingAngleMod = 1;
        }
    }
    public void Slice()
    {
        //print("cutting");
        if (cuttingSoundEffect != null)
        {
            AudioManager.Instance.Play(cuttingSoundEffect, this.transform.position);
        }

        var objects = FindCuttableObjects();
        if (objects == null) { return; }

        Collider[] hits = new Collider[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            hits[i] = objects[i].GetComponent<Collider>();
        }

        if (hits.Length <= 0) { return; }

        for (int i = 0; i< hits.Length; i++)
        {
            SlicedHull hull = SliceObject(hits[i].gameObject,cutMaterial);
            if (hull != null)
            {
                float mass = hits[i].attachedRigidbody.mass;
                Material mat = hits[i].GetComponent<MeshRenderer>().sharedMaterial;
                GameObject bottom = hull.CreateLowerHull(hits[i].gameObject, null);
                GameObject top = hull.CreateUpperHull(hits[i].gameObject, null);
                AddHullComponents(bottom, mass, objects[i],mat);
                AddHullComponents(top, mass, objects[i],mat);
                Destroy(hits[i].gameObject);
            }
        }
    }
    public void IncorrectCuttable()
    {
        print("BAD CUT");
    }

    public SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial)
    {
        return obj.Slice(cuttingPlane.position,cuttingPlane.up,crossSectionMaterial);
    }

    public void AddHullComponents(GameObject go,float mass, CuttableObject cuttableData, Material mat)
    {
        go.layer = 9; //figure that out later
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.mass = mass;

        MeshCollider collider = go.AddComponent<MeshCollider>();
        collider.convex = true;

        CuttableObject cO = go.AddComponent<CuttableObject>();
        cO.SetCuttableData(cuttableData);

        var mr = go.GetComponent<MeshRenderer>();
        mr.sharedMaterial = mat;

        mr.renderingLayerMask = renderingLayer;
        List<Material> materials = new List<Material>();
        mr.GetMaterials(materials);

        for (int i = 0; i < materials.Count; i++)
        {
            materials[i] = mat;
        }
        mr.SetMaterials(materials);
        rb.AddExplosionForce(explosiveCuttingForce, go.transform.position,10);
    }
}

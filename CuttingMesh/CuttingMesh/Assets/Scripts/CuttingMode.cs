using EzySlice;
using StarterAssets;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using UnityEngine.Events;
public class CuttingMode : MonoBehaviour
{
    bool isCutting = false;
    public Transform cuttingPlane;
    public FirstPersonController firstPersonController;
    public LayerMask layerMask;
    public RenderingLayerMask renderingLayer;

    public float rotationAngle = 25;
    private float currentRotation;
    public Material cutMaterial;

    public SoundEffectSO cuttingSoundEffect;
    bool cut;
    public float explosiveCuttingForce = 50f;

    public UnityEvent cuttingEvent;
    public FrictionGrabber frictionGrab;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            firstPersonController.moveCamera = !firstPersonController.moveCamera;
            firstPersonController.Inputs.SetCursorState(firstPersonController.moveCamera);
            
        }
        if (Input.GetMouseButtonDown(0) && !cut)
        {
            cuttingEvent.Invoke();
            //Slice();
            cut = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            cut = false;
        }
        //rotation locks do not work yet
        currentRotation = cuttingPlane.rotation.z;
        if (Input.GetKey(KeyCode.E) && !(currentRotation >= 160))
        {
            RotatePlane(+rotationAngle * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Q) && !(currentRotation <= -160))
        {
            RotatePlane(-rotationAngle * Time.deltaTime);
        }
    }
    public void RotatePlane()
    {
        cuttingPlane.eulerAngles += new Vector3(0, 0, -Input.GetAxis("Mouse X") * 5);
    }

    public void RotatePlane(float angle)
    {
        cuttingPlane.eulerAngles += new Vector3(0, 0, -angle * 5);
    }
    public void Slice()
    {
        //print("cutting");
        if (cuttingSoundEffect != null)
        {
            AudioManager.Instance.Play(cuttingSoundEffect, this.transform.position);
        }

        if (frictionGrab.FindCuttableObjects() == null) { return; }
        var objects = frictionGrab.FindCuttableObjects();


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
                GameObject bottom = hull.CreateLowerHull(hits[i].gameObject, null);
                GameObject top = hull.CreateUpperHull(hits[i].gameObject, null);
                AddHullComponents(bottom, mass, objects[i]);
                AddHullComponents(top, mass, objects[i]);
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

    public void AddHullComponents(GameObject go,float mass, CuttableObject cuttableData)
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
        mr.renderingLayerMask = renderingLayer;
        List<Material> materials = new List<Material>();
        mr.GetMaterials(materials);

        for (int i = 0; i < materials.Count; i++)
        {
            if (materials[i] == null)
            {
                materials[i] = cutMaterial;
            }
        }
        rb.AddExplosionForce(explosiveCuttingForce, go.transform.position,10);
    }
}

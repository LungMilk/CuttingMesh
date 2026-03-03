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
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !cut)
        {
            Slice();
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
        if (cuttingSoundEffect != null)
        {
            AudioManager.Instance.Play(cuttingSoundEffect, this.transform.position);
        }

        Collider[] hits = Physics.OverlapBox(cuttingPlane.position, new Vector3(10, 0.1f, 10), cuttingPlane.rotation, layerMask);

        if (hits.Length <= 0) { return; }

        for (int i = 0; i< hits.Length; i++)
        {
            SlicedHull hull = SliceObject(hits[i].gameObject,cutMaterial);
            if (hull != null)
            {
                float mass = hits[i].attachedRigidbody.mass;
                GameObject bottom = hull.CreateLowerHull(hits[i].gameObject, null);
                GameObject top = hull.CreateUpperHull(hits[i].gameObject, null);
                AddHullComponents(bottom,mass);
                AddHullComponents(top,mass);
                Destroy(hits[i].gameObject);
            }
        }
    }

    public SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial)
    {
        return obj.Slice(cuttingPlane.position,cuttingPlane.up,crossSectionMaterial);
    }

    public void AddHullComponents(GameObject go,float mass)
    {
        go.layer = 9; //figure that out later
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.mass = mass;
        MeshCollider collider = go.AddComponent<MeshCollider>();
        collider.convex = true;
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
        rb.AddExplosionForce(100, go.transform.position,10);
    }
}

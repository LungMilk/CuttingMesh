using EzySlice;
using StarterAssets;
using UnityEngine;
using UnityEngine.Rendering;

public class CuttingMode : MonoBehaviour
{
    bool isCutting = false;
    public Transform cuttingPlane;
    public FirstPersonController firstPersonController;
    public LayerMask layerMask;

    public SoundEffectSO cuttingSoundEffect;
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isCutting = firstPersonController.moveCamera;
            firstPersonController.moveCamera = !firstPersonController.moveCamera;
        }

        if (isCutting)
        {
            RotatePlane();
            if (Input.GetMouseButtonDown(0))
            {
                //Slice();
            }
        }
    }
    public void RotatePlane()
    {
        cuttingPlane.eulerAngles += new Vector3(0, 0, -Input.GetAxis("Mouse X") * 5);
    }

    public void Slice()
    {

        if (cuttingSoundEffect != null)
        {
            Debug.Log("Playing cutting sound effect");
            AudioManager.Instance.Play(cuttingSoundEffect, this.transform.position);
        }

        Collider[] hits = Physics.OverlapBox(cuttingPlane.position, new Vector3(10, 0.1f, 10), cuttingPlane.rotation, layerMask);

        if (hits.Length <= 0) { return; }

        for (int i = 0; i< hits.Length; i++)
        {
            SlicedHull hull = SliceObject(hits[i].gameObject,null);
            if (hull != null)
            {
                GameObject bottom = hull.CreateLowerHull(hits[i].gameObject, null);
                GameObject top = hull.CreateUpperHull(hits[i].gameObject, null);
                AddHullComponents(bottom);
                AddHullComponents(top);
                Destroy(hits[i].gameObject);
            }
        }
    }

    public SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        return obj.Slice(cuttingPlane.position,cuttingPlane.up,crossSectionMaterial);
    }

    public void AddHullComponents(GameObject go)
    {
        go.layer = 9; //figure that out later
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        MeshCollider collider = go.AddComponent<MeshCollider>();
        collider.convex = true;

        rb.AddExplosionForce(100, go.transform.position,10);
    }
}

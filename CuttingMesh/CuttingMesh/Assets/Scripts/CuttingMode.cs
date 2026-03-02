using StarterAssets;
using UnityEngine;

public class CuttingMode : MonoBehaviour
{
    bool isCutting = false;
    public Transform cuttingPlane;
    public FirstPersonController firstPersonController;
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
        }
    }
    public void RotatePlane()
    {
        cuttingPlane.eulerAngles += new Vector3(0, 0, -Input.GetAxis("Mouse X") * 5);
    }
}

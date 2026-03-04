using TMPro;
using UnityEngine;

public class CanGrab : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject text;
    public TextMeshProUGUI fuelText;
    bool canGrab = false;
    GameObject can;
    public float fuel = 100f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (canGrab)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Destroy(can);
                fuel += 10f;
                fuelText.text = "Fuel : " + fuel;
                canGrab = false;
            }
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            fuel += 10f;
            fuelText.text = "Fuel: " + fuel;
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            /*text.SetActive(false);
            canGrab = false;*/
        }
    }
}

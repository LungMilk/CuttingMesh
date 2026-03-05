using TMPro;
using UnityEngine;

public class InputDetector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI Q;
    public TextMeshProUGUI E;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            E.text = "<sprite name=\"keyboard-&-mouse_sheet_default_170\">";
        }
        else
        {
            E.text = "<sprite name=\"keyboard-&-mouse_sheet_default_171\">";
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Q.text = "<sprite name=\"keyboard-&-mouse_sheet_default_95\">";
        }
        else
        {
            Q.text = "<sprite name=\"keyboard-&-mouse_sheet_default_64\">";
        }
    }
}

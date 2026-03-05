using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float counter = 0;
    public Image fade;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            counter++;
            Color color = fade.color;
            color.a = (counter/500f);            
            fade.color = color;
            if (counter >= 500f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else
        {
            counter = 0;
            Color color = fade.color;
            color.a = counter;
            fade.color = color;
        }
    }
}

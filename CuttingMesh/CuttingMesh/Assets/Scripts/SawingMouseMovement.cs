using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor.PackageManager.UI;
using UnityEditor;
public enum Direction
{
    Up,Down
}
public class SawingMouseMovement : MonoBehaviour
{
    public float lockMax;
    [SerializeField] private float lockValue;
    public float cutStrength;
    public float validDistance;
    private Vector2 previousMousePos;

    public Slider slider;
    public Slider sawPositionSlider;
    //public float distanceStartThreshold = 10;

    [SerializeField] private float targetTime = 60f;

    public bool lockDirection;

    [SerializeField] private Direction movingDirection;
    private Direction previousDirection;

    //private float scrollAccumulator = 0f;
    public float mouseDistThreshold = 1.5f;
    private void Start()
    {
        validDistance = validDistance * 60f;
        targetTime = validDistance;

        lockValue = lockMax;
        slider.maxValue = lockMax;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            lockValue = lockMax;
            slider.maxValue = lockMax;
        }
        slider.value = lockValue;
        sawPositionSlider.value = NormalizeMousePos().y;
        print(NormalizeMousePos().y);
        //if (!Input.GetMouseButton(0))
        //{
        //    return;
        //}

        if (lockValue < 0)
        {
            print("passed lock");
        }
        ////scroll wheel
        //if (Input.mouseScrollDelta.y >0)
        //{
        //    print("moving up");
        //}
        //if (Input.mouseScrollDelta.y < 0)
        //{
        //    print("movingDown");
        //}
        //but in pixel coords
        //Mouse Position

        //if (targetTime <= 0)
        //{
        //    print("reached threshold");
        //    targetTime = validDistance;
        //    lockDirection = true;
        //}
        
        //As long as moving we are removing it has no relation on the distance moved between the two points.
        if (movingDirection != previousDirection)
        {
            lockDirection = false;
        }
        float distance = Vector2.Distance(Input.mousePosition, previousMousePos);

        if (previousMousePos.y < Input.mousePosition.y)
        {
            movingDirection = Direction.Up;
            //print("mouse Up");
            //targetTime -= Time.deltaTime;
            lockValue -= cutStrength *Time.deltaTime;
        }
        else if (previousMousePos.y > Input.mousePosition.y)
        {
            movingDirection = Direction.Down;
            //print("mouse Down");
            //targetTime -= Time.deltaTime;
            lockValue -= cutStrength * Time.deltaTime;
        }

        previousMousePos = Input.mousePosition;
        previousDirection = movingDirection;
    }

    Vector2 NormalizeMousePos()
    {
        Vector3 mousePos = Input.mousePosition;

        float normalizedX = mousePos.x / Screen.width;
        float normalizedY = mousePos.y/Screen.height;
        return new Vector2(normalizedX, normalizedY);
    }
}

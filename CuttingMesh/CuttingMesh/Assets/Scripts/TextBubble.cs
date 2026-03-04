using TMPro;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// An individual TextBubble object
/// </summary>
/// <para>
/// <see cref="TextBubbleManager"/> will manage the bubbles visibility and text.
/// </para>
public class TextBubble : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image background;
    [SerializeField] private Vector3 offset = Vector3.up * 2f;

    private Transform parent;
    private Transform activeCamera;
    private void Awake()
    {
        activeCamera = Camera.main.transform;
        //text = GetComponent<TextMeshProUGUI>();
        //this.gameObject.SetActive(false);
    }
    public void Show(string message, Transform target)
    {
        parent = target;
        gameObject.SetActive(true);
        SetText(message);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        parent = null;
    }
    private void Update()
    {
        if (parent != null)
        {
            transform.position = parent.position + offset;
            //transform.forward = activeCamera.transform.forward;
            transform.LookAt(activeCamera.transform);
            transform.Rotate(0, 180f, 0);
        }
    }
    public void SetText(string message)
    {
        this.gameObject.SetActive(true);
        text.text = message;
        text.ForceMeshUpdate();
        Vector2 textSize = text.GetRenderedValues(false);

        Vector2 padding = new Vector2(0.3f, 0.3f);
        RectTransform backGroundRect = background.GetComponent<RectTransform>();
        backGroundRect.sizeDelta = new Vector2(textSize.x + padding.x, backGroundRect.sizeDelta.y);
    }
}
using UnityEngine;
/// <summary>
/// A component that will speak to the TextBubbleManager.Instance to create and dismiss the bubble.
/// </summary>
public class PopUpSpeaker : MonoBehaviour
{
    [SerializeField]
    private Transform textAnchor;
    [TextArea]
    public string text;
    TextBubble textBubble;
    [ContextMenu("SpawnBubble")]
    public void CreateBubble()
    {
        textBubble = TextBubbleManager.Instance.Get(text, textAnchor);
    }
    /// <summary>
    /// Create a bubble from the speaker
    /// </summary>
    /// <param name="text">text you want to display</param>
    public void CreateBubble(string text)
    {
        textBubble = TextBubbleManager.Instance.Get(text, textAnchor);
    }
    /// <summary>
    /// dismiss a bubble controlled by the speaker
    /// </summary>
    /// <para>
    /// <see cref="PopUpSpeaker"/> contains its own reference to a bubble, calling this will simply dismiss the bubble.
    /// </para>
    public void DismissBubble()
    {
        TextBubbleManager.Instance.Release(textBubble);
    }
}

using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manager that creates a static Instance of an object pool of <see cref="TextBubble"/> objects that anything can get.
/// </summary>
public class TextBubbleManager : MonoBehaviour
{
    public static TextBubbleManager Instance;

    [SerializeField] private TextBubble bubblePrefab;
    [SerializeField] private int poolSize = 15;

    private Queue<TextBubble> pool = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        for (int i = 0; i < poolSize; i++)
        {
            CreateBubble();
        }
    }

    void CreateBubble()
    {
        var bubble = Instantiate(bubblePrefab, transform);
        bubble.gameObject.SetActive(false);
        pool.Enqueue(bubble);
        //return bubble;
    }
    /// <summary>
    /// Asks the object pool to return a textbubble object
    /// </summary>
    /// <param name="text">What the bubble should contain</param>
    /// <param name="anchor">Where the bubble will be anchored</param>
    /// <returns></returns>
    public TextBubble Get(string text, Transform anchor)
    {
        if (pool.Count == 0)
        {
            CreateBubble();
        }

        var bubble = pool.Dequeue();
        bubble.Show(text, anchor);
        return bubble;
    }
    /// <summary>
    /// With the reference from <see cref="Release(TextBubble)"/> we return our textBubble back to the pool
    /// </summary>
    /// <param name="bubble">The bubble gained from <see cref="Get(string, Transform)"/></param>
    public void Release(TextBubble bubble)
    {
        print("Release");
        bubble.Hide();
        pool.Enqueue(bubble);
    }
}

using UnityEngine;

public class ColorLerp : MonoBehaviour
{
    public Color color1 = Color.green;  // First color
    public Color color2 = Color.blue;   // Second color
    public float duration = 2.0f;       // Time it takes to transition from one color to the other

    private Renderer objectRenderer;

    void Start()
    {
        // Get the Renderer component of the object to which this script is attached
        objectRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        // Calculate the t value for Lerp based on time
        float t = Mathf.PingPong(Time.time / duration, 1.0f);
        
        // Lerp between the two colors based on t
        objectRenderer.material.color = Color.Lerp(color1, color2, t);
    }
}

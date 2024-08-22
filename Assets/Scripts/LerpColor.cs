using UnityEngine;

public class ColorLerp : MonoBehaviour
{
    public Color color1 = Color.green;  
    public Color color2 = Color.blue;   
    public float duration = 2.0f;       

    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time / duration, 1.0f);
        
        objectRenderer.material.color = Color.Lerp(color1, color2, t);
    }
}

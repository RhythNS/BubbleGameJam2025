using UnityEngine;

public class GradientBackground : MonoBehaviour
{
    private Material material;
    private Shader shader;
    [SerializeField]
    private int propertyY;
    [SerializeField]
    private int propertyGradientY;

    private void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        shader = material.shader;
        
        propertyY = shader.FindPropertyIndex("Y");
        propertyGradientY = shader.FindPropertyIndex("GradientY");
    }


    private void Update()
    {
        float y = transform.position.y;
        float gradientY = Mathf.Clamp01((y + 5) / 10);

        material.SetFloat(propertyY, y);
        material.SetFloat(propertyGradientY, gradientY);
    }
}

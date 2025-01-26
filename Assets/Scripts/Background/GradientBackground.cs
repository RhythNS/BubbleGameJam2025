using UnityEngine;

public class GradientBackground : MonoBehaviour
{
    private Material material;
    private Shader shader;
    [SerializeField]
    private int propertyY;
    [SerializeField]
    private int propertyGradientY;

    [SerializeField]
    private int selector;

    private void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        shader = material.shader;

        propertyY = shader.FindPropertyIndex("Y");
        propertyGradientY = shader.FindPropertyIndex("GradientY");

        selector = shader.FindPropertyIndex("IsUnderwater");

        Underwater();
    }


    private void Update()
    {
        float y = transform.position.y;

        material.SetFloat(propertyY, y);

        float gradient = LevelLoader.Instance.GetThingForGradient();
        material.SetFloat(propertyGradientY, gradient);

    }

    public void Credits()
    {
        material.SetInt(selector, 1);
    }

    public void Underwater()
    {
        material.SetInt(selector, 0);
    }
}

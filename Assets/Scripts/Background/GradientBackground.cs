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

        propertyY = shader.FindPropertyIndex("_Y");
        Debug.Log("Propperty count: " + shader.GetPropertyCount());
        propertyGradientY = shader.FindPropertyIndex("_GradientY");

        selector = shader.FindPropertyIndex("_IsUnderwater");

        Underwater();
    }


    private void Update()
    {
        float y = transform.position.y;

        material.SetFloat(propertyY, y);

        float gradient = LevelLoader.Instance.GetThingForGradient();
        gradient = Mathf.Clamp01(gradient);
        
        material.SetFloat(propertyGradientY, gradient);
        //Debug.Log(gradient);
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

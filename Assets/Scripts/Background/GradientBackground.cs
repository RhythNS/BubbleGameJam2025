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

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetFloat("_GradientY", gradient);
        spriteRenderer.SetPropertyBlock(block);
    }

    public void Credits()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetFloat("_IsUnderwater", 0);
        spriteRenderer.SetPropertyBlock(block);
    }

    public void Underwater()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetFloat("_IsUnderwater", 1);
        spriteRenderer.SetPropertyBlock(block);
    }
}

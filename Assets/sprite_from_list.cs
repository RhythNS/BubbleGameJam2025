using UnityEngine;

public class sprite_from_list : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = RandomUtil.Element(sprites);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

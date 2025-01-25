using UnityEngine;
using UnityEngine.EventSystems;

public class Mu_UIOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float increasedValueOnHover = 1.2f;
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(increasedValueOnHover, increasedValueOnHover);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }
}

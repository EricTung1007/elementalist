using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ElementHoveredHint : MonoBehaviour, IPointerEnterHandler//, IPointerExitHandler
{
    /*
    int animationCooldown = 0;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (animationCooldown <= 0)
        {
            animationCooldown = 50;
        }
    }

    private void FixedUpdate()
    {
        //Debug.Log("PLayed!");
        if (animationCooldown >= 50) 
        {
            GetComponent<Animation>().Play();
        }
        animationCooldown -= 50;
    }
    */
    public void OnPointerEnter(PointerEventData eventData)
    {
        var anim = GetComponent<Animation>();

        //anim.Rewind();
        anim.Play();
        //transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }
    /*
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }*/
}

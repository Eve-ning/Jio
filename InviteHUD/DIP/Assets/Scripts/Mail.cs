using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mail : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite closedMail;
    public Sprite openedMail;

    void OnMouseOver()
    {
        spriteRenderer.sprite = openedMail;
    }

    void OnMouseExit()
    {
        spriteRenderer.sprite = closedMail;
    }
}

using TMPro;
using UnityEngine;

public class Target : MonoBehaviour, IDamageble
{
    private Renderer render;
    private Color defaultColor;
    private bool colorChanged = false;
    private void Awake()
    {
        render = GetComponent<Renderer>();
    }
    private void ChangeColor()
    {
        if (!colorChanged) defaultColor = render.material.color;

        render.material.SetColor("_Color", Color.red);
        colorChanged = true;
        Invoke("RestoreColor", 0.5f);
    }
    private void RestoreColor()
    {
        render.material.SetColor("_Color", defaultColor);
        colorChanged = false;
    }
    public void TakeDamage(float amount)
    {
        ChangeColor();
    }
}
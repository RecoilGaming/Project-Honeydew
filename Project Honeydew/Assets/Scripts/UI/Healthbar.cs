using System.Collections;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    // private variables
    [SerializeField] private Transform healthOverlay;

    public void SetHealthPercent(float health)
    {
        Vector2 scale = healthOverlay.localScale;
        scale.x = health;
        healthOverlay.localScale = scale;
    }
}
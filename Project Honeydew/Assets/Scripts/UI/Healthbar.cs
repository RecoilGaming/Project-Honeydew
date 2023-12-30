using UnityEngine;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Transform healthOverlay;

    public void SetHealthPercent(float health)
    {
        Vector2 scale = healthOverlay.localScale;
        scale.x = health;
        healthOverlay.localScale = scale;
    }
}
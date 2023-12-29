using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public void SetHealthPercent(float health)
    {
        Vector2 scale = transform.localScale;
        scale.x = health;
        transform.localScale = scale;
    }
}
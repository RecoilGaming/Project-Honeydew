using UnityEngine;

public class Experiencebar : MonoBehaviour
{
    [SerializeField] private Transform experienceOverlay;

    public void SetExperiencePercent(float experience)
    {
        Vector2 scale = experienceOverlay.localScale;
        scale.x = experience;
        experienceOverlay.localScale = scale;
    }
}
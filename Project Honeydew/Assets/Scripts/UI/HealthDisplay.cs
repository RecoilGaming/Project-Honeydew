using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    private TextMeshProUGUI text;

    // runs before scene loads
    private void Awake()
	{
        text = transform.GetComponent<TextMeshProUGUI>();
    }

    public void SetHealth(int health, int maxHealth)
    {
        text.text = health + "/" + maxHealth;
    }
}

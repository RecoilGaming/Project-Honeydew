using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public PlayerUpgrade upgrade;
    private new TextMeshProUGUI name;
    private TextMeshProUGUI desc;
    private Color originalColor;

    // runs before scene loads
    private void Awake()
	{
        name = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        desc = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    // runs when script loads
    private void Start()
    {
        originalColor = transform.GetComponentInParent<Image>().color;
    }

    public void SetRare(bool b)
    {
        if (b) transform.GetComponentInParent<Image>().color = new Color(51, 255, 85);
        else transform.GetComponentInParent<Image>().color = originalColor;
    }

    public void SetUpgrade(PlayerUpgrade upgrade)
    {
        this.upgrade = upgrade;
        name.text = upgrade.name;
        desc.text = upgrade.description;
    }
}

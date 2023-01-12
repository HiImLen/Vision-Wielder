using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EXPBarScript : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;
    [SerializeField] private Vector3 offset;
    [SerializeField] private TextMeshProUGUI levelText;
    string maxEXP;

    void Awake()
    {
        
    }

    public void SetEXP(float exp, float maxExp, int level)
    {
        slider.maxValue = maxExp;
        slider.value = exp;
        maxEXP = maxExp.ToString();

        fill.color = gradient.Evaluate(slider.normalizedValue);
        if (levelText != null) levelText.text = "Lv. " + level.ToString();
    }
}

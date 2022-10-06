using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthFill;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Character character;

    void OnEnable()
    {
        character.OnHealthChange += OnUpdateHealth;
    }

    void OnDisable()
    {
        character.OnHealthChange -= OnUpdateHealth;
    }

    void Start()
    {
        OnUpdateHealth();
    }

    void OnUpdateHealth()
    {
        healthFill.fillAmount = character.GetHealthPercentage();
        healthText.text = character.CurHP + " / " + character.MaxHP;
    }
}

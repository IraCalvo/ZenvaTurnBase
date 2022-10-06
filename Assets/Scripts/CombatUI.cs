using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUI : MonoBehaviour
{
    //group of buttons that will be enabled/disabled when it is player turn
    [SerializeField] private GameObject visualContainer;
    [SerializeField] private Button[] combatActionButtons;

    void OnEnable()
    {
        TurnManager.Instance.OnBeginTurn += OnBeginTurn;
        TurnManager.Instance.OnEndTurn += OnEndTurn;
    }

    void OnDisable()
    {
        TurnManager.Instance.OnBeginTurn -= OnBeginTurn;
        TurnManager.Instance.OnEndTurn -= OnEndTurn;
    }

    void OnBeginTurn(Character character)
    {
        if(!character.isPlayer)
            return;

        visualContainer.SetActive(true);

        for(int i = 0; i < combatActionButtons.Length; i++)
        {
            if(i < character.CombatActions.Count)
            {
                combatActionButtons[i].gameObject.SetActive(true);
                CombatAction ca = character.CombatActions[i];

                combatActionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = ca.DisplayName;
                combatActionButtons[i].onClick.RemoveAllListeners();
                combatActionButtons[i].onClick.AddListener(() => OnClickCombatAction(ca));
            }
            else
            {
                combatActionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnEndTurn(Character character)
    {
        visualContainer.SetActive(false);
    }

    public void OnClickCombatAction(CombatAction combatAction)
    {
        TurnManager.Instance.CurrentCharacter.CastCombatAction(combatAction);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private AnimationCurve healChanceCurve;

    [SerializeField] private Character character;

    void OnEnable()
    {
        TurnManager.Instance.OnBeginTurn += OnBeginTurn;
    }

    void OnDisable()
    {
        TurnManager.Instance.OnBeginTurn -= OnBeginTurn;
    }

    void OnBeginTurn(Character c)
    {
        if(character == c)
        {
            DetermineCombatAction();
        }
    }

    void DetermineCombatAction()
    {
        float healthPercentage = character.GetHealthPercentage();
        bool wantToHeal = Random.value < healChanceCurve.Evaluate(healthPercentage);

        CombatAction ca = null;

        if(wantToHeal && HasCombatActionOfType(CombatAction.Type.Heal))
        {
            ca = GetCombatActionOfType(CombatAction.Type.Heal);
        }
        else if(HasCombatActionOfType(CombatAction.Type.Attack))
        {
            ca = GetCombatActionOfType(CombatAction.Type.Attack);
        }

        if(ca != null)
            character.CastCombatAction(ca);
        else
            TurnManager.Instance.EndTurn();

    }

    bool HasCombatActionOfType(CombatAction.Type type)
    {
        //going thru enemies list of available moves and checking if anything exists of the current condition,if it does exist then it returns true
        return character.CombatActions.Exists(x => x.ActionType == type);
    }

    CombatAction GetCombatActionOfType(CombatAction.Type type)
    {
        List<CombatAction> availableActions = character.CombatActions.FindAll(x => x.ActionType == type);

        return availableActions[Random.Range(0, availableActions.Count)];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    public int CurHP;
    public int MaxHP;

    public bool isPlayer;

    public List<CombatAction> CombatActions = new List<CombatAction>();

    [SerializeField] private Character opponent;

    private Vector3 startPos;

    public event UnityAction OnHealthChange;
    //not subscribing to a specific instance, rather it just to know if a character died
    public static event UnityAction<Character> OnDie;
    

    void Start()
    {
        startPos = transform.position;
    }

    public void TakeDamage(int damageToTake)
    {
        CurHP -= damageToTake;

        //? is so that it will run IF it is not equal to null
        OnHealthChange?.Invoke();

        if(CurHP <= 0)
            Die();
    }

    void Die()
    {
        OnDie?.Invoke(this);
        Destroy(gameObject);
    }

    public void Heal(int healAmount)
    {
        CurHP += healAmount;

        OnHealthChange?.Invoke();

        //to catch if the player heals more than their current max hp
        if(CurHP > MaxHP)
            CurHP = MaxHP;
    }

    public void CastCombatAction(CombatAction combatAction)
    {
        if(combatAction.Damage > 0)
        {
            StartCoroutine(AttackOpponent(combatAction));
        }
        else if(combatAction.ProjectilePrefab != null)
        {
            GameObject proj = Instantiate(combatAction.ProjectilePrefab, transform.position, Quaternion.identity);
            proj.GetComponent<Projectile>().Initialize(opponent, TurnManager.Instance.EndTurn);
        }
        else if(combatAction.HealAmount > 0)
        {
            Heal(combatAction.HealAmount);
            TurnManager.Instance.EndTurn();
        }
    }

    IEnumerator AttackOpponent(CombatAction combatAction)
    {
        while(transform.position != opponent.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, opponent.transform.position, 50 * Time.deltaTime);
            yield return null;
        }

        opponent.TakeDamage(combatAction.Damage);

        while(transform.position != startPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, 20 * Time.deltaTime);
            yield return null;
        }

        TurnManager.Instance.EndTurn();

    }

    public float GetHealthPercentage()
    {
        return (float)CurHP/ (float)MaxHP;   
    }
}

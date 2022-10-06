using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private Character[] characters;
    [SerializeField] private float nextTurnDelay = 1.0f;

    private int curCharacterIndex = -1;

    //keeps track of which character is currently having their turn
    public Character CurrentCharacter;

    //this event will happen when it's a new character turn is up
    public event UnityAction<Character> OnBeginTurn;
    public event UnityAction<Character> OnEndTurn;

    public static TurnManager Instance;

    void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    void OnEnable()
    {
        Character.OnDie += OnCharacterDie;
    }

    void OnDisable()
    {
        Character.OnDie -= OnCharacterDie;
    }

    void Start()
    {
        BeginNextTurn();
    }

    public void BeginNextTurn()
    {
        curCharacterIndex++;

        if(curCharacterIndex == characters.Length)
            curCharacterIndex = 0;

        CurrentCharacter = characters[curCharacterIndex];
        //going to detect who's turn it is, and run respective AI if enemy
        OnBeginTurn?.Invoke(CurrentCharacter);
    }

    public void EndTurn()
    {
        OnEndTurn?.Invoke(CurrentCharacter);
        //makes it so that it doesnt happen right away, so it is smoother
        Invoke(nameof(BeginNextTurn), nextTurnDelay);
    }

    void OnCharacterDie(Character character)
    {
        if(character.isPlayer)
            Debug.Log("You lost!");
        else
            Debug.Log("You Win!");
    }
}

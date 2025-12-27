using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Player Stats")]
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _moveSpeed = 5f;

    [Space(10)]
    [Header("Damage Stats")]
    [SerializeField] private float _meleeDamage = 10f;
    [SerializeField] private float _rangedDamage = 10f;

    [Space(10)]
    [Header("Attack Stats")]
    [SerializeField] private float _attackSpeed = 1f;
    [SerializeField] private float _attackRange = 5f;
    [SerializeField] private float _critChance = 0.05f;

    [Space(10)]
    [Header("Not used for now")]
    [SerializeField] private int _lvl = 1;
    [SerializeField] private int _exp = 0;
    [SerializeField] private int _coins = 100;


    public float maxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }

    public float moveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }

    public float meleeDamage
    {
        get => _meleeDamage;
        set => _meleeDamage = value;
    }

    public float rangedDamage
    {
        get => _rangedDamage;
        set => _rangedDamage = value;
    }

    public float attackSpeed
    {
        get => _attackSpeed;
        set => _attackSpeed = value;
    }

    public float attackRange
    {
        get => _attackRange;
        set => _attackRange = value;
    }

    public float critChance
    {
        get => _critChance;
        set => _critChance = value;
    }

    public int lvl
    {
        get => _lvl;
        set => _lvl = value;
    }

    public int exp
    {
        get => _exp;
        set => _exp = value;
    }
    public int coins
    {
        get => _coins;
        set => _coins = value;
    }
}
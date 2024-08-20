using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class BaseShootingTower : BaseTower
{
    //Components
    [SerializeField]
    protected CircleCollider2D _circleCollider;


    protected float AttackSpeedModifier = 1.00f;
    protected float DamageModifier = 1.00f;
    protected float RangeModifier = 1.00f;

    public string TowerName => _scriptableTower.TowerName;
    public float BaseAttackSpeed => _scriptableTower.BaseAttackSpeed;
    public int BaseDamage => _scriptableTower.BaseDamage;
    public float BaseRange => _scriptableTower.BaseRange;

    public float AttackSpeed => BaseAttackSpeed * AttackSpeedModifier;
    public int Damage => (int)(BaseDamage * DamageModifier);
    public float Range => BaseRange * RangeModifier;


    protected BaseEnemy _target = null;
    protected bool _attackIsOnCooldown = false;


    protected void Awake()
    {
        if(_circleCollider == null)
            _circleCollider = GetComponent<CircleCollider2D>();
        _circleCollider.isTrigger = true;
        _circleCollider.radius = Range;
    }


    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (_target != null)
            return;
        FindTarget(collision);
    }
    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (_target != null)
        {
            Attack();
            return;
        }
        else
            FindTarget(collision);
    }

    protected void FindTarget(Collider2D collision)
    {
        if (_target != null)
            return;
        if(collision.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
            _target = enemy;
    }
    protected void Attack()
    {
        if(_attackIsOnCooldown)
            return;

        _target.GetDamaged(Damage);

        _attackIsOnCooldown = true;
        _spriteRenderer.color = Color.blue;
        StartCoroutine(StartAttackCooldown());
    }
    protected IEnumerator StartAttackCooldown()
    {
        yield return new WaitForSeconds(AttackSpeed);
        _spriteRenderer.color = Color.cyan;
        _attackIsOnCooldown = false;
    }
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}

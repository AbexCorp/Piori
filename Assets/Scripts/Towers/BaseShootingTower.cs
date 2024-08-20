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



    protected void OnTriggerStay2D(Collider2D collision)
    {
        if(_attackIsOnCooldown)
            return;
        if (_target != null && collision.gameObject != _target.gameObject)
            return;

        FindTarget(collision);
        Attack();
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if(_target == null)
            return;
        if (collision.gameObject == _target.gameObject)
            _target = null;
    }

    protected void FindTarget(Collider2D collision)
    {
        if (_target == null)
        {
            collision.TryGetComponent<BaseEnemy>(out BaseEnemy enemy);
            _target = enemy;
        }
        if (_target == null)
            return;

        RaycastHit2D hit = Physics2D.Raycast
        (
            origin: gameObject.transform.position, 
            direction: collision.gameObject.transform.position - gameObject.transform.position, 
            distance: Vector2.Distance(gameObject.transform.position, collision.gameObject.transform.position),
            layerMask: LayerMask.GetMask(new string[] {"Enemy", "ObstacleFull"})
        );
        if (!hit)
            return;
        if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Enemy"))
            _target = null;
    }
    protected void Attack()
    {
        if(_attackIsOnCooldown || _target == null)
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _circleCollider.radius);

        if (_target == null) 
            return;

        Gizmos.DrawLine(transform.position, _target.gameObject.transform.position);
    }
}

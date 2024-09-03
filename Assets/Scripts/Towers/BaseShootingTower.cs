using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShootingTower : BaseTower
{
    protected float AttackSpeedModifier = 1.00f;
    protected float DamageModifier = 1.00f;
    protected float RangeModifier = 1.00f;

    public float BaseAttackSpeed => _scriptableTower.BaseAttackSpeed;
    public int BaseDamage => _scriptableTower.BaseDamage;
    public float BaseRange => _scriptableTower.BaseRange;

    public float AttackSpeed => BaseAttackSpeed * AttackSpeedModifier;
    public int Damage => (int)(BaseDamage * DamageModifier);
    public float Range => BaseRange * RangeModifier;


    protected BaseEnemy _target = null;
    protected bool _attackIsOnCooldown = false;
    protected int _obstacleLayer = 0;


    protected void Awake()
    {
        //if(_circleCollider == null)
        //    _circleCollider = GetComponent<CircleCollider2D>();
        //_circleCollider.isTrigger = true;
        //_circleCollider.radius = Range;
        _obstacleLayer = LayerMask.GetMask("ObstacleFull");
        StartCoroutine(Targeting());
    }


    protected IEnumerator Targeting()
    {
        YieldInstruction yield = new WaitForSeconds(0.075f);
        yield return new WaitForEndOfFrame();

        while (true)
        {
            if (_target != null)
                CheckIfTargetIsCorrect(_target.gameObject);
            else
            {
                foreach (var enemy in UnitManager.Instance.SpawnedEnemies)
                {
                    if (CheckIfTargetIsCorrect(enemy.gameObject))
                    {
                        _target = enemy;
                        break;
                    }
                    //else
                    //    yield return null;
                }
            }
            Attack();
            yield return yield;
        }
    }
    protected bool CheckIfTargetIsCorrect(GameObject target)
    {
        float distance = Vector2.Distance(transform.position, target.transform.position);
        if(distance > BaseRange)
        {
            _target = null;
            return false;
        }
        RaycastHit2D hit = Physics2D.Raycast
        (
            origin: gameObject.transform.position, 
            direction: target.transform.position - gameObject.transform.position, 
            distance: distance,
            layerMask: _obstacleLayer
        );
        if(hit)
        {
            _target = null;
            return false;
        }
        return true;
    }
    protected void Attack()
    {
        if(_attackIsOnCooldown || _target == null)
            return;

        _target.GetDamaged(Damage);

        _attackIsOnCooldown = true;
        _spriteRenderer.color = Color.gray;
        StartCoroutine(StartAttackCooldown());
    }
    protected IEnumerator StartAttackCooldown()
    {
        yield return new WaitForSeconds(AttackSpeed);
        _spriteRenderer.color = Color.white;
        _attackIsOnCooldown = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, BaseRange);

        if (_target == null) 
            return;

        Gizmos.DrawLine(transform.position, _target.gameObject.transform.position);
    }
}

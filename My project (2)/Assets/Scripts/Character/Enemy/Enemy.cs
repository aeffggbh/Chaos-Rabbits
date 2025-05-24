using UnityEngine;
/// <summary>
/// Dudas:
/// - La pistola se resizea cuando la agarro y la droppeo muy rapido
/// - No logro que dispare donde apunta el crosshair (bala por instancia)
/// - El enemigo esta medio curseado cuando le digo que mire para algun lado
/// </summary>


[RequireComponent(typeof(Rigidbody))]
public abstract class Enemy : Character
{
    protected enum States
    {
        NONE,
        IDLE,
        PATROL,
        CHASE,
        ATTACK
    }

    //TODO Hacerlo service locator / singleton
    public EnemyManager _manager;
    public BoxCollider _collider;
    protected PlayerController _playerController;
    protected Transform _head;
    protected Rigidbody _rb;
    protected Vector3 _targetWalk;
    protected Vector3 _targetLook;
    protected Vector3 _moveDir;
    protected bool _pausedPatrol;
    protected float _moveSpeed;
    protected float _attackRange;
    protected float _timeSinceAttacked;
    protected float _chaseRange;
    protected float _chasingSpeed;
    protected float _patrolSpeed;
    protected float _patrolTimer;
    protected float _patrolCurrentTime;
    protected float _idleTimer;
    protected float _idleCurrentTime;
    protected Vector3 _counterMovement;
    protected AnimationController animationController;
    protected LookAtTarget _lookAtTrarget;
    protected States currentState = States.PATROL;

    virtual protected void Start()
    {
        maxHealth = 100.0f;
        currentHealth = maxHealth;

        damage = 10.0f;

        IsWeaponUser = false;
        Obj = gameObject;
        
        _manager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        _collider = gameObject.GetComponent<BoxCollider>();
        _head = transform.Find("head"); ;
        _lookAtTrarget = gameObject.AddComponent<LookAtTarget>();

        _rb = gameObject.GetComponent<Rigidbody>();

        if (_manager != null)
        {
            _manager.enemies.Add(this);
            _patrolTimer = _manager.patrolTimer;
            _attackRange = _manager.attackRange;
            _chaseRange = _manager.chaseRange;
            _playerController = _manager.playerController;
            _patrolSpeed = _manager.patrolSpeed;
            _chasingSpeed = _manager.chasingSpeed;
        }
        else
            Debug.LogError(nameof(EnemyManager) + " is null");

        _pausedPatrol = false;

        _idleTimer = 2f;
        _idleCurrentTime = 0f;

        _timeSinceAttacked = 0;

        ActivatePatrol();
    }

    protected float GetPlayerDistance()
    {
        return Vector3.Distance(transform.position, _playerController.transform.position);
    }

    protected Vector3 GetPlayerDirection()
    {
        Vector3 playerDir = (_playerController.transform.position - transform.position).normalized;
        //para que no vuele
        playerDir.y = 0;
        return playerDir;
    }

    virtual protected void FixedUpdate()
    {
        _counterMovement = new Vector3
                       (-_rb.linearVelocity.x * _manager.counterMovementForce,
                       0,
                       -_rb.linearVelocity.z * _manager.counterMovementForce);
        Move();

        CheckTimers();

        CheckRange();

        switch (currentState)
        {
            case States.IDLE:
                Idle();
                break;
            case States.PATROL:
                Patrol();
                break;
            case States.CHASE:
                Chase();
                break;
            case States.ATTACK:
                Attack();
                _targetLook = _playerController.transform.position;
                break;
            default:
                break;
        }

        if (this.GetComponent<ExplodingEnemy>() == null)
            _lookAtTrarget.Look(_targetLook);
    }

    private void Idle()
    {
        if (_idleCurrentTime > _idleTimer)
        {
            _idleCurrentTime = 0;
            currentState = States.PATROL;
            ActivatePatrol();
        }
    }

    private void Patrol()
    {
        if (Vector3.Distance(transform.position, _targetWalk) < 0.5f || _patrolCurrentTime > _patrolTimer)
        {
            _patrolCurrentTime = 0;
            _moveSpeed = 0;
            _moveDir = Vector3.zero;
            currentState = States.IDLE;
            ActivateIdle();
        }
    }
    private void CheckTimers()
    {
        switch (currentState)
        {
            case States.IDLE:
                _idleCurrentTime += Time.fixedDeltaTime;
                break;
            case States.PATROL:
                _patrolCurrentTime += Time.fixedDeltaTime;
                break;
            case States.CHASE:
                break;
            case States.ATTACK:
                _timeSinceAttacked += Time.fixedDeltaTime;
                break;
            default:
                break;
        }
    }

    private void Chase()
    {
        //Debug.Log("Player direction:" + GetPlayerDirection());
        _moveDir = GetPlayerDirection();

        _targetLook = _playerController.transform.position;
    }


    private void CheckRange()
    {
        if (GetPlayerDistance() <= _attackRange && currentState != States.ATTACK)
        {
            _timeSinceAttacked = 0;
            ActivateAttack();
            currentState = States.ATTACK;
        }
        else if (GetPlayerDistance() > _attackRange)
            currentState = States.NONE;

        if (currentState != States.CHASE && GetPlayerDistance() <= _chaseRange && currentState != States.ATTACK)
        {
            currentState = States.CHASE;
            ActivateChase();
        }
        if (GetPlayerDistance() >= _chaseRange && currentState != States.PATROL)
        {
            if (currentState != States.IDLE)
            {
                currentState = States.PATROL;
                ActivatePatrol();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _moveDir * 2);
    }

    protected abstract void ActivateIdle();

    virtual protected void ActivatePatrol()
    {
        _patrolCurrentTime = 0;
        float randomZ = UnityEngine.Random.Range(-_manager.walkRange, _manager.walkRange);
        float randomX = UnityEngine.Random.Range(-_manager.walkRange, _manager.walkRange);

        _targetWalk = new Vector3(transform.position.x + randomX,
                                     transform.position.y,
                                     transform.position.z + randomZ);

        _targetLook = _targetWalk;

        _moveDir = (_targetWalk - transform.position).normalized;
        _moveDir.y = 0;
        _moveSpeed = _patrolSpeed;
    }

    protected abstract void Move();
    protected abstract void ActivateChase();
    protected abstract void Attack();
    protected abstract void ActivateAttack();

    public override void Die()
    {
        //maybe take damage instead, lol
        Debug.Log("DIE");

        _manager.enemies.Remove(this);
        //gameObject.SetActive(false);
        //Destroy(gameObject);
    }

}

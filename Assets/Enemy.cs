using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivingMonoBehaviour
{
    [SerializeField] private float _speed = .5f;
    [SerializeField] private float _damage = 5f;
    [SerializeField] private Rigidbody2D _rigidBody;

    private float _actualSpeed = 0;
    protected bool _isFreezed;
    protected bool _isTornadoed;
    private GameObject _target;

    void Start()
    {
        _target = GameManager.Instance().GetTowerGameObject();
        SkillCaster.SkillFreezeStarted += Freeze;
        SkillCaster.SkillFreezeEnded += UnFreeze;

        SkillCaster.SkillTornadoStarted += StartTornadoMove;
        SkillCaster.SkillTornadoEnded += StopTornadoMove;
    }

    private void Update()
    {
        if (CanMove()) {

            if (_actualSpeed < _speed) {
                _actualSpeed += 0.1f * Time.deltaTime;
            }

            if (_actualSpeed > _speed) {
                _actualSpeed = _speed;
            }
        
            MoveForward();
            RotateTowardsTarget();
        }
    }

    protected virtual bool CanMove()
    {
      return !_isFreezed && !_isTornadoed; 
    }

    public float GetDamage() {
        return _damage;
    }
    
    private void MoveForward()
    {
        transform.position += transform.up * _actualSpeed * Time.deltaTime;
    }
    
    private void RotateTowardsTarget()
    {
        var offset = -90f;
        Vector2 direction = (Vector2) _target.transform.position - (Vector2) transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation= Quaternion.Slerp(transform.rotation, Quaternion.Euler(Vector3.forward * (angle + offset)), _actualSpeed / 15);
    }

    void OnCollisionEnter2D(Collision2D col) // todo where to properly handle collisions? i think it should be in wall, tower, shield objects, not here
    {
        if (col.gameObject.CompareTag("wall"))
        {
            GameManager.Instance().GetTower().AddMoney(5 * (int)_damage); // todo better money generating algorithm
            col.gameObject.GetComponent<Wall>().ReduceHealth(_damage);
            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag("tower")) {
            GameManager.Instance().GetTower().OnHitByEnemy(this);
            Destroy(gameObject);
        } 
        else if (col.gameObject.CompareTag("shield")) {
            HandleShieldCollision();
        }
    }

    protected virtual void HandleShieldCollision()
    {
       Destroy(gameObject);
    }

    public void Freeze() {
        _isFreezed = true;
        _rigidBody.velocity = new Vector2(0, 0);
        _rigidBody.angularVelocity = 0;
        _actualSpeed = 0;
    }

    public void UnFreeze() {
        _isFreezed = false;
    }

    public void StartTornadoMove() {
        _isTornadoed = true;
        BlowOffEffect();
    }

    protected void BlowOffEffect() {
        _actualSpeed = 0;
        _rigidBody.AddForce(transform.up * -3, ForceMode2D.Impulse);
        _rigidBody.AddTorque(2);
    }

    public void StopTornadoMove() {
        _isTornadoed = false;
    }
    private void OnDestroy () {
        
        SkillCaster.SkillFreezeStarted -= Freeze;
        SkillCaster.SkillFreezeEnded -= UnFreeze;

        SkillCaster.SkillTornadoStarted -= StartTornadoMove;
        SkillCaster.SkillTornadoEnded -= StopTornadoMove;
    }
}

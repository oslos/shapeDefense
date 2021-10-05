using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingMonoBehaviour : MonoBehaviour
{
    [SerializeField] private float _health = 100;
    [SerializeField] private float _maxHealth = 150;

    private float _originalHealth;

    void Awake() {
        _originalHealth = _health;
    }

    public void ReduceHealth(float amount) {
        
        _health -= Mathf.Abs(amount);

        Debug.Log("Current health - " + _health + "/" + _originalHealth);

        if (_health <= 0) {
            Die();
        }
    }

    public void IncreaseHealth(float amount) {
        _health += amount;
        ClampHealth();
    }

    public void SetHealth(float amount) {
        _health = amount;
        ClampHealth();
    }

    public void ResetHealth() {
        _health = _originalHealth;
    }

    public virtual void Die() {
        Destroy(gameObject);
    }

    private void ClampHealth() {
        _health = Mathf.Clamp(_health, 0, _maxHealth);
    }

    public float GetHealth() {
        return _health;
    }

    public float GetMaxHealth() {
        return _maxHealth;
    }
}

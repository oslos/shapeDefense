using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : LivingMonoBehaviour
{
    private int _money = 2345;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMoney(int money) {
        _money += money;
        UIManager.Instance().SetMoney(_money);
    }

    public void OnHitByEnemy(Enemy enemy) {
        ReduceHealth(enemy.GetDamage());
        UIManager.Instance().SetHealth(GetHealth());
    }

    public override void Die() {
        // todo game over
    }

    public int GetMoney() {
        return _money;
    }
}

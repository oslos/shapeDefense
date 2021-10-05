using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private Text _waveText;
    [SerializeField] private Text _moneyText;

    private static UIManager _instance;

    public static UIManager Instance()
    {
        return _instance;
    }

    private void Awake()
    {
        if (!_instance)
            _instance = this;
    }

    public void SetHealth(float health) {
        _healthSlider.value = health;
    }

    public void SetMaxHealth(float maxHealth) {
        _healthSlider.maxValue = maxHealth;
    }

    public void SetWave(int wave) {
        _waveText.text = "WAVE " + wave;
    }

    public void SetMoney(int money) {
        _moneyText.text = string.Format("{0:#,##0}", money);
    }

    // Start is called before the first frame update
    void Start()
    {
        var tower = GameManager.Instance().GetTower();
        SetMaxHealth(tower.GetMaxHealth());
        SetHealth(tower.GetHealth());
        SetMoney(tower.GetMoney());
    }
}

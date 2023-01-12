using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject levelUpCanvas;
    [SerializeField] private GameObject hitParticlePrefab;
    [SerializeField] private int _maxHealth = 1000;
    [SerializeField] private int _damage = 5;
    [SerializeField] public float internalHitCD = 0.1f;
    public int damage { get { return _damage; } set { damage = _damage; } }
    public int maxHealth { get { return _maxHealth; } set { maxHealth = _maxHealth; } }
    public int currentHealth { get; set; }
    private HealthBar healthBar;
    private EXPBarScript expBar;
    private GameObject timer;
    public float normalAttackMultiplier = 0.42f;
    public float skillMultiplier = 1.11f;
    public float burstMultiplier = 1.12f;

    private float[] XianglingNormalAttackMultiplier = { 0.42f, 0.49f, 0.57f, 0.66f, 0.77f };
    private float[] XianglingSkillMultiplier = { 1.11f, 1.28f, 1.47f, 1.67f, 1.89f };
    private float[] XianglingBurstMultiplier = { 1.12f, 1.29f, 1.48f, 1.68f, 1.90f };
    private float[] XianglingBurstSpeed = { 2.5f, 3.1f, 3.7f, 4.3f, 5.0f };
    public float xianglingBurstAngularSpeed = 2.5f;

    private float[] YoimiyaNormalAttackMultiplier = { 1.059f, 1.203f, 1.371f, 1.564f, 1.78f };
    private float[] YoimiyaSkillMultiplier = { 1.379f, 1.424f, 1.477f, 1.529f, 1.588f };
    private float[] YoimiyaBurstMultiplier = { 1.22f, 1.40f, 1.62f, 1.83f, 2.07f };

    private int NALevel = 1, SkillLevel = 1, BurstLevel = 1;
    public int naLevel { get { return NALevel; } }
    public int skillLevel { get { return SkillLevel; } }
    public int burstLevel { get { return BurstLevel; } }

    private int _currentLevel = 1;
    public int CurrentLevel { get { return _currentLevel; } }
    private float _currentExp = 0f;
    public float CurrentExp { get { return _currentExp; } }
    private float _neededExp;
    public float NeededExp { get { return _neededExp; } }

    void Awake()
    {
        healthBar = GameObject.FindWithTag("HealthBar").GetComponent<HealthBar>();
        expBar = GameObject.FindWithTag("EXPBar").GetComponent<EXPBarScript>();
        timer = GameObject.FindWithTag("Timer");
        healthBar.SetMaxHealth(maxHealth);
        if (!GameManager.Instance.newGame)
        {
            GameSaveData.SaveData data = GameManager.Instance.saveManager.LoadGameBinary();
            currentHealth = data.health;
            _currentExp = data.currentExp;
            _currentLevel = data.playerLevel;
            NALevel = data.NALevel;
            SkillLevel = data.skillLevel;
            BurstLevel = data.burstLevel;
            XianglingController xianglingController = GetComponent<XianglingController>();
            if (xianglingController != null)
            {
                normalAttackMultiplier = XianglingNormalAttackMultiplier[NALevel - 1];
                skillMultiplier = XianglingSkillMultiplier[SkillLevel - 1];
                burstMultiplier = XianglingBurstMultiplier[BurstLevel - 1];

                xianglingController.IncreaseProjectileCount(NALevel);
                xianglingBurstAngularSpeed = XianglingBurstSpeed[BurstLevel - 1];
            }

            YoimiyaController yoimiyaController = GetComponent<YoimiyaController>();
            if (yoimiyaController != null)
            {
                normalAttackMultiplier = YoimiyaNormalAttackMultiplier[NALevel - 1];
                skillMultiplier = YoimiyaSkillMultiplier[SkillLevel - 1];
                burstMultiplier = YoimiyaBurstMultiplier[BurstLevel - 1];

                yoimiyaController.IncreaseProjectileCount(NALevel);
            }
        }
        else
        {
            currentHealth = maxHealth;
            _currentLevel = 1;
        }
        healthBar.SetHealth(currentHealth);
        _neededExp = Mathf.Pow((_currentLevel / 0.1f), 2);
        expBar.SetEXP(_currentExp, _neededExp, _currentLevel);
    }

    void Start()
    {
        levelUpCanvas.SetActive(false);
    }

    public void LevelUp()
    {
        _currentLevel++;
        _currentExp -= _neededExp;
        _neededExp = Mathf.Pow((_currentLevel / 0.1f), 2);
        expBar.SetEXP(_currentExp, _neededExp, _currentLevel);
        levelUpCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void AddExp(float exp)
    {
        _currentExp += exp;
        while (_currentExp >= _neededExp)
        {
            if (_currentLevel == 13)
                return;
            LevelUp();
        }
        expBar.SetEXP(_currentExp, _neededExp, _currentLevel);
    }

    public void UpgradeNormalAttack(string character)
    {
        if (NALevel == 5)
            return;
        NALevel++;
        if (character == "Xiangling")
        {
            normalAttackMultiplier = XianglingNormalAttackMultiplier[NALevel - 1];
            XianglingController xianglingController = GetComponent<XianglingController>();
            xianglingController.IncreaseProjectileCount(NALevel);
        }
        else if (character == "Yoimiya")
        {
            normalAttackMultiplier = YoimiyaNormalAttackMultiplier[NALevel - 1];
            YoimiyaController yoimiyaController = GetComponent<YoimiyaController>();
            yoimiyaController.IncreaseProjectileCount(NALevel);
        }
        levelUpCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    public void UpgradeSkill(string character)
    {
        if (SkillLevel == 5)
            return;
        SkillLevel++;
        if (character == "Xiangling")
        {
            skillMultiplier = XianglingSkillMultiplier[SkillLevel - 1];
        }
        else if (character == "Yoimiya")
        {
            skillMultiplier = YoimiyaSkillMultiplier[SkillLevel - 1];
        }
        levelUpCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    public void UpgradeBurst(string character)
    {
        if (BurstLevel == 5)
            return;
        BurstLevel++;
        if (character == "Xiangling")
        {
            burstMultiplier = XianglingBurstMultiplier[BurstLevel - 1];
            xianglingBurstAngularSpeed = XianglingBurstSpeed[BurstLevel - 1];
        }
        else if (character == "Yoimiya")
        {
            burstMultiplier = YoimiyaBurstMultiplier[BurstLevel - 1];
        }
        levelUpCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    public IEnumerator Damaged(int damage, System.Action<bool> callback)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        healthBar.SetHealth(currentHealth);
        HitParticle();
        if (currentHealth <= 0)
        {
            // Die
            int currentlv = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            timer.SetActive(true);
            GameManager.Instance.GameOver(currentlv, 0, maxHealth, 0f, 1, 1, 1, 1);
        }
        yield return new WaitForSeconds(internalHitCD);
        callback(true);
    }

    private void HitParticle()
    {
        GameObject particle = Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
        Destroy(particle, 1f);
    }

    public void IncreaseHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    public void OnObjectDestroy() { }
}

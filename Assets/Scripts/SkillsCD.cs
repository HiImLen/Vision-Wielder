using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsCD : MonoBehaviour
{
    [Header("Skill")]
    [SerializeField] private Image skill;
    public float skillCD;
    private bool isSkillCoolDown = false;
    [SerializeField] private KeyCode skillKey;

    [Header("Burst")]
    [SerializeField] private Image burst;
    public float burstCD;
    private bool isBurstCoolDown = false;
    [SerializeField] private KeyCode burstKey;

    // Start is called before the first frame update
    void Start()
    {
        skill.fillAmount = 0;
        burst.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Skill();
        Burst();
    }

    void Skill()
    {
        if (Input.GetKeyDown(skillKey) && !isSkillCoolDown)
        {
            isSkillCoolDown = true;
            skill.fillAmount = 1;
        }

        if (isSkillCoolDown)
        {
            skill.fillAmount -= 1 / skillCD * Time.deltaTime;
            if (skill.fillAmount <= 0)
            {
                isSkillCoolDown = false;
                skill.fillAmount = 0;
            }
        }
    }

    void Burst()
    {
        if (Input.GetKeyDown(burstKey) && !isBurstCoolDown)
        {
            isBurstCoolDown = true;
            burst.fillAmount = 1;
        }

        if (isBurstCoolDown)
        {
            burst.fillAmount -= 1 / burstCD * Time.deltaTime;
            if (burst.fillAmount <= 0)
            {
                isBurstCoolDown = false;
                burst.fillAmount = 0;
            }
        }
    }
}

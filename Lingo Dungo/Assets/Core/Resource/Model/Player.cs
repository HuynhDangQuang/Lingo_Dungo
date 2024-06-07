using Assets.Core.Resource.Skills.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Combatant
{


    public Player(PlayerClasses playerClass)
    {
        SetupClass(playerClass);
        UpdateStat();
        hp = maxHp;
    }

    public void SetupClass(PlayerClasses playerClass)
    {
        switch (playerClass)
        {
            case PlayerClasses.Knight:
                ownerClass = new Class(100, 100, 20, 20)
                {
                    normalAttack = new Knight_NormalAttack(),
                    primarySkill = new Knight_Block(),
                    secondarySkill = new Knight_Cleave(),
                };
                break;
        }
    }
}

public enum PlayerClasses
{
    Knight
}

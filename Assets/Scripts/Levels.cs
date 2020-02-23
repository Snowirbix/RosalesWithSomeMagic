using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Assertions;

public class Levels : NetworkBehaviour
{
    [ReadOnly][SyncVar(hook = "OnLevelUp")]
    public int level = 1;

    [ReadOnly][SyncVar(hook = "OnXpGained")]
    public int xp = 0;
    public int xpForNextLevel;

    public int[] tabXp = {10,25,45,70};

    public int levelMax;
    
    [System.Serializable]
    public class LevelUpEvent : UnityEvent<int,int> {};

    [System.Serializable]
    public class XpGainedEvent : UnityEvent<int, int, int> {};

    /// <summary>
    /// new level, levelMax
    /// </summary>
    public LevelUpEvent levelUpEvent;

    /// <summary>
    /// xp amount, xp, xpForNextLevel
    /// </summary>
    public XpGainedEvent xpGainedEvent;

    void Start()
    {
        level = 1;
        xp = 0;
        xpForNextLevel = tabXp[0];
        levelMax = tabXp.Length + 1;
    }

    public void XpGained(int xpGained)
    {
        if(level < levelMax)
        {
            int xpBeforeLevelUp = xpForNextLevel - xp;

            // On vérifie si on Level Up
            if(xpGained >= xpBeforeLevelUp)
            {
                // On reset l'xp à 0, On change l'xp nécessaire pour level up, on level up et on appel l'event associé
                xp = 0;
                LevelUp();
                xpGainedEvent.Invoke(xpBeforeLevelUp,xp,xpForNextLevel);
                // on fait du récursif avec le reste de l'xp gagné
                XpGained(xpGained - xpBeforeLevelUp);
            }
            else
            {
                xp += xpGained;
                xpGainedEvent.Invoke(xpGained,xp,xpForNextLevel);
            }
        }
    }

    public void LevelUp()
    {
        level++;
        if(level < levelMax)
        {
            Assert.IsTrue(level-1 < tabXp.Length, $" {gameObject.name} can't level up more than that !");
            xpForNextLevel = tabXp[level - 1];
        }
        levelUpEvent.Invoke(level,levelMax);
    }
    protected void OnLevelUp(int newLevel)
    {
        levelUpEvent.Invoke(newLevel,levelMax);
    }

    protected void OnXpGained(int xp)
    {

    }
}

using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

    /* State of the Art */
public class State : NetworkBehaviour
{
    public delegate void ModifierUpdate (Data data);
    
    public abstract class Modifier
    {
        public ModifierUpdate modifierUpdate;

        public Modifier ()
        {
            //
        }

        public Modifier (ModifierUpdate modifierUpdate)
        {
            this.modifierUpdate = modifierUpdate;
        }

        public void SetDelegate (ModifierUpdate modifierUpdate)
        {
            this.modifierUpdate = modifierUpdate;
        }

        public virtual bool Update (Data data, float deltaTime)
        {
            this.modifierUpdate(data);
            return true;
        }
    }

    public class TempModifier : Modifier
    {
        public float time;

        public TempModifier (float time) : base()
        {
            this.time = time;
        }

        public TempModifier (ModifierUpdate modifierUpdate, float time) : base(modifierUpdate)
        {
            this.time = time;
        }

        public override bool Update (Data data, float deltaTime)
        {
            time -= deltaTime;
            base.Update(data, deltaTime);
            return (time > 0);
        }
    }

    public class PermaModifier : Modifier
    {
        public bool active;

        public PermaModifier (bool active = true) : base()
        {
            this.active = active;
        }

        public PermaModifier (ModifierUpdate modifierUpdate, bool active = true) : base(modifierUpdate)
        {
            this.active = active;
        }

        public override bool Update (Data data, float deltaTime)
        {
            base.Update(data, deltaTime);
            return active;
        }
    }

    public class Data
    {
        public float speed = 1.0f;
        public float outputDamage = 1.0f;
        public float inputDamage = 1.0f;
        public float healing = 1.0f;
    }

    [SyncVar][ReadOnly]
    public bool silence = false;
    protected float silenceTimer;

    [SyncVar][ReadOnly]
    public bool root = false;
    protected float rootTimer;

    [SyncVar][ReadOnly]
    public bool invisible = false;
    protected float invisibleTimer;
    
    [SyncVar][ReadOnly]
    public bool invincible = false;
    protected float invincibleTimer;

    [SyncVar][ReadOnly]
    public float outputDamage = 1.0f;

    [SyncVar][ReadOnly]
    public float inputDamage = 1.0f;

    [SyncVar][ReadOnly]
    public float healing = 1.0f;

    [ReadOnly] // Local
    public float speed = 1.0f;

    protected List<Modifier> modifiers = new List<Modifier>();

    public void AddModifier (Modifier modifier)
    {
        if (!isServer)
        {
            Debug.LogError("Use AddLocalModifier instead");
        }
        modifiers.Add(modifier);
    }

    public void AddLocalModifier (Modifier modifier)
    {
        if (!isLocalPlayer)
        {
            Debug.LogError("Use AddModifier instead");
        }
        modifiers.Add(modifier);
    }

    private void Update ()
    {
        Data data = new Data();

        for (int i = modifiers.Count-1; i >= 0; i--)
        {
            if (modifiers[i].Update(data, Time.deltaTime) == false)
            {
                modifiers.RemoveAt(i);
            }
        }

        // handle speed locally
        if (isLocalPlayer)
        {
            if (data.speed != speed)
            {
                speed = data.speed;
            }
        }

        if (isServer)
        {
            // put data var in sync vars
            if (data.outputDamage != outputDamage)
            {
                outputDamage = data.outputDamage;
            }
            if (data.inputDamage != inputDamage)
            {
                inputDamage = data.inputDamage;
            }
            if (data.healing != healing)
            {
                healing = data.healing;
            }

            // update timers
            silenceTimer -= Time.deltaTime;
            rootTimer -= Time.deltaTime;
            invisibleTimer -= Time.deltaTime;
            invincibleTimer -= Time.deltaTime;

            // update states
            if (silenceTimer <= 0)
            {
                silence = false;
            }
            if (rootTimer <= 0)
            {
                root = false;
            }
            if (invisibleTimer <= 0)
            {
                invisible = false;
            }
            if (invincibleTimer <= 0)
            {
                invincible = false;
            }
        }
    }

    public void Silence (float time)
    {
        if (silence)
        {
            // keep the greater timer
            silenceTimer = (time > silenceTimer) ? time : silenceTimer;
        }
        else
        {
            silence = true;
            silenceTimer = time;
        }
    }

    public void Root (float time)
    {
        if (root)
        {
            rootTimer = (time > rootTimer) ? time : rootTimer;
        }
        else
        {
            root = true;
            rootTimer = time;
        }
    }

    public void Invisible (float time)
    {
        if (invisible)
        {
            invisibleTimer = (time > invisibleTimer) ? time : invisibleTimer;
        }
        else
        {
            invisible = true;
            invisibleTimer = time;
        }
    }

    public void Invincible (float time)
    {
        if (invincible)
        {
            invincibleTimer = (time > invincibleTimer) ? time : invincibleTimer;
        }
        else
        {
            invincible = true;
            invincibleTimer = time;
        }
    }
}

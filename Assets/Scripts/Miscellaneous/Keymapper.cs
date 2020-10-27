using System;
using UnityEngine;

public static class Keymapper
{
    public class NoKeyException : Exception
    {
        public NoKeyException() : base() { }
        public NoKeyException(object context) : base(string.Format("This is no key logged on this keymapper: {0}", context)) { }
    }
    public class Key
    {
        public string Name { get; private set; }
        public KeyCode KeyCode { get; private set; }
        
        EventManager.Event keyEvent;

        public Key(string name, KeyCode key)
        {
            Name = name;
            KeyCode = key;
            
        }
        public void SetKeyEvent(EventManager.Event keyEvent)
        {
            if(this.keyEvent == null)
                this.keyEvent = keyEvent;
        }

        public void ToggleKeyEvent()
        {
            if(keyEvent.HasListerners())
                keyEvent.Trigger();
        }

        public bool HasAttachedEvent() => keyEvent != null;
    }

    static Key[] loggedKeys;

    public static void Configure(params Key[] keys)
    {
        int size = keys.Length;
        
        loggedKeys = new Key[size];

        foreach (Key key in keys)
        {
            AddNewKey(key);
        }
    }

    static void AddNewKey(Key newKey)
    {
        for (int index = 0; index < loggedKeys.Length - 1; index++)
        {
            if (loggedKeys[index] == null)
            {
                loggedKeys[index] = newKey;
                return;
            }
        }
    }

    public static void ControlAction(string keyName, bool allowHold, Action onKeyPressed)
    {
        try
        {
            ControlAction(keyName, allowHold, onKeyPressed, null);
        }
        catch { }
    }

    public static void ControlAction(string keyName, bool allowHold, Action onKeyPressed, Action onKeyReleased)
    {
        try
        {
            //Use OnKey function if AllowHold is true
            if (allowHold ? OnKey(keyName) : OnKeyDown(keyName))
                onKeyPressed.Invoke();

            else if (OnKeyRelease(keyName))
                onKeyReleased.Invoke();
        }
        catch { }
    }

    public static bool OnKeyDown(string name, bool triggerEvent = false)
    {
        try
        {
            Key currentKey = GetKeyByName(name);

            if (currentKey == null) throw new NoKeyException(currentKey);

            if (currentKey.HasAttachedEvent() && triggerEvent)
                currentKey.ToggleKeyEvent();

            return Input.GetKeyDown(currentKey.KeyCode);
        }
        catch(NoKeyException noKey) {
            Debug.LogException(noKey);
            return false;
        }
    }

    public static bool OnKey(string name, bool triggerEvent = false)
    {
        try
        {
            Key currentKey = GetKeyByName(name);

            if (currentKey == null) throw new NoKeyException(currentKey);

            if (currentKey.HasAttachedEvent() && triggerEvent)
                currentKey.ToggleKeyEvent();

            return Input.GetKey(currentKey.KeyCode);
        }
        catch(NoKeyException noKey) { 
            Debug.LogException(noKey); 
            return false;
        }
    }

    public static bool OnKeyRelease(string name, bool triggerEvent = false)
    {
        try
        {
            Key currentKey = GetKeyByName(name);

            if (currentKey == null) throw new NoKeyException(currentKey);

            if (currentKey.HasAttachedEvent() && triggerEvent)
                currentKey.ToggleKeyEvent();

            return Input.GetKeyUp(currentKey.KeyCode);
        } catch(NoKeyException noKey) {
            Debug.LogException(noKey);
            return false;
        }
    }

    static Key GetKeyByName(string name)
    {
        for (int index = 0; index < loggedKeys.Length - 1; index++)
        {
            if (loggedKeys[index].Name.Equals(name))
                return loggedKeys[index];
        }
        return null;
    }

    static void RemoveKey(string name)
    {
        for(int index = 0; index < loggedKeys.Length - 1; index++)
        {
            if (loggedKeys[index].Name.Equals(name))
            {
                loggedKeys[index] = null;
                return;
            }
        }
    } 
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class KeyMessageSys
{
    Dictionary<string, InfaceMessage> MessageDictionary = new Dictionary<string, InfaceMessage>();

    public KeyMessageSys()
    {
        AddMessage(KeyCode.W.ToString(), new KeyWMessage());
        AddMessage(KeyCode.S.ToString(), new KeySMessage());
        AddMessage(KeyCode.A.ToString(), new KeyAMessage());
        AddMessage(KeyCode.D.ToString(), new KeyDMessage());
        AddMessage(KeyCode.J.ToString(), new KeyJMessage());
        AddMessage(KeyCode.L.ToString(), new KeyLMessage());
        AddMessage(KeyCode.I.ToString(), new KeyIMessage());
        AddMessage(KeyCode.K.ToString(), new KeyKMessage());
        AddMessage(KeyCode.LeftArrow.ToString(), new KeyLeftMessage());
        AddMessage(KeyCode.RightArrow.ToString(), new KeyRightMessage());
        AddMessage(KeyCode.UpArrow.ToString(), new KeyUpMessage());
        AddMessage(KeyCode.DownArrow.ToString(), new KeyDownMessage());
        AddMessage(KeyCode.Alpha1.ToString(), new KeyAlpha1Message());
        AddMessage(KeyCode.Alpha2.ToString(), new KeyAlpha2Message());
        AddMessage(KeyCode.Alpha3.ToString(), new KeyAlpha3Message());
        AddMessage(KeyCode.LeftControl.ToString(), new KeyCtrlMessage());
    }

    public void AddMessage(string key, InfaceMessage message)
    {
        MessageDictionary.Add(key, message);
    }

    public void SendMessage(string key)
    {
        if (!MessageDictionary.ContainsKey(key))
        {
            return;
        }

        InfaceMessage message = MessageDictionary[key];

        if(message == null)
        {
            return;
        }

        message.Execute();
    }
}


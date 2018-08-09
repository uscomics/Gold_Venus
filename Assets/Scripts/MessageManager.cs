﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Messages : MonoBehaviour
{
    public static string MSG_HOW_TO_MOVE = "Use AWSD, cursor keys, or movement pad to move.";
}

public class MessageManager : MonoBehaviour {
    public TextMeshProUGUI message;
    public CanvasGroup messageCanvasGroup;
    public int lifetime = 5; // seconds

    private Queue<string> messageQueue = new Queue<string>();
    private bool visible = false;
    private float startTime;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if ((true == visible)
        && (Time.time >= startTime + lifetime))
        {
            HideMessage();
        } // if
    }

    public void ShowMessage(string inMessage)
    {
        if (null == inMessage)
        {
            Debug.LogError("MessageManager::ShowMessage: Invalide data: inMessage: " + inMessage + ".");
            return;
        }
        if (true == visible)
        {
            messageQueue.Enqueue(inMessage);
            return;
        }
        visible = true;
        startTime = Time.time;
        message.text = inMessage;
        messageCanvasGroup.alpha = 1;
        messageCanvasGroup.interactable = true;
        messageCanvasGroup.blocksRaycasts = true;
    }

    void HideMessage()
    {
        messageCanvasGroup.alpha = 0;
        messageCanvasGroup.interactable = false;
        messageCanvasGroup.blocksRaycasts = false;
        visible = false;
        startTime = 0;
        if (0 == messageQueue.Count)
        {
            return;
        }
        string msg = messageQueue.Dequeue();
        ShowMessage(msg);
    }
}
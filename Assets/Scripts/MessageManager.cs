using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace USComics_Message_Manager
{

    public class Messages : MonoBehaviour
    {
        public static string MSG_HOW_TO_MOVE = "Use AWSD, cursor keys, or movement pad to move.";
        public static string MSG_HOW_TO_CLIMB = "Use the C key or movement menu to start climbing.";
        public static string MSG_HOW_TO_SNEAK = "Use Shift key or movement menu to sneak.";
        public static string MSG_HOW_TO_RUN = "Use Return or Enter key or movement menu to run.";
        public static string MSG_HOW_TO_WALK = "Use Esc or movement menu to walk.";
        public static string MSG_HOW_TO_STOP = "Use Backspace or delete keys or movement pad to Stop.";
        public static string MSG_NOTHING_TO_CLIMB = "There's nothing here to climb.";
        public static string MSG_ATTACK_TIMERS_CLEARED = "Attack timers cleared.";
        public static string MSG_ATTACK_DAMAGE_BONUS = "+2 Damage bonus.";
        public static string MSG_ATTACK_SUPER_BAR_BONUS = "+5 Super bonus.";
    }

    public class MessageManager : MonoBehaviour
    {
        public TextMeshProUGUI message;
        public CanvasGroup messageCanvasGroup;
        public int lifetime = 5; // seconds
        public AudioClip messageSound;

        private Queue<string> messageQueue = new Queue<string>();
        private bool visible = false;
        private float startTime;
        private AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            GameObject playerCharacter = GameObject.FindWithTag("MainCamera") as GameObject;
            if (null != playerCharacter) audioSource = playerCharacter.GetComponent<AudioSource>();
            if (null == audioSource) { Debug.LogError("MessageManager.Start: audioSource is null."); }
            if (null == audioSource) { return; }
        }

        // Update is called once per frame
        void Update()
        {
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
            if (null != messageSound)
            {
                audioSource.PlayOneShot(messageSound);
            }
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
}

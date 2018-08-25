using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking.NetworkSystem;

namespace USComics_Message_Manager {

    public class Messages : MonoBehaviour {
        public static string MSG_HOW_TO_MOVE = "Use AWSD, cursor keys, or movement pad to move.";
        public static string MSG_HOW_TO_CLIMB = "Some things can be climbed. Just walk up to one and you'll switch to climb mode. Use the Climb button or the C key to stop climbing.";
        public static string MSG_HOW_TO_SNEAK = "Use Shift key or Speed Bar to sneak.";
        public static string MSG_HOW_TO_RUN = "Use Return or Enter key or Speed Bar to run.";
        public static string MSG_HOW_TO_WALK = "Use Esc or Speed Bar to walk.";
        public static string MSG_HOW_TO_STOP = "Use Backspace or Delete keys, Speed Bar, or Movement Pad to Stop.";
        public static string MSG_HOW_TO_ATTACK = "Use the Attack Buttons or the 1, 2, 3, 4, and 9 keys to attack. The 9 key is your Super attack.";
        public static string MSG_ATTACK_CONTROLS = "Your attack controls will appear whenever an enemy is in sight.";
        public static string MSG_CONTROL_CAMERA = "Use Left and Right Mouse Buttons and Scroll Wheel to control the camera.";
        public static string MSG_SUPER_BAR = "Build up the Super Bar with attacks, then click the Super Bar for a super attack!";
        public static string MSG_ATTACK_TIMERS_CLEARED = "Attack timers cleared.";
        public static string MSG_ATTACK_DAMAGE_BONUS = "+2 Damage bonus.";
        public static string MSG_ATTACK_SUPER_BAR_BONUS = "+5 Super bonus.";
    }

    public class MessageManager : MonoBehaviour {
        public TextMeshProUGUI Message;
        public CanvasGroup MessageCanvasGroup;
        public Image ImageImage;
        public TextMeshProUGUI ImageMessage;
        public CanvasGroup ImageMessageCanvasGroup;
        public int Lifetime = 5; // seconds
        public int DefaultLifetime = 5; // seconds
        public AudioClip MessageSound;

        private Queue<QueuedMessage> MessageQueue = new Queue<QueuedMessage>();
        private Queue<QueuedMessage> ImageMessageQueue = new Queue<QueuedMessage>();
        private bool Visible = false;
        private float StartTime;
        private AudioSource AudioSource;

        // Use this for initialization
        void Start() {
            GameObject playerCharacter = GameObject.FindWithTag("MainCamera") as GameObject;
            if (null != playerCharacter) AudioSource = playerCharacter.GetComponent<AudioSource>();
            if (null == AudioSource) { Debug.LogError("MessageManager.Start: audioSource is null."); }
            if (null == AudioSource) { return; }
        }

        // Update is called once per frame
        void Update() {
            if ((true == Visible)
            && (Time.time >= StartTime + Lifetime))
            {
                HideMessage();
                HideImageMessage();
            } // if
        }

        public void ShowMessage(string inMessage, int inLifetime = -1) {
            if (null == inMessage) {
                Debug.LogError("MessageManager::ShowMessage: Invalide data: inMessage: " + inMessage + ".");
                return;
            }
            if (true == Visible) {
                MessageQueue.Enqueue(new QueuedMessage(inMessage, inLifetime));
                return;
            }
            if (-1 != inLifetime) Lifetime = inLifetime;
            else Lifetime = DefaultLifetime;
            Visible = true;
            StartTime = Time.time;
            Message.text = inMessage;
            MessageCanvasGroup.alpha = 1;
            MessageCanvasGroup.interactable = true;
            MessageCanvasGroup.blocksRaycasts = true;
            if (null != MessageSound) { AudioSource.PlayOneShot(MessageSound); }
        }

        void HideMessage() {
            MessageCanvasGroup.alpha = 0;
            MessageCanvasGroup.interactable = false;
            MessageCanvasGroup.blocksRaycasts = false;
            Visible = false;
            StartTime = 0;
            if (0 == MessageQueue.Count) { return; }
            QueuedMessage msg = MessageQueue.Dequeue();
            ShowMessage(msg.message, msg.lifetime);
        }

        public void ShowImageMessage(string inMessage, int inLifetime = -1) {
            if (null == inMessage) {
                Debug.LogError("MessageManager::ShowMessage: Invalide data: inMessage: " + inMessage + ".");
                return;
            }
            if (true == Visible) {
                ImageMessageQueue.Enqueue(new QueuedMessage(inMessage, inLifetime));
                return;
            }
            if (-1 != inLifetime) Lifetime = inLifetime;
            else Lifetime = DefaultLifetime;
            Visible = true;
            StartTime = Time.time;
            ImageMessage.text = inMessage;
            ImageMessageCanvasGroup.alpha = 1;
            ImageMessageCanvasGroup.interactable = true;
            ImageMessageCanvasGroup.blocksRaycasts = true;
            if (null != MessageSound) {  AudioSource.PlayOneShot(MessageSound); }
        }

        void HideImageMessage() {
            ImageMessageCanvasGroup.alpha = 0;
            ImageMessageCanvasGroup.interactable = false;
            ImageMessageCanvasGroup.blocksRaycasts = false;
            Visible = false;
            StartTime = 0;
            if (0 == ImageMessageQueue.Count) { return; }
            QueuedMessage msg = ImageMessageQueue.Dequeue();
            ShowImageMessage(msg.message, msg.lifetime);
        }
    }

    public class QueuedMessage {
        public QueuedMessage(string inMesaage, int inLifetime) {
            message = inMesaage;
            lifetime = inLifetime;
        }
        public string message;
        public int lifetime;
    }
}

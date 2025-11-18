using io.github.ykysnk.WorldBasic.Udon;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace io.github.ykysnk.AudioManager
{
    // Refs from Vowgan.Contact.ContactInstance
    [RequireComponent(typeof(AudioSource))]
    [AddComponentMenu("yky/Audio Manager/Simple Audio Instance")]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class SimpleAudioInstance : BasicUdonSharpBehaviour
    {
        public int index;
        public bool isPlaying;
        public bool isChilded;
        public float startTime;
        public float clipLength;
        public SimpleAudioPlayer audioPlayer;

        private AudioSource _source;

        private void Start()
        {
            _source = GetComponent<AudioSource>();
            _source.playOnAwake = false;
            gameObject.SetActive(false);
        }

        public bool PlaySound(AudioClip clip, Vector3 position, int key, float maxDistance = 32, float volume = 1,
            float pitch = 1)
        {
            if (!IsKeyCorrect(key)) return false;
            var player = Networking.LocalPlayer;
            if (!Utilities.IsValid(player)) return false;

            if (!Utilities.IsValid(clip) || Vector3.Distance(player.GetPosition(), position) > maxDistance)
            {
                ReturnToPool(RandomKey);
                return false;
            }

            isPlaying = true;
            startTime = Time.timeSinceLevelLoad;
            clipLength = clip.length;

            _source.clip = clip;
            _source.maxDistance = maxDistance;
            _source.volume = volume;
            _source.pitch = pitch;
            transform.position = position;
            gameObject.SetActive(true);
            _source.Play();

            return true;
        }

        public void ReturnToPool(int key)
        {
            if (!IsKeyCorrect(key)) return;
            isPlaying = false;

            if (isChilded)
            {
                isChilded = false;
                transform.SetParent(audioPlayer.transform);
                SetSiblingIndex(index);
            }

            startTime = 0;
            clipLength = 0;

            gameObject.SetActive(false);
        }

        public void ChildTo(Transform parent, int key)
        {
            if (!IsKeyCorrect(key)) return;
            isChilded = true;
            transform.SetParent(parent);
        }

        private void SetSiblingIndex(int index2) => transform.SetSiblingIndex(index2);
    }
}
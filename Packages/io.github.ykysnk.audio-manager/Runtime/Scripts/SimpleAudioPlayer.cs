using io.github.ykysnk.WorldBasic.Udon;
using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace io.github.ykysnk.AudioManager
{
    // Refs form Vowgan.Contact.ContactAudioPlayer
    [AddComponentMenu("yky/Audio Manager/Simple Audio Player")]
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    [PublicAPI]
    public class SimpleAudioPlayer : BasicUdonSharpBehaviour
    {
        private const int DefaultDistance = 3;

        public GameObject audioPrefab;
        public int sourceCount = 12;
        [HideInInspector] public SimpleAudioInstance[] instances;

        private int _lastPlayedIndex = -1;

        protected override bool LogShowFullName => false;

        public void Update()
        {
            foreach (var instance in instances)
            {
                if (!instance.isPlaying) continue;

                var instanceTime = Time.timeSinceLevelLoad - instance.startTime;

                if (instanceTime >= instance.clipLength)
                    instance.ReturnToPool(instance.RandomKey);
            }
        }

        public SimpleAudioInstance PlaySoundChilded(AudioClip clip, Transform parent, Vector3 position, int key,
            float maxDistance = DefaultDistance, float volume = 1, float pitch = 1)
        {
            if (!IsKeyCorrect(key)) return null;
            var instance = TryGetSource();
            if (!Utilities.IsValid(instance)) return null;

            instance.ChildTo(parent, instance.RandomKey);
            instance.PlaySound(clip, position, instance.RandomKey, maxDistance, volume, pitch);

            return instance;
        }

        public SimpleAudioInstance PlaySound([CanBeNull] AudioClip[] clips, Vector3 position, int key,
            float maxDistance = DefaultDistance, float volume = 1, float pitch = 1)
        {
            if (!IsKeyCorrect(key)) return null;
            if (clips == null || clips.Length == 0)
            {
                LogWarning("Tried to play null clip array.", this);
                return null;
            }

            var clip = clips[Random.Range(0, clips.Length)];

            var instance = PlaySound(clip, position, RandomKey, maxDistance, volume, pitch);
            return instance;
        }

        public SimpleAudioInstance PlaySound([CanBeNull] AudioClip clip, Vector3 position, int key,
            float maxDistance = DefaultDistance, float volume = 1, float pitch = 1)
        {
            if (!IsKeyCorrect(key)) return null;
            if (clip == null)
            {
                LogWarning("Tried to play null clip.", this);
                return null;
            }

            var instance = TryGetSource();
            if (!Utilities.IsValid(instance)) return null;

            return instance.PlaySound(clip, position, instance.RandomKey, maxDistance, volume, pitch)
                ? instance
                : null;
        }

        private SimpleAudioInstance TryGetSource()
        {
            for (var i = 0; i < sourceCount; i++)
            {
                _lastPlayedIndex++;
                if (_lastPlayedIndex >= sourceCount) _lastPlayedIndex = 0;

                var instance = instances[_lastPlayedIndex];
                if (instance.isPlaying) continue;

                return instance;
            }

            LogWarning("No available Audio Sources found.", this);
            return null;
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using VRC.SDKBase;
using Object = UnityEngine.Object;

namespace io.github.ykysnk.AudioManager.Editor;

public static class AudioManagerScenePostProcess
{
    private static SimpleAudioPlayer[] _simpleAudioPlayers =
    {
    };

    [PostProcessScene(-100)]
    public static void ScenePostProcess()
    {
        _simpleAudioPlayers = Object.FindObjectsOfType<SimpleAudioPlayer>();
        EffectPool();
    }

    private static void EffectPool()
    {
        foreach (var simpleAudioPlayer in _simpleAudioPlayers)
        {
            var sources = new List<SimpleAudioInstance>();

            for (var i = 0; i < simpleAudioPlayer.sourceCount; i++)
            {
                var audioObj = Object.Instantiate(simpleAudioPlayer.audioPrefab, simpleAudioPlayer.transform);
                audioObj.name = $"Audio Source {i}";
                var instance = audioObj.GetComponent<SimpleAudioInstance>();
                if (!Utilities.IsValid(instance))
                    throw new NullReferenceException($"Audio Source {i} is not a valid SimpleAudioInstance");
                instance.audioPlayer = simpleAudioPlayer;
                sources.Add(instance);
            }

            simpleAudioPlayer.instances = sources.ToArray();
        }
    }
}
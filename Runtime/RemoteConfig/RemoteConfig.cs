using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using UnityEngine.Scripting;

namespace VervePlace.YandexGames
{
    public static class RemoteConfig
    {
        private static readonly SerializableDictionary<string, string> DefaultFlags = new();
        private static readonly SerializableClientFeatures ClientFeatures = new();
        
        private static Action<IReadOnlyDictionary<string, string>> s_onGetFlagsSuccessCallback;
        private static Action<string> s_onGetFlagsErrorCallback;

        public static void GetFlags(Action<IReadOnlyDictionary<string, string>> onSuccessCallback, IReadOnlyDictionary<string, string> defaultFlags = null, IReadOnlyDictionary<string, string> clientFeatures = null, Action<string> onErrorCallback = null)
        {
            s_onGetFlagsSuccessCallback = onSuccessCallback;
            s_onGetFlagsErrorCallback = onErrorCallback;
            
            DefaultFlags.Dictionary.Clear();
            if (defaultFlags is not null)
                foreach (KeyValuePair<string, string> flag in defaultFlags)
                    DefaultFlags.Dictionary.Add(flag.Key, flag.Value);

            ClientFeatures.features.Clear();
            if (clientFeatures is not null)
                foreach (KeyValuePair<string, string> feature in clientFeatures)
                    ClientFeatures.features.Add(new ClientFeature{ name = feature.Key, value = feature.Value });

            string defaultFlagsJson = JsonUtility.ToJson(DefaultFlags);
            string clientFeaturesJson = JsonUtility.ToJson(ClientFeatures);
         
            RemoteConfigurationGetFlags(defaultFlagsJson, clientFeaturesJson, OnGetFlagsSuccessCallback, OnGetFlagsErrorCallback);
        }

        [DllImport("__Internal")]
        private static extern void RemoteConfigurationGetFlags(string defaultFlagsJson, string clientFeaturesJson, Action<string> resultCallbackPtr, Action<string> onErrorCallback);

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetFlagsSuccessCallback(string flagsJson)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(RemoteConfig)}.{nameof(OnGetFlagsSuccessCallback)} invoked");
            
            s_onGetFlagsSuccessCallback?.Invoke(JsonUtility.FromJson<SerializableDictionary<string, string>>(flagsJson).Dictionary);
        }

        [MonoPInvokeCallback(typeof(Action<string>))]
        private static void OnGetFlagsErrorCallback(string errorMessage)
        {
            if (YandexGamesSDK.CallbackLogging)
                Debug.Log($"{nameof(RemoteConfig)}.{nameof(OnGetFlagsErrorCallback)} invoked, {nameof(errorMessage)} = {errorMessage}");

            s_onGetFlagsErrorCallback?.Invoke(errorMessage);
        }
        
        [Serializable]
        private class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
        {
            public Dictionary<TKey, TValue> Dictionary = new();

            [Preserve] public TKey[] keys;
            [Preserve] public TValue[] values;

            public void OnBeforeSerialize()
            {
                keys = Dictionary.Keys.ToArray();
                values = Dictionary.Values.ToArray();
            }

            public void OnAfterDeserialize()
            {
                for (int i = 0; i < keys.Length; i++)
                    Dictionary.Add(keys[i], values[i]);
            }
        }
        
        [Serializable]
        private class SerializableClientFeatures
        {
            [Preserve]
            public List<ClientFeature> features = new();
        }
        
        [Serializable]
        private class ClientFeature
        {
            [Preserve] 
            public string name;
            [Preserve] 
            public string value;
        }
    }
}
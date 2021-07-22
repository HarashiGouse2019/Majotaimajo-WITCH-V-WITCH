using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

#region Extensions
namespace Extensions
{
    public static class Convenience
    {

        public static int ZeroBased(this int value)
        {
            return value - 1;
        }

        public static float ZeroBased(this float value)
        {
            return value - 1f;
        }

        public static double ZeroBased(this double value)
        {
            return value - 1f;
        }
    }


    public static class Coroutine
    {
        public static void Start(this IEnumerator enumerator)
        {
            CoroutineHandler.Execute(enumerator);
        }

        public static void Stop(this IEnumerator enumerator)
        {
            CoroutineHandler.Halt(enumerator);
        }
    }

    [ImmutableObject(true), Serializable]
    public struct Var<T>
    {
        public T Value;

        public Var(T value)
        {
            Value = value;
        }

        public static Var<T> operator +(Var<T> a, Var<T> b)
        {
            return (a + b);
        }

        public static Var<T> operator -(Var<T> a, Var<T> b)
        {
            return (a - b);
        }

        public static implicit operator Var<T>(T value)
        {
            return new Var<T>(value);
        }
    }

    public static class Dictionary
    {
        public static K GetKey<K, V>(this Dictionary<K, V> keyValuePairs, V value)
        {
            foreach (KeyValuePair<K, V> keyValuePair in keyValuePairs)
            {
                if (value.ToString() == keyValuePair.Value.ToString())
                    return keyValuePair.Key;
            }
            return default;
        }

        public static V GetValue<K, V>(this Dictionary<K, V> keyValuePairs, K key)
        {
            foreach (KeyValuePair<K, V> keyValuePair in keyValuePairs)
            {
                if (key.ToString() == keyValuePair.Key.ToString())
                    return keyValuePair.Value;
            }
            return default;
        }
    }

    public static class Boolean
    {
        public static int AsNumericValue(this bool boolean) => boolean ? 1 : 0;
        public static void Set(this bool _, bool value) { _ = value; }
    }

    public static class String
    {
        
        public static string TryConcat(this string _, params string[] strings)
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                foreach (string _string in strings)
                {
                    stringBuilder = stringBuilder.AppendLine(_string);
                }
                return stringBuilder.ToString();
            }
            catch (IOException e)
            {
                Debug.Log(string.Format("Failed to concatenate: {0}", e.Message));
                return string.Empty;
            }
        }
    }

    public static class File
    {
        public static void TryCopy(string sourceName, string destination)
        {
            try
            {
                System.IO.File.Copy(sourceName, destination);
            }
            catch (IOException e)
            {
                Debug.Log(string.Format("Failed to Copy File Info: {0}", e.Message));
                return;
            }
        }

    }        
}
#endregion

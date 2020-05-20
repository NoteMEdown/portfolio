using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{
    
    public static T[][] FromJson<T>(string[] json, int size)
    {
        Wrapper<T> wrapper = null;
        T[][] array = new T[json.Length * size][];
        T[][] tempArray = null;
        for (int i = 0; i < json.Length; i++)
        {
            wrapper = JsonUtility.FromJson<Wrapper<T>>(json[i]);
            tempArray = wrapper.DestructArray();
            for (int j = 0; j < size; j++)
            {
                array[(i* size) + j] = tempArray[j];
            }
        }
        return array;
    }

    public static string[] ToJson<T>(T[][] array, int size)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        string[] JsonStrings = new string[array.Length/size];
        T[][] tempArray = null;

        for (int i = 0; i < array.Length; i+=size)
        {
            tempArray = new T[size][];
            for (int j = 0; j < size && (i+j)< array.Length; j++)
            {
                tempArray[j] = array[i + j];
            }
            wrapper.CreateArray(tempArray);
            JsonStrings[i/size] = JsonUtility.ToJson(wrapper);
        }
        return JsonStrings;
    }

    public static string[] ToJson<T>(T[][] array, bool prettyPrint, int size)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        string[] JsonStrings = new string[array.Length];
        T[][] tempArray = null;

        for (int i = 0; i < array.Length; i += size)
        {
            tempArray = new T[size][];
            for (int j = 0; j < size && (i + j) < array.Length; j++)
            {
                tempArray[j] = array[i + j];
            }
            wrapper.CreateArray(tempArray);
            JsonStrings[i] = JsonUtility.ToJson(wrapper, prettyPrint);
        }
        return JsonStrings;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public string[] Items;

        public void CreateArray(T[][] array)
        {
            Items = new string[array.Length];
            for(int i=0; i < array.Length; i++)
            {
                Items[i] = JsonHelper2.ToJson(array[i]);
            }
        }
        public T[][] DestructArray()
        {
            T[][] array = new T[Items.Length][];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = JsonHelper2.FromJson<T>(Items[i]);
            }
            return array;
        }
    } 
}
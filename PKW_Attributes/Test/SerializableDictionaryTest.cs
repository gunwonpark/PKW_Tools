using System.Collections.Generic;
using UnityEngine;

namespace PKW_Attributes
{
    public class SerializableDictionaryTest : MonoBehaviour
    {
        [SerializeField]
        SerializableDictionary<int, string> testDictionary = new SerializableDictionary<int, string>();

        [SerializeField] private List<int> keys = new List<int>();
        [Button("Test")]
        private void Test()
        {
            testDictionary.TryAdd(1, "One");
            testDictionary.TryAdd(2, "Two");
            testDictionary.TryAdd(3, "Three");

            Debug.Log(testDictionary[1]);
            Debug.Log(testDictionary[2]);
            Debug.Log(testDictionary[3]);
        }
    }
}

using UnityEngine;

namespace PKW_Attributes
{
    public class ButtonAttributeTest : MonoBehaviour
    {
        public int testInt = 0;
        public const string testString = "Hello";

        [Button("TestMethod")]
        public void TestMethod()
        {
            Debug.Log("TestMethod");
        }

        [Button("TestMethod2", testString)]
        public void TestMethod2(string _test)
        {
            Debug.Log(_test);
        }

        [Button("TestMethod3")]
        public void TestMethod3()
        {
            testInt++;
            Debug.Log("TestMethod3");
        }
    }
}

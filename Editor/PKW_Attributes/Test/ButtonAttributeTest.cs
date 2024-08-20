using UnityEngine;

namespace PKW_Attributes
{
    public class ButtonAttributeTest : MonoBehaviour
    {
        public int testInt = 0;

        [Button("TestMethod")]
        public void TestMethod()
        {
            Debug.Log("TestMethod");
        }

        public void TestMethod2()
        {
            Debug.Log("TestMethod2");
        }

        [Button("TestMethod3")]
        public void TestMethod3()
        {
            testInt++;
            Debug.Log("TestMethod3");
        }
    }
}

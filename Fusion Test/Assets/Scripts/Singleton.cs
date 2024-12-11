using UnityEngine;

namespace Wiviart.Utilities
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (!instance)
                {
                    instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                }

                return instance;
            }
            set { instance = value; }
        }
    }
}
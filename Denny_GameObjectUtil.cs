using UnityEngine;
using UnityEngine.SceneManagement;

namespace Denny_TameMobsNS
{
    public static class Denny_GameObjectUtil
    {
        public static GameObject? FindByPathIncludeInactive(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            var parts = path.Split('/');
            if (parts.Length == 0) return null;

            // Includes inactive root objects
            var scene = SceneManager.GetActiveScene();
            var roots = scene.GetRootGameObjects();

            GameObject? currentGo = null;

            for (int i = 0; i < roots.Length; i++)
            {
                if (roots[i].name == parts[0])
                {
                    currentGo = roots[i];
                    break;
                }
            }

            if (currentGo == null) return null;

            Transform current = currentGo.transform;

            for (int p = 1; p < parts.Length; p++)
            {
                string want = parts[p];
                Transform? next = null;

                for (int c = 0; c < current.childCount; c++)
                {
                    var child = current.GetChild(c);
                    if (child.name == want)
                    {
                        next = child;
                        break;
                    }
                }

                if (next == null) return null;
                current = next;
            }

            return current.gameObject;
        }
    }
}
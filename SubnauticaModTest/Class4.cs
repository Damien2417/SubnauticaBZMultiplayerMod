using System.Collections;
using UnityEngine;

namespace SubnauticaModTest
{
    public partial class Enumerable
    {
        public static IEnumerator SetupNewGameObject(TechType objectTechType, System.Action<GameObject> callback = null)
        {
            CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(objectTechType,false);
            yield return task;
            GameObject gameObjectPrefab = task.GetResult();
            GameObject gameObject = global::Utils.CreatePrefab(gameObjectPrefab, 1, false);
            LargeWorldEntity.Register(gameObject);
            CrafterLogic.NotifyCraftEnd(gameObject, objectTechType);
            gameObject.SendMessage("StartConstruction", 1);
            yield return gameObject;
            if (callback != null) { callback.Invoke(gameObject); }
        }
    }
}
using ClientSubnautica.ClientManager;
using System.Collections;
using UnityEngine;

namespace ClientSubnautica
{
    public partial class Enumerable
    {
        public static IEnumerator SetupNewGameObject(TechType objectTechType, Vector3 vector2, string guid, System.Action<GameObject> callback = null)
        {
            CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(objectTechType,true);
            yield return task;
            GameObject gameObjectPrefab = task.GetResult();

            //Builder.BeginAsync(objectTechType);
            //GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(gameObjectPrefab);

            Vector3 toDirection = Vector3.up;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(gameObjectPrefab, vector2, Quaternion.FromToRotation(Vector3.up, toDirection));
            gameObject.AddComponent<UniqueGuid>();
            gameObject.GetComponent<UniqueGuid>().guid = guid;
            gameObject.SetActive(true);


			LargeWorldEntity.Register(gameObject);
            CrafterLogic.NotifyCraftEnd(gameObject, objectTechType);
            gameObject.SendMessage("StartConstruction", 1);
            if (callback != null) { callback.Invoke(gameObjectPrefab); }
        }
    }
}
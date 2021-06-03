using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public static EventManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // Collision Events

    public void TriggerEntered(Trigger trigger, Collider collider)
    {
        if(collider.CompareTag("PressurePlate"))
        {
            // Activate pressureplate here
        }
    }

    public void TriggerExit(Trigger trigger, Collider collider)
    {
        if (collider.CompareTag("PressurePlate"))
        {
            // Deactivate pressureplate here
        }
    }

    public void TriggerStay(Trigger trigger, Collider collider)
    {

    }

    public void CollisionEnter(Trigger trigger, Collision collider)
    {
        if (collider.transform.CompareTag("Bolder"))
        {
            GameObject bolder = collider.gameObject.transform.GetChild(0).gameObject;
            bolder.GetComponent<Bolder>().Slot();
            collider.transform.position = trigger.transform.position + trigger.collider.center;
            trigger.navObstacle.enabled = false;
            trigger.collider.enabled = false;
        }
    }

    public void CollisionExit(Trigger trigger, Collision collider)
    {

    }

    public void CollisionStay(Trigger trigger, Collision collider)
    {

    }

    public void OnProjectileHit(Projectile projectile, Vector3 projectileDir, Collision collision)
    {
        if (collision.transform.CompareTag("Entity"))
        {
            Entity entity = collision.gameObject.GetComponent<Entity>();
            entity.Hurt(projectileDir, projectile.damageVal, entity.knockedBackPower * 1f);
        }
    }

    // Utilities

    /// <summary>
    /// Used to instantiate prefab from non monobehaviour object
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public GameObject InstantiateGameObject(string prefab, Transform parent)
    {
        return Instantiate(Resources.Load(prefab) as GameObject, parent);
    }

    /// <summary>
    /// Used to instantiate prefab from non monobehaviour object without a parent
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public GameObject InstantiateGameObject(string prefab)
    {
        return Instantiate(Resources.Load(prefab) as GameObject);
    }

    /// <summary>
    /// Used to access Destroy from non monobehaviour object
    /// </summary>
    /// <param name="obj"></param>
    public void DestroyGameObject(GameObject obj) {
        Destroy(obj);
    }
   
    public void ExecuteCoroutine(IEnumerator method)
    {
        StartCoroutine(method);
    }

}

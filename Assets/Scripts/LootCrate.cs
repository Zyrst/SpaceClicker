using UnityEngine;
using System.Collections;

public class LootCrate : MonoBehaviour {

    public GameObject _enemyOwner;
    public GameObject _potion;

    private FMOD.Studio.EventInstance _crateSound;

	// Use this for initialization
	void Start () {
        _crateSound = FMOD_StudioSystem.instance.GetEvent(Sounds.Instance.uiSounds.lootcrate);
	}
	
	// Update is called once per frame
	void Update () {
        if (MouseController.Instance.buttonDown)
        {
            Ray ray = Camera.main.ScreenPointToRay(MouseController.Instance.position);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit) && !IsInvoking())
            {
                if (hit.collider.transform == transform && !_enemyOwner.GetComponent<Character>()._isAlive)
                {
                    Vector3 dir = (Vector3.up * 25f) + -(transform.position - Global.Instance.player.transform.position);
                    _potion.GetComponent<Rigidbody>().AddForce(dir * 20f);
                    Debug.Log(_crateSound.isValid());
                    _crateSound.start();
                    gameObject.SetActive(false);
                    Invoke("ActivatePotion", 0.3f);
                }
            }
        }
	}

    /// <summary>
    /// Associate a potion and enemy with loot crate
    /// </summary>
    /// <param name="enemy_">Enemy who needs to killed to activate crate</param>
    /// <param name="potion_">Potion inside the loot crate</param>
    public void Activate(GameObject enemy_, GameObject potion_)
    {
        _enemyOwner = enemy_;
        _potion = potion_;
    }

    /// <summary>
    /// Destroy both lootcrate and potion, used if new wave start
    /// </summary>
    public void UltimateDestroy()
    {
        GameObject.Destroy(gameObject);
        GameObject.Destroy(_potion.gameObject);
    }

    /// <summary>
    /// Activates the potion and destroys the lootcrate
    /// </summary>
    public void ActivatePotion()
    {
        _potion.GetComponent<HealthPotion>().staticPotion = false;
        GameObject.Destroy(gameObject);
        
    }
}

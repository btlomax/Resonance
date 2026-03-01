using Assets.Data.GameManagement;
using Assets.Player.Contracts;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : BaseMechanism
{
    [SerializeField] private bool _unlocked;

    private void Awake()
    {
        _unlocked = false;
    }

    public override void ActivateMechanism()
    {
        _unlocked = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(_unlocked && other.CompareTag("Player"))
        {
            GameEventDispatcher.Instance.TriggerEvent(GameUI_Event.LevelCompleted);

            StartCoroutine(WaitBeforeLoadMainMenu(1f));
        }
        else
            Debug.Log("Door is locked");
    }

    private IEnumerator WaitBeforeLoadMainMenu(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(0);
    }
}

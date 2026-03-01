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
            GameEventDispatcher.Instance.TriggerUIEvent(GameUI_Event.LevelCompleted);

            StartCoroutine(WaitBeforeLoadMainMenu(0.3f));
        }
        else
            Debug.Log("Door is locked");
    }

    private IEnumerator WaitBeforeLoadMainMenu(float delay)
    {
        GameEventDispatcher.Instance.TriggerFadeScreenEvent();

        yield return new WaitForSeconds(delay);
    }
}

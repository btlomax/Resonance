using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public void PlayFootstep()
    {
        AudioManager.Instance.Footstep.Post(gameObject);
    }
}

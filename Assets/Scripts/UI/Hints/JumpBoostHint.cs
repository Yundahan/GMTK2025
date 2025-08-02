using UnityEngine;

public class JumpBoostHint : Hint
{
    private PlayerActions player;

    void Awake()
    {
        player = FindFirstObjectByType<PlayerActions>();
    }

    void Update()
    {
        if (player.GetJumpBoosting())
        {
            HideMessage();
        }
    }
}

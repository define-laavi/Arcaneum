using UnityEngine;

public abstract class BulletBehaviour : SpaceObjectBehaviour
{
    protected bool _leftPlayer = false;

    protected override void OnEnabled()
    {
        _leftPlayer = false;
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        _leftPlayer = true; //left players body
    }

    public bool CanHitPlayer => _leftPlayer;
}

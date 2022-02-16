using UnityEngine;

public abstract class SpaceshipBulletBehaviour : SpaceObjectBehaviour
{
    protected bool _leftPlayer = false;

    private void OnCollisionExit2D(Collision2D col)
    {
        _leftPlayer = true; //left players body
    }
}

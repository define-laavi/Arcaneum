using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), DisallowMultipleComponent]
public abstract class SpaceObjectBehaviour : MonoBehaviour
{
    public string spaceObjectTag;

    private void Start(){ OnStart(); OnEnabled(); }
    private void OnEnable(){OnEnabled();}
    private void Update()
    {
        OnUpdate();
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.TryGetComponent<SpaceObjectBehaviour>(out var spaceObjectBehaviour))
            OnHit(col, spaceObjectBehaviour);
    }

    protected virtual void OnStart() {}
    protected virtual void OnEnabled(){}
    protected virtual void OnUpdate(){}
    protected virtual void OnHit(Collision2D col, SpaceObjectBehaviour colBehaviour){}
    public virtual void OnDestroy(){Destroy(this.gameObject);}
}

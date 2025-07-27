using UnityEngine;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public class Weapon_Sword : Weapon
{
    [SerializeField] private int Damage;
    [SerializeField] private float Speed;
    [Range(.1f, 5f)]
    [SerializeField] private float ActiveTime = .5f;
    private CancellationTokenSource Token;
    private Vector2 Target;

    public override void OnCarry_Enter()
    {
        base.OnCarry_Enter();
        LeanTween.cancel(gameObject);
    }

    public override void OnPrimaryUse_Release()
    {
        base.OnPrimaryUse_Release();
        Token = new CancellationTokenSource();
        _ = StartActiveTimer(Token);
    }

    public void FixedUpdate()
    {
        if (CurrentState == W_State.Primary)
            WeaponRigidBody.MovePosition(WeaponRigidBody.position + (Vector2)transform.right * Speed * Time.fixedDeltaTime);
    }

    public override void OnTriggerEnter2D(Collider2D Source)
    {
        if (CurrentState != W_State.Primary) return;
        base.OnTriggerEnter2D(Source);
        if (Source.tag == "Entity")
        {
            Source.TryGetComponent<Entity>(out var entity);
            if (entity == null) return;
            if (entity == null) return;
            if (!TargetTeam) if (entity.AssignedTeam == AssignedTeam) return;
                else if (entity.AssignedTeam != AssignedTeam) return;

            entity.TakeDamage(Damage);
        }

        if (Source.tag == "Environment")
        {
            OnExit();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        if (Token == null) return;
        Token.Cancel();
        Token = null;


        LeanTween.rotateAroundLocal(gameObject, Vector3.forward, 360f, .5f).setRepeat(-1);
        LeanTween.scale(gameObject, new Vector3(1.5f, 1.5f, 1f), 0.5f)
            .setEase(LeanTweenType.easeInQuad)
            .setUseEstimatedTime(true)
            .setLoopPingPong(1).setOnComplete(() => { gameObject.SetActive(false); });
        LeanTween.move(gameObject, transform.position + (-transform.right * .75f), .5f).setEase(LeanTweenType.easeOutQuad);
    }

    private async UniTask StartActiveTimer(CancellationTokenSource _Token)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(ActiveTime), cancellationToken: _Token.Token);
        OnExit();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using UnityEngine;

public class AIActionCircleAroundTarget : AIAction
{
    // Start is called before the first frame update
    [AddComponentMenu("TopDown Engine/Character/AI/Actions/AIActionCircleTarget2D")]

    public float MinimumDistance = 1f;

    public float Angle = 15f;
    public float TimeToChangeDirection = 1f;
    public float CircleRadius = 4f;

    protected CharacterMovement _characterMovement;
    protected float _timeSinceLastChange = 0f;
    protected bool _clockwise = true;
    protected Vector3 _circleTargetPosition;
   

    public override void Initialization()
    {
        if(!ShouldInitialize) return;
        base.Initialization();
        _characterMovement = this.gameObject.GetComponentInParent<Character>()?.FindAbility<CharacterMovement>();
    }

    public override void PerformAction()
    {
        CheckForDistance();
        MoveTowardsTarget();
    }

    private void CheckForDistance()
    {
        if ((transform.position - _circleTargetPosition).magnitude < MinimumDistance)
        {
            SetNewTarget();
        }
        
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
        SetNewTarget();
    }

    private void MoveTowardsTarget()
    {
        _characterMovement?.SetMovement(_circleTargetPosition - this.transform.position);
    }

    private void SetNewTarget()
    {
        if (_brain.Target == null) return;
        Vector2 v = (transform.position - _brain.Target.position).normalized;

        v = v.MMRotate(Angle * (_clockwise ? 1 : -1));
        Vector3 c = new Vector3(v.x, v.y, 0);
        _circleTargetPosition = _brain.Target.position + c * CircleRadius;

    }
    
    
    
    /// <summary>
    /// On exit state we stop our movement
    /// </summary>
    public override void OnExitState()
    {
        base.OnExitState();

        _characterMovement?.SetHorizontalMovement(0f);
        _characterMovement?.SetVerticalMovement(0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_circleTargetPosition, 0.5f);
    }
}

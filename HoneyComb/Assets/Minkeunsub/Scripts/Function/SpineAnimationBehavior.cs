using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineAnimationBehavior : StateMachineBehaviour
{

    public AnimationClip motion;
    string animationClip;

    public int layer = 0;
    public float timeScale = 1.0f;
    bool loop;
    SkeletonGraphic skeletonAnimation;
    Spine.AnimationState spineAnimationState;
    Spine.TrackEntry trackEntry;

    private void Awake()
    {
        if (motion != null)
            animationClip = motion.name;
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(skeletonAnimation == null)
        {
            skeletonAnimation = animator.GetComponent<SkeletonGraphic>();
            spineAnimationState = skeletonAnimation.AnimationState;
        }

        if(animationClip != null)
        {
            loop = stateInfo.loop;
            trackEntry = spineAnimationState.SetAnimation(layer, animationClip, loop);
            trackEntry.TimeScale = timeScale;
        }
    }
}

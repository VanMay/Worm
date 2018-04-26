using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Main.Public
{
    [TaskCategory("Main/Public")]
    public class PlayAnimationToEnd : Action
    {
        public SharedString StateName;
        public SharedInt LayerIndex = 0;

        private Animator anim;
        private AnimatorStateInfo stateInfo;
        private float duration;

        public override void OnStart()
        {
            anim = GetComponent<Animator>();
            stateInfo = anim.GetCurrentAnimatorStateInfo(LayerIndex.Value);
            if (!stateInfo.IsName(StateName.Value))
            {
                anim.CrossFade(StateName.Value, 0.25f);
            }
        }

        public override TaskStatus OnUpdate()
        {
            stateInfo = anim.GetCurrentAnimatorStateInfo(LayerIndex.Value);
            if (!stateInfo.IsName(StateName.Value))
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
    }
}
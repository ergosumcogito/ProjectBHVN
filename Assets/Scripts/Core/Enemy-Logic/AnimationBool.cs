using System;
using System.ComponentModel;

namespace Core.Enemy_Logic
{
    /*
     * Are used for setting the Entry AnimationState via Method setAnimationState found in EnemyAbstract class
     * Is mainly used in the Children Classes of BaseState class in the EnterState() Method
     */
    public enum AnimationBool
    {
        [Description("IsChasing")] IsChasing,

        [Description("IsAttacking")] IsAttacking,

        [Description("IsDead")] IsDead,

        [Description("IsInactive")] IsInactive,

        [Description("IsIdle")] IsIdle
        // add here more
    }

    public static class AnimationBoolExtensions
    {
        public static string GetAnimatorName(this AnimationBool animationBool)
        {
            var member = animationBool.GetType().GetField(animationBool.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(member,
                typeof(DescriptionAttribute));
            return attribute.Description;
        }
    }

    public struct AnimationStateChange
    {
        public AnimationBool state;
        public bool value;

        public AnimationStateChange(AnimationBool st, bool val)
        {
            state = st;
            value = val;
        }
    }
}
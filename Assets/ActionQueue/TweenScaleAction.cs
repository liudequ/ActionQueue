using System;
using UnityEngine;

namespace ActionQueue
{
    public class TweenScaleAction : TweenAction
    {
        [SerializeField]
        public Vector3 From;

        [SerializeField]
        public Vector3 To;


        public override void SetFrom(object from)
        {
            From = (Vector3)from;
        }

        public override void SetTo(object to)
        {
            To = (Vector3)To;
        }

        public override object GetFrom()
        {
            return this.From;
        }

        public override object GetTo()
        {
            return this.To;
        }

        protected override Type GetTweenerType()
        {
            return typeof(TweenScale);
        }

        protected override void ResetFromAndTo()
        {
            this.GetTweener<TweenScale>().@from = From;
            this.GetTweener<TweenScale>().to = To;
        }

        protected override void SetTargetFrom()
        {
            this.mTarget.transform.localScale = From;
        }

        protected override void SetTargetTo()
        {
            this.mTarget.transform.localScale = To;
        }
    }
}
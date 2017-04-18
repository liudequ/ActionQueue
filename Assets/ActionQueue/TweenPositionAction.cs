using System;
using System.Collections;
using UnityEngine;

namespace ActionQueue
{
    [System.Serializable]
    public class TweenPositionAction :TweenAction
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
            To = (Vector3) To;
        }

        protected override Type GetTweenerType()
        {
            return typeof(TweenPosition);
        }

        protected override void ResetFromAndTo()
        {
            this.GetTweener<TweenPosition>().from = From;
            this.GetTweener<TweenPosition>().to = To;
        }

        protected override void SetTargetFrom()
        {
            this.mTarget.transform.localPosition = From;
        }

        protected override void SetTargetTo()
        {
            this.mTarget.transform.localPosition = To;
        }

        public override object GetFrom()
        {
            return this.From;
        }

        public override object GetTo()
        {
            return this.To;
        }
    }
}
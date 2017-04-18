using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActionQueue
{
    [System.Serializable]
    public class TweenAlphaAction : TweenAction
    {
        [SerializeField]
        public float From;

        [SerializeField]
        public float To;


        public override void SetFrom(object from)
        {
            this.From = (float)from;
        }

        public override void SetTo(object to)
        {
            this.To = (float) to;
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
            return typeof(TweenAlpha);
        }

        protected override void ResetFromAndTo()
        {
            this.GetTweener<TweenAlpha>().from = this.From;
            this.GetTweener<TweenAlpha>().to = this.To;
        }

        protected override void SetTargetFrom()
        {
            this.mTarget.GetComponent<UIWidget>().alpha = this.From;
        }

        protected override void SetTargetTo()
        {
            this.mTarget.GetComponent<UIWidget>().alpha = this.To;
        }
    }


}


using UnityEngine;

namespace ActionQueue
{
    public class ActivateAction :ActionBase
    {
        [SerializeField]
        public bool From;

        [SerializeField]
        public bool To;


        public override void Reset()
        {
            base.Reset();
            if(!this.CanDo())return;
            this.mTarget.SetActive(this.From);
        }

        public override void Do()
        {
            base.Do();
            if(!this.CanDo())return;
            this.mTarget.SetActive(To);
            this.OnFinish();
        }

        protected override void SkipToDone()
        {
            if(!this.CanDo())return;
            this.mTarget.SetActive(To);
        }

        public override void SetFrom(object @from)
        {
            this.From = (bool) from;
        }

        public override void SetTo(object to)
        {
            this.To = (bool) to;
        }

        public override object GetFrom()
        {
            return this.From;
        }

        public override object GetTo()
        {
            return this.To;
        }

        public override bool CanDo()
        {
            return !this.From.Equals(this.To);
        }
    }
}
using System.Collections;
using UnityEngine;

namespace ActionQueue
{
    public class DelayAction :ActionBase
    {
        [SerializeField]
        public float DelayTime;

        protected UnityEngine.Coroutine mOperator;


        public override void Do()
        {
            base.Do();
            this.mOperator = this.StartCoroutine(this.DelayFinish());
        }

        protected IEnumerator DelayFinish()
        {
            yield return new WaitForSeconds(this.DelayTime);
            this.OnFinish();
        }

        protected override void SkipToDone()
        {
            
        }

        protected override void Stop()
        {
            base.Stop();
            if (this.mOperator != null)
                this.StopCoroutine(this.mOperator);
        }

        public override void SetFrom(object @from)
        {
           
        }

        public override void SetTo(object to)
        {
           
        }

        public override object GetFrom()
        {
            return null;
        }

        public override object GetTo()
        {
            return null;
        }
    }
}
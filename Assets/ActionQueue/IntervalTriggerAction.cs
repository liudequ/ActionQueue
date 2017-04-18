using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActionQueue
{
    //todo 无法确认所有触发的动作，什么时候完成，就没法确认，该行为真实结束时机
    public class IntervalTriggerAction : ActionBase
    {
        [SerializeField]
        public List<ActionBase> TriggerActions;
        
        [SerializeField]
        public float IntervalTime;

        protected UnityEngine.Coroutine mOperator;
        private int mIndex = 0;


        public override void Reset()
        {
            base.Reset();
            if(!Application.isPlaying)return;
            List<ActionBase> del = new List<ActionBase>();
            for (int i = 0; i < TriggerActions.Count; i++)
            {
                var action = TriggerActions[i];
                if (action == null)
                {
                    del.Add(action);
                }
            }
            for (int i = 0; i < del.Count; i++)
            {
                var actionBase = del[i];
                this.TriggerActions.Remove(actionBase);
            }
        }

        public override void Do()
        {
            base.Do();
            this.mOperator = this.StartCoroutine(TriggerAction());
        }

        protected IEnumerator TriggerAction()
        {
            while (mIndex<this.TriggerActions.Count)
            {
                if (this.TriggerActions[mIndex].CanDo())
                {
                    yield return new WaitForSeconds(this.IntervalTime);
                    this.TriggerActions[mIndex].PrevFinish();
                }
                mIndex++;
            }
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
            while (mIndex<this.TriggerActions.Count)
            {
                this.TriggerActions[mIndex].PrevFinish();
                mIndex++;
            }
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
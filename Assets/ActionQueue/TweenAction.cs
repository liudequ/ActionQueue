using System;
using System.Collections;
using UnityEngine;

namespace ActionQueue
{
    [System.Serializable]
    public abstract class TweenAction : ActionBase
    {
        [SerializeField]
        public float Duration = 1;

        [SerializeField]
        public AnimationCurve Curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

        protected UnityEngine.Coroutine mOperator;

        public UITweener Tween
        {
            get
            {
                if (this.mTween != null) return mTween;
                if (this.mTarget == null) return null;
                return this.GetTweener<UITweener>();
            }
            set { this.mTween = value; }
        }

        protected UITweener mTween;

        protected T GetTweener<T>() where T : UITweener
        {
            Type type = this.GetTweenerType();

            mTween = this.mTarget.GetComponent(type) as T;
            if (mTween == null)
                mTween = this.mTarget.AddComponent(type) as T;
            return (T)mTween;
        }

        protected abstract Type GetTweenerType();


        public override void Reset()
        {
            base.Reset();
            if (!Application.isPlaying) return;
            this.Tween.enabled = false;
            this.ResetFromAndTo();
            this.Tween.ResetToBeginning();
            this.Tween.duration = this.Duration;
            this.Tween.animationCurve = Curve;
            this.SetTargetFrom();
        }


        protected abstract void ResetFromAndTo();

        protected abstract void SetTargetFrom();

        protected abstract void SetTargetTo();

        public override void Do()
        {
            base.Do();
            this.Tween.enabled = true;
            this.Tween.PlayForward();
            this.mOperator = this.StartCoroutine(this.TweenFinish());
        }


        private IEnumerator TweenFinish()
        {
            yield return new WaitForSeconds(this.Tween.duration);
            this.mOperator = null;
            this.OnFinish();
        }

        protected override void SkipToDone()
        {
            this.SetTargetTo();
        }


        protected override void Stop()
        {
            base.Stop();
            this.Tween.enabled = false;
            if (this.mOperator != null)
                this.StopCoroutine(this.mOperator);
        }

    }
}
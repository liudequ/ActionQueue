using System;
using UnityEngine;

namespace ActionQueue
{
    public class NumberRollAction : ActionBase
    {
        public enum RollType
        {
            Step = 1,
            TotalTime
        }

        [SerializeField]
        public RollType Type;

        [SerializeField]
        public float From;

        [SerializeField]
        public float To;

        /// <summary>
        /// 每步变多少
        /// </summary>
        [SerializeField]
        public float Steps;

        /// <summary>
        /// 每步间隔几帧
        /// </summary>
        [SerializeField]
        public int StepSpeed = 1;

        /// <summary>
        /// 总时间模式，总共播多少帧
        /// </summary>
        [SerializeField]
        public float TotalTime;

        /// <summary>
        /// 时间模式时，是否整数变化
        /// </summary>
        [SerializeField]
        public bool IsIntChange;

        /// <summary>
        /// 当前显示的值
        /// </summary>
        private float mCurValue;

        /// <summary>
        /// 当前步走过的帧数
        /// </summary>
        private int mCurStep;

        /// <summary>
        /// 效果是否为递增
        /// </summary>
        private bool mIsAdd = true;

        /// <summary>
        /// 开始行为时时间
        /// </summary>
        private float mStartTime;

        public UILabel Label
        {
            get
            {
                if (this.mLabel != null) return this.mLabel;
                if (this.mTarget == null) return null;
                this.mLabel = this.mTarget.GetComponent<UILabel>();
                return this.mLabel;
            }
        }

        protected UILabel mLabel;

        public override void Reset()
        {
            base.Reset();
            if(!Application.isPlaying)return;
            if (this.Type == RollType.Step)
            {
                this.mCurStep = 0;
                if (this.From > this.To)
                {
                    this.mIsAdd = false;
                    this.Steps = -this.Steps;
                }
            }
            this.SetValue(this.From);
        }

        public override void Do()
        {
            this.mStartTime = Time.realtimeSinceStartup;
            base.Do();
        }


        public override void OnTick()
        {
            base.OnTick();
            if (this.mIsPlaying)
            {
                if (this.Type == RollType.Step)
                {
                    this.mCurStep++;
                    if (this.mCurStep == this.StepSpeed)
                    {
                        this.mCurStep = 0;
                        this.SetValue(this.mCurValue + this.Steps);
                    }
                }
                else if(this.Type == RollType.TotalTime)
                {
                    var passTime = Time.realtimeSinceStartup - this.mStartTime;
                    var value = Mathf.Lerp(this.From, this.To, (passTime/this.TotalTime));
                    this.SetValue(value);
                }
                this.CheckFinish();
            }
        }

        protected void CheckFinish()
        {
            if (this.mCurValue == this.To)
            {
                this.OnFinish();
            }
        }

        protected void SetValue(float value)
        {
            if (this.mIsAdd && value > this.To)
            {
                value = this.To;
            }
            else if (!this.mIsAdd && value < this.To)
            {
                value = this.To;
            }
            if (this.IsIntChange)
            {
                this.Label.text = value.ToString("F0");
            }
            else
            {
                this.Label.text = value.ToString("F2");
            }
            
            this.mCurValue = value;
        }


        protected override void SkipToDone()
        {
            this.SetValue(this.To);
        }

        public override void SetFrom(object from)
        {
            this.From = (float)from;
        }

        public override void SetTo(object to)
        {
            this.To = (float)to;
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
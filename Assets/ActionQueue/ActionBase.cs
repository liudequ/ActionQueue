using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActionQueue
{

    public class ActionData
    {

        public int ActionID { get; set; }

        public object From { get; set; }
        public object To { get; set; }

        public Action<ActionBase> FinishAction { get; set; }

    }
    //todo 添加父节点概念
    [System.Serializable]
    public class ActionBase 
    {
        public static int StartID = -1;
        public static int EndId = int.MaxValue;

        #region 编译器使用数据
#if UNITY_EDITOR
        [SerializeField]
        public Rect position;

        public virtual int color
        {
            get { return this.mColor; }
            set { this.mColor = value; }
        }

        [SerializeField]
        protected int mColor;

        [SerializeField]
        protected string name;

        public virtual string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
            }
        }
#endif

        #endregion

        public List<int> PrevIDs { get { return this.mPrevIDs; } }

        public int ID
        {
            get { return this.mID; }
        }

        public List<int> NextIDs { get { return this.mNextIDs; } }

        public ActionData Data
        {
            get
            {
                return this.mData;
            }
            set
            {
                this.mData = value;
                if (this.mData != null && this.mData.From != null) //数据内有相应值，则使用数据内值，否则使用序列化保存的值
                    this.SetFrom(this.mData.From);
                if (this.mData != null && this.mData.To != null)
                    this.SetTo(this.mData.To);
                this.Reset();
            }
        }

        public GameObject Target { get { return this.mTarget; } set { this.mTarget = value; } }


        public bool IsSkip
        {
            get { return this.mIsSkip; }
            set
            {
                this.mIsSkip = value;
                if (this.mIsSkip && this.mIsPlaying)
                    this.Skip();
            }
        }

        [SerializeField]
        protected int mID;

        [SerializeField]
        protected List<int> mNextIDs;

        [SerializeField]
        protected List<int> mPrevIDs;

        /// <summary>
        /// 当前动作开始条件
        /// 当前的动作开始，需要由 几个前置动作完成 触发
        /// </summary>
        [SerializeField]
        protected int mStartCondition;

        protected int mCurStartCondition;

        [SerializeField]
        protected GameObject mTarget;

        protected ActionData mData;

        protected List<Action<ActionBase>> mFinishActions;

        protected bool mIsFinish = false;

        protected bool mIsPlaying = false;

        protected bool mIsSkip = false;



        #region ADD REMOVE

        public void AddFinishAction(Action<ActionBase> finishAction)
        {
            if (this.mFinishActions == null)
                this.mFinishActions = new List<Action<ActionBase>>();
            this.mFinishActions.Add(finishAction);
        }

        public void AddPrevID(int id)
        {
            if (this.mPrevIDs == null)
                this.mPrevIDs = new List<int>();
            if (!this.mPrevIDs.Contains(id))
                this.mPrevIDs.Add(id);
            this.mStartCondition = this.mPrevIDs.Count;
        }

        public void RemovePrevID(int id)
        {
            if (this.mPrevIDs == null) return;
            this.mPrevIDs.Remove(id);
            this.mStartCondition = this.mPrevIDs.Count;
        }

        public void AddNextID(int id)
        {
            if (this.mNextIDs == null)
                this.mNextIDs = new List<int>();
            if (!this.mNextIDs.Contains(id))
                this.mNextIDs.Add(id);
        }

        public void RemoveNextID(int id)
        {
            if (this.mNextIDs == null) return;
            this.mNextIDs.Remove(id);
        }


        #endregion


        public virtual void OnTick()
        {

        }

        public virtual void Reset()
        {
            this.mCurStartCondition = this.mStartCondition;
            this.mIsFinish = false;
            this.mIsPlaying = false;
            this.mIsSkip = false;
        }

        public virtual void Do()
        {
            this.mIsPlaying = true;
            LogDebug(string.Format("Do Action : {0},Target is {1},ID is {2},from is {3},to is {4}", this, this.mTarget, this.mID, this.GetFrom(), this.GetTo()));
        }

        protected virtual void Stop()
        {
            LogDebug(string.Format("Stop Action : {0},Target is {1},ID is {2}", this, this.mTarget, this.mID));
        }

        protected virtual void SkipToDone()
        {
        }


        protected virtual void Skip()
        {
            if (this.mIsFinish) return;
            LogDebug(string.Format("Skip Action : {0},Target is {1},ID is {2}", this, this.mTarget, this.mID));

            this.mCurStartCondition = 0;
            this.mIsSkip = true;

            if (this.mIsPlaying)
                this.Stop();
            this.SkipToDone();
            this.OnFinish();
        }

        protected virtual void OnFinish()
        {
            LogDebug(string.Format("Finish Action : {0},Target is {1} ,ID is {2}", this, this.mTarget, this.mID));
            this.mIsFinish = true;
            this.mIsPlaying = false;
            this.DoFinishAction();
        }

        public virtual void PrevFinish()
        {
            this.mCurStartCondition--;
            LogDebug(string.Format("PrevFinish Action : {0},Target is {1},ID is {2},IsSkip : {3},mStartCondition : {4}", this, this.mTarget, this.mID, this.mIsSkip, this.mCurStartCondition));
            if (this.mCurStartCondition == 0)
            {
                if (this.mIsSkip)
                    this.Skip();
                else if (!this.CanDo())
                    this.OnFinish();
                else
                    this.Do();
            }
        }

        protected virtual void DoFinishAction()
        {
            if (this.mData != null && this.mData.FinishAction != null)
                this.mData.FinishAction(this);
            if (this.mFinishActions != null && this.mFinishActions.Count > 0)
            {
                for (int index = 0; index < this.mFinishActions.Count; index++)
                {
                    var finishAction = this.mFinishActions[index];
                    if (finishAction != null)
                        finishAction(this);
                }
            }
        }

        #region Coroutine

        public UnityEngine.Coroutine StartCoroutine(IEnumerator enumerator)
        {
            return this.mTarget.GetComponent<MonoBehaviour>().StartCoroutine(enumerator);
        }

        public void StopCoroutine(UnityEngine.Coroutine coroutine)
        {
            this.mTarget.GetComponent<MonoBehaviour>().StopCoroutine(coroutine);
        }

        #endregion


        #region Get/Set

        public virtual void SetID(int id)
        {
            this.mID = id;
        }

        public virtual void SetFrom(object from)
        {
        }


        public virtual void SetTo(object to)
        {
        }

        public virtual object GetFrom()
        {
            return null;
        }

        public virtual object GetTo()
        {
            return null;
        }

        public static bool GetIsStartOrEnd(int ActionID)
        {
            if (ActionID == StartID || ActionID == EndId)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 当前的行为是否在运行时可以被执行，
        /// 取决于写入数据时，from与to 是否相同
        /// </summary>
        /// <returns></returns>
        public virtual bool CanDo()
        {
            if (this.GetFrom() == null && this.GetTo() == null) return true;
            if (this.GetFrom() == this.GetTo())
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Log

        public static void LogDebug(string content)
        {
            Debug.Log(content);
        }

        public static void LogError(string content)
        {
            Debug.LogError(content);
        }

        //        public static void LogDebug(string content)
        //        {
        //            Log.Debug(content);
        //        }
        //
        //        public static void LogError(string content)
        //        {
        //            Log.Error(content);
        //        }
        #endregion

    }

}


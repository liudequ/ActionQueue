using UnityEngine;

namespace ActionQueue
{
    [System.Serializable]
    public class EndAction : ActionBase
    {
        public override int color
        {
            get { return this.mColor; }
            set { this.mColor = 2; }
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = "End";
                base.name = "End";
            }
        }

        public override void SetID(int id)
        {
            this.mID = ActionBase.EndId;
        }

        public override void SetFrom(object @from)
        {
            throw new System.NotImplementedException();
        }

        public override void SetTo(object to)
        {
            throw new System.NotImplementedException();
        }

        public override object GetFrom()
        {
            return null;
        }

        public override object GetTo()
        {
            return null;
        }

        public override void Do()
        {
            base.Do();
            Debug.Log("Action Queue finish");
        }

        protected override void SkipToDone()
        {
            
        }
    }
}
namespace ActionQueue
{
    [System.Serializable]
    public class StartAction : ActionBase
    {
        public override int color {
            get { return this.mColor; }
            set { this.mColor = 1; }
        }

        public override string Name
        {
            get { return this.name; }
            set
            {
                this.name = "Start";
                base.name = "Start";
            }
        }

        public override void SetID(int id)
        {
            this.mID = ActionBase.StartID;
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
            this.OnFinish();
        }

        protected override void SkipToDone()
        {
            
        }
    }
}
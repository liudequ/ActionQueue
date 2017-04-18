using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActionQueue
{

    public class ActionExecutor : MonoBehaviour
    {

        [SerializeField]
        public List<ActionBase> Actions = new List<ActionBase>();

        // Use this for initialization
        void Awake()
        {

            this.StartCoroutine(waitPlayAction());


            this.AddFinishAction();
        }

        private IEnumerator waitPlayAction()
        {
            yield return new WaitForSeconds(2);
            this.PlayActions();
            yield return new WaitForSeconds(0.5f);
            //            this.PlaySkip();
        }


        void Update()
        {
            if (this.Actions != null && this.Actions.Count > 0)
            {
                for (int i = 0; i < this.Actions.Count; i++)
                {
                    var action = this.Actions[i];
                    action.OnTick();
                }
            }
        }

        public void PlayActions()
        {
            this.ResetAll();
            this.GetActionByID(ActionBase.StartID).Do();
        }


        public void ResetAll()
        {
            for (int i = this.Actions.Count - 1; i >= 0; i--)
            {
                var action = this.Actions[i];
                action.Reset();
            }
        }

        public void PlaySkip()
        {
            for (int i = this.Actions.Count - 1; i >= 0; i--)
            {
                var action = this.Actions[i];
                action.IsSkip = true;
            }
        }

        void OnDisable()
        {
            if (Application.isPlaying)
            {
                for (int i = 0; i < this.Actions.Count; i++)
                {
                    var action = this.Actions[i];
                    action.Reset();
                }
            }
        }

        public void SetDatas(List<ActionData> datas)
        {
            for (int index = 0; index < Actions.Count; index++)
            {
                var action = Actions[index];

                for (int i = 0; i < datas.Count; i++)
                {
                    var data = datas[i];
                    if (action.ID == data.ActionID)
                    {
                        action.Data = data;
                        break;
                    }
                }
            }
        }


        private void AddFinishAction()
        {
            for (int i = 0; i < this.Actions.Count; i++)
            {
                var action = this.Actions[i];
                action.AddFinishAction(this.OnActionFinish);
            }
        }


        private void OnActionFinish(ActionBase action)
        {
            if (action.NextIDs != null && action.NextIDs.Count > 0)
            {
                for (int index = 0; index < action.NextIDs.Count; index++)
                {
                    var nextID = action.NextIDs[index];
                    var next = this.GetActionByID(nextID);
                    if (next != null)
                        next.PrevFinish();
                }
            }
            if (action.ID == ActionBase.EndId)  //当前列队完成
            {

            }
        }

        #region Get

        public ActionBase GetActionByID(int id)
        {
            for (int index = 0; index < this.Actions.Count; index++)
            {
                var action = this.Actions[index];
                if (action.ID == id)
                    return action;
            }
            return null;
        }

        [SerializeField, HideInInspector]
        private int CurCreateID = 0;

        public int GetNextUniqueID()
        {
            this.CurCreateID++;
            return this.CurCreateID;
        }


        #endregion




    }
}



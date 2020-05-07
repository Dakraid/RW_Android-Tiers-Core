using System;
using UnityEngine;
using Verse;

namespace MOARANDROIDS
{
    public class Dialog_Msg : Dialog_MessageBox
    {
        private const float TitleHeight = 42f;
        private const float DialogWidth = 500f;
        private const float DialogHeight = 300f;

        public Dialog_Msg(string title, string text, Action confirmedAct = null, bool destructive = false)
            : base(text, "Confirm".Translate(), confirmedAct, "GoBack".Translate(), null, title, destructive)
        {
            closeOnCancel = false;
            closeOnAccept = false;
        }

        public override Vector2 InitialSize
        {
            get
            {
                var height = DialogHeight;
                if (title != null) height += TitleHeight;
                return new Vector2(DialogWidth, height);
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            base.DoWindowContents(inRect);
            if (Event.current.type != EventType.KeyDown) return;
            
            switch (Event.current.keyCode)
            {
                case KeyCode.Return:
                case KeyCode.KeypadEnter:
                    buttonAAction?.Invoke();
                    Close();
                    break;
                case KeyCode.Escape:
                    buttonBAction?.Invoke();
                    Close();
                    break;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuestParser
{
    public partial class DialogStepType : Form
    {
        public StepType Type;

        public DialogStepType(string stepDescription)
        {
            InitializeComponent();

            lblStepDescription.Text = stepDescription;
            Type = StepType.Generic;
        }

        private void SetStepType(object sender, EventArgs e)
        {
            if (sender == btnChat)
                Type = StepType.Chat;
            else if (sender == btnCraft)
                Type = StepType.Craft;
            else if (sender == btnGeneric)
                Type = StepType.Generic;
            else if (sender == btnHarvest)
                Type = StepType.Harvest;
            else if (sender == btnKill)
                Type = StepType.Kill;
            else if (sender == btnLocation)
                Type = StepType.Location;
            else if (sender == btnObtain)
                Type = StepType.ObtainItem;
            else if (sender == btnSpell)
                Type = StepType.Spell;
        }
    }
}

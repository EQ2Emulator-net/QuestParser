namespace QuestParser
{
    partial class DialogStepType
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGeneric = new System.Windows.Forms.Button();
            this.btnChat = new System.Windows.Forms.Button();
            this.btnKill = new System.Windows.Forms.Button();
            this.btnObtain = new System.Windows.Forms.Button();
            this.btnLocation = new System.Windows.Forms.Button();
            this.btnSpell = new System.Windows.Forms.Button();
            this.btnCraft = new System.Windows.Forms.Button();
            this.btnHarvest = new System.Windows.Forms.Button();
            this.lblStepDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGeneric
            // 
            this.btnGeneric.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnGeneric.Location = new System.Drawing.Point(177, 93);
            this.btnGeneric.Name = "btnGeneric";
            this.btnGeneric.Size = new System.Drawing.Size(75, 23);
            this.btnGeneric.TabIndex = 2;
            this.btnGeneric.Text = "Generic";
            this.btnGeneric.UseVisualStyleBackColor = true;
            this.btnGeneric.Click += new System.EventHandler(this.SetStepType);
            // 
            // btnChat
            // 
            this.btnChat.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnChat.Location = new System.Drawing.Point(15, 93);
            this.btnChat.Name = "btnChat";
            this.btnChat.Size = new System.Drawing.Size(75, 23);
            this.btnChat.TabIndex = 0;
            this.btnChat.Text = "Chat";
            this.btnChat.UseVisualStyleBackColor = true;
            this.btnChat.Click += new System.EventHandler(this.SetStepType);
            // 
            // btnKill
            // 
            this.btnKill.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnKill.Location = new System.Drawing.Point(15, 122);
            this.btnKill.Name = "btnKill";
            this.btnKill.Size = new System.Drawing.Size(75, 23);
            this.btnKill.TabIndex = 4;
            this.btnKill.Text = "Kill";
            this.btnKill.UseVisualStyleBackColor = true;
            this.btnKill.Click += new System.EventHandler(this.SetStepType);
            // 
            // btnObtain
            // 
            this.btnObtain.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnObtain.Location = new System.Drawing.Point(177, 122);
            this.btnObtain.Name = "btnObtain";
            this.btnObtain.Size = new System.Drawing.Size(75, 23);
            this.btnObtain.TabIndex = 6;
            this.btnObtain.Text = "Obtain Item";
            this.btnObtain.UseVisualStyleBackColor = true;
            this.btnObtain.Click += new System.EventHandler(this.SetStepType);
            // 
            // btnLocation
            // 
            this.btnLocation.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnLocation.Location = new System.Drawing.Point(96, 122);
            this.btnLocation.Name = "btnLocation";
            this.btnLocation.Size = new System.Drawing.Size(75, 23);
            this.btnLocation.TabIndex = 5;
            this.btnLocation.Text = "Location";
            this.btnLocation.UseVisualStyleBackColor = true;
            this.btnLocation.Click += new System.EventHandler(this.SetStepType);
            // 
            // btnSpell
            // 
            this.btnSpell.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSpell.Location = new System.Drawing.Point(258, 122);
            this.btnSpell.Name = "btnSpell";
            this.btnSpell.Size = new System.Drawing.Size(75, 23);
            this.btnSpell.TabIndex = 7;
            this.btnSpell.Text = "Spell";
            this.btnSpell.UseVisualStyleBackColor = true;
            this.btnSpell.Click += new System.EventHandler(this.SetStepType);
            // 
            // btnCraft
            // 
            this.btnCraft.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCraft.Location = new System.Drawing.Point(96, 93);
            this.btnCraft.Name = "btnCraft";
            this.btnCraft.Size = new System.Drawing.Size(75, 23);
            this.btnCraft.TabIndex = 1;
            this.btnCraft.Text = "Craft";
            this.btnCraft.UseVisualStyleBackColor = true;
            this.btnCraft.Click += new System.EventHandler(this.SetStepType);
            // 
            // btnHarvest
            // 
            this.btnHarvest.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnHarvest.Location = new System.Drawing.Point(258, 93);
            this.btnHarvest.Name = "btnHarvest";
            this.btnHarvest.Size = new System.Drawing.Size(75, 23);
            this.btnHarvest.TabIndex = 3;
            this.btnHarvest.Text = "Harvest";
            this.btnHarvest.UseVisualStyleBackColor = true;
            this.btnHarvest.Click += new System.EventHandler(this.SetStepType);
            // 
            // lblStepDescription
            // 
            this.lblStepDescription.AutoSize = true;
            this.lblStepDescription.Location = new System.Drawing.Point(12, 19);
            this.lblStepDescription.MaximumSize = new System.Drawing.Size(333, 0);
            this.lblStepDescription.Name = "lblStepDescription";
            this.lblStepDescription.Size = new System.Drawing.Size(35, 13);
            this.lblStepDescription.TabIndex = 8;
            this.lblStepDescription.Text = "label1";
            // 
            // DialogStepType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 157);
            this.Controls.Add(this.lblStepDescription);
            this.Controls.Add(this.btnHarvest);
            this.Controls.Add(this.btnCraft);
            this.Controls.Add(this.btnSpell);
            this.Controls.Add(this.btnLocation);
            this.Controls.Add(this.btnObtain);
            this.Controls.Add(this.btnKill);
            this.Controls.Add(this.btnChat);
            this.Controls.Add(this.btnGeneric);
            this.Name = "DialogStepType";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "What type of step is this?";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGeneric;
        private System.Windows.Forms.Button btnChat;
        private System.Windows.Forms.Button btnKill;
        private System.Windows.Forms.Button btnObtain;
        private System.Windows.Forms.Button btnLocation;
        private System.Windows.Forms.Button btnSpell;
        private System.Windows.Forms.Button btnCraft;
        private System.Windows.Forms.Button btnHarvest;
        private System.Windows.Forms.Label lblStepDescription;
    }
}
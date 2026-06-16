namespace Win_UFSM
{
    partial class FormApp
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ListView lstUsers;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ComboBox cmbUnit;
        private System.Windows.Forms.Label lblTotalSize;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lstUsers = new ListView();
            btnRefresh = new Button();
            btnDelete = new Button();
            progressBar = new ProgressBar();
            cmbUnit = new ComboBox();
            lblTotalSize = new Label();
            SuspendLayout();
            // 
            // lstUsers
            // 
            lstUsers.CheckBoxes = true;
            lstUsers.Dock = DockStyle.Top;
            lstUsers.FullRowSelect = true;
            lstUsers.GridLines = true;
            lstUsers.Location = new Point(0, 0);
            lstUsers.Name = "lstUsers";
            lstUsers.Size = new Size(600, 320);
            lstUsers.TabIndex = 0;
            lstUsers.UseCompatibleStateImageBehavior = false;
            lstUsers.View = View.Details;
            lstUsers.Columns.Add("Dossier Utilisateur", 300);
            lstUsers.Columns.Add("Taille", 150);
            // 
            // cmbUnit
            // 
            cmbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUnit.Items.AddRange(new object[] { "Auto", "To", "Go", "Mo", "Ko", "Octets" });
            cmbUnit.Location = new Point(10, 390);
            cmbUnit.Name = "cmbUnit";
            cmbUnit.Size = new Size(100, 23);
            cmbUnit.TabIndex = 4;
            cmbUnit.SelectedIndex = 0;
            cmbUnit.SelectedIndexChanged += CmbUnit_SelectedIndexChanged;
            // 
            // lblTotalSize
            // 
            lblTotalSize.AutoSize = true;
            lblTotalSize.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTotalSize.Location = new Point(120, 393);
            lblTotalSize.Name = "lblTotalSize";
            lblTotalSize.Size = new Size(150, 15);
            lblTotalSize.TabIndex = 5;
            lblTotalSize.Text = "Poids total : Calcul en cours...";
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(10, 340);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 40);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "Actualiser";
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(120, 340);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(150, 40);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "Supprimer la sélection";
            btnDelete.Click += BtnDelete_Click;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(317, 350);
            progressBar.MarqueeAnimationSpeed = 30;
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(226, 20);
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.TabIndex = 3;
            progressBar.Visible = false;
            // 
            // FormApp
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 450);
            Controls.Add(lstUsers);
            Controls.Add(btnRefresh);
            Controls.Add(btnDelete);
            Controls.Add(progressBar);
            Controls.Add(cmbUnit);
            Controls.Add(lblTotalSize);
            Name = "FormApp";
            Text = "Gestionnaire d'espace Utilisateurs";
            Load += FormApp_Load;
            ResumeLayout(false);
        }

        #endregion
    }
}

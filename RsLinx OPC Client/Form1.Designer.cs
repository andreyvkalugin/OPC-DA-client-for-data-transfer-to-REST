namespace RsLinx_OPC_Client
{
    partial class Form1
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
            this.m_treeOPCServerBrowse = new System.Windows.Forms.TreeView();
            this.m_listOPCServers = new System.Windows.Forms.ListView();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.ValueButton = new System.Windows.Forms.Button();
            this.m_valueView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.historyList = new System.Windows.Forms.ListView();
            this.eventHistory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timeStamp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // m_treeOPCServerBrowse
            // 
            this.m_treeOPCServerBrowse.FullRowSelect = true;
            this.m_treeOPCServerBrowse.Location = new System.Drawing.Point(201, 12);
            this.m_treeOPCServerBrowse.Name = "m_treeOPCServerBrowse";
            this.m_treeOPCServerBrowse.Size = new System.Drawing.Size(289, 367);
            this.m_treeOPCServerBrowse.TabIndex = 0;
            // 
            // m_listOPCServers
            // 
            this.m_listOPCServers.Location = new System.Drawing.Point(12, 12);
            this.m_listOPCServers.Name = "m_listOPCServers";
            this.m_listOPCServers.Size = new System.Drawing.Size(183, 367);
            this.m_listOPCServers.TabIndex = 1;
            this.m_listOPCServers.UseCompatibleStateImageBehavior = false;
            this.m_listOPCServers.View = System.Windows.Forms.View.List;
            this.m_listOPCServers.SelectedIndexChanged += new System.EventHandler(this.OPCListClick);
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(282, 385);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(189, 23);
            this.OkButton.TabIndex = 2;
            this.OkButton.Text = "Добавить тег в опрос";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(282, 414);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(189, 23);
            this.CancelButton.TabIndex = 3;
            this.CancelButton.Text = "Закрытие приложения";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ValueButton
            // 
            this.ValueButton.Location = new System.Drawing.Point(282, 549);
            this.ValueButton.Name = "ValueButton";
            this.ValueButton.Size = new System.Drawing.Size(189, 23);
            this.ValueButton.TabIndex = 4;
            this.ValueButton.Text = "Значение с телефона";
            this.ValueButton.UseVisualStyleBackColor = true;
            this.ValueButton.Click += new System.EventHandler(this.ValueButton_Click);
            // 
            // m_valueView
            // 
            this.m_valueView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.m_valueView.Location = new System.Drawing.Point(12, 385);
            this.m_valueView.Name = "m_valueView";
            this.m_valueView.Size = new System.Drawing.Size(246, 187);
            this.m_valueView.TabIndex = 5;
            this.m_valueView.UseCompatibleStateImageBehavior = false;
            this.m_valueView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Элемент";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Значение";
            this.columnHeader2.Width = 100;
            // 
            // historyList
            // 
            this.historyList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.eventHistory,
            this.timeStamp});
            this.historyList.GridLines = true;
            this.historyList.Location = new System.Drawing.Point(496, 12);
            this.historyList.Name = "historyList";
            this.historyList.Size = new System.Drawing.Size(397, 560);
            this.historyList.TabIndex = 6;
            this.historyList.UseCompatibleStateImageBehavior = false;
            this.historyList.View = System.Windows.Forms.View.Details;
            // 
            // eventHistory
            // 
            this.eventHistory.Text = "команда";
            this.eventHistory.Width = 293;
            // 
            // timeStamp
            // 
            this.timeStamp.Text = "время";
            this.timeStamp.Width = 100;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 584);
            this.ControlBox = false;
            this.Controls.Add(this.historyList);
            this.Controls.Add(this.m_valueView);
            this.Controls.Add(this.ValueButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.m_listOPCServers);
            this.Controls.Add(this.m_treeOPCServerBrowse);
            this.Name = "Form1";
            this.Text = "SCADA клиент ООО Транснефть-Порт Приморск";
            this.Load += new System.EventHandler(this.OnLoadForm);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView m_treeOPCServerBrowse;
        private System.Windows.Forms.ListView m_listOPCServers;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button ValueButton;
        private System.Windows.Forms.ListView m_valueView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        public System.Windows.Forms.ListView historyList;
        public System.Windows.Forms.ColumnHeader eventHistory;
        public System.Windows.Forms.ColumnHeader timeStamp;
    }

}







/*namespace RsLinx_OPC_Client
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Liberare le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtN7Value = new System.Windows.Forms.TextBox();
            this.btnWriteN7 = new System.Windows.Forms.Button();
            this.lblN7Read = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(21, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(122, 61);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtN7Value
            // 
            this.txtN7Value.Location = new System.Drawing.Point(21, 99);
            this.txtN7Value.Name = "txtN7Value";
            this.txtN7Value.Size = new System.Drawing.Size(100, 20);
            this.txtN7Value.TabIndex = 1;
            // 
            // btnWriteN7
            // 
            this.btnWriteN7.Location = new System.Drawing.Point(153, 97);
            this.btnWriteN7.Name = "btnWriteN7";
            this.btnWriteN7.Size = new System.Drawing.Size(100, 23);
            this.btnWriteN7.TabIndex = 2;
            this.btnWriteN7.Text = "Write N7";
            this.btnWriteN7.UseVisualStyleBackColor = true;
            this.btnWriteN7.Click += new System.EventHandler(this.btnWriteN7_Click);
            // 
            // lblN7Read
            // 
            this.lblN7Read.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblN7Read.Location = new System.Drawing.Point(313, 94);
            this.lblN7Read.Name = "lblN7Read";
            this.lblN7Read.Size = new System.Drawing.Size(105, 29);
            this.lblN7Read.TabIndex = 3;
            this.lblN7Read.Text = "N7:0 Value";
            this.lblN7Read.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Location = new System.Drawing.Point(313, 142);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 29);
            this.label1.TabIndex = 4;
            this.label1.Text = "N7:1 Value";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.Location = new System.Drawing.Point(313, 184);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 29);
            this.label2.TabIndex = 5;
            this.label2.Text = "N11:0 Value";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label3.Location = new System.Drawing.Point(313, 226);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 29);
            this.label3.TabIndex = 5;
            this.label3.Text = "myValue";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(21, 184);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(122, 61);
            this.button1.TabIndex = 6;
            this.button1.Text = "valueFromCellPhone";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 272);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblN7Read);
            this.Controls.Add(this.btnWriteN7);
            this.Controls.Add(this.txtN7Value);
            this.Controls.Add(this.btnConnect);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtN7Value;
        private System.Windows.Forms.Button btnWriteN7;
        private System.Windows.Forms.Label lblN7Read;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
    }
}

*/

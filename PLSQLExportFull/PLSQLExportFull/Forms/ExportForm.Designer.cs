namespace PLSQLExportFull.Forms
{
    partial class ExportForm
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
            this.chkTruncate = new System.Windows.Forms.CheckBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabConnection = new System.Windows.Forms.TabPage();
            this.btnLoadConfig = new System.Windows.Forms.Button();
            this.btnPasteString = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtServiceName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.lblConnectionString = new System.Windows.Forms.Label();
            this.tabExport = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSearchTable = new System.Windows.Forms.TextBox();
            this.gridTables = new System.Windows.Forms.DataGridView();
            this.cmbTableGroups = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnDeselectAllExport = new System.Windows.Forms.Button();
            this.btnSelectAllExport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.txtWhereClause = new System.Windows.Forms.TextBox();
            this.lblWhereClause = new System.Windows.Forms.Label();
            this.chkZipOutput = new System.Windows.Forms.CheckBox();
            this.lblExportTables = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl.SuspendLayout();
            this.tabConnection.SuspendLayout();
            this.tabExport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTables)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkTruncate
            // 
            this.chkTruncate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkTruncate.AutoSize = true;
            this.chkTruncate.Location = new System.Drawing.Point(299, 398);
            this.chkTruncate.Name = "chkTruncate";
            this.chkTruncate.Size = new System.Drawing.Size(140, 17);
            this.chkTruncate.TabIndex = 12;
            this.chkTruncate.Text = "Truncar tabelas (Limpar)";
            this.chkTruncate.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabConnection);
            this.tabControl.Controls.Add(this.tabExport);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(600, 541);
            this.tabControl.TabIndex = 0;
            // 
            // tabConnection
            // 
            this.tabConnection.Controls.Add(this.btnLoadConfig);
            this.tabConnection.Controls.Add(this.btnPasteString);
            this.tabConnection.Controls.Add(this.btnDisconnect);
            this.tabConnection.Controls.Add(this.btnConnect);
            this.tabConnection.Controls.Add(this.lblConnectionStatus);
            this.tabConnection.Controls.Add(this.txtPassword);
            this.tabConnection.Controls.Add(this.label5);
            this.tabConnection.Controls.Add(this.txtUserId);
            this.tabConnection.Controls.Add(this.label4);
            this.tabConnection.Controls.Add(this.txtServiceName);
            this.tabConnection.Controls.Add(this.label3);
            this.tabConnection.Controls.Add(this.txtPort);
            this.tabConnection.Controls.Add(this.label2);
            this.tabConnection.Controls.Add(this.txtHost);
            this.tabConnection.Controls.Add(this.label1);
            this.tabConnection.Controls.Add(this.txtConnectionString);
            this.tabConnection.Controls.Add(this.lblConnectionString);
            this.tabConnection.Location = new System.Drawing.Point(4, 22);
            this.tabConnection.Margin = new System.Windows.Forms.Padding(2);
            this.tabConnection.Name = "tabConnection";
            this.tabConnection.Padding = new System.Windows.Forms.Padding(2);
            this.tabConnection.Size = new System.Drawing.Size(592, 515);
            this.tabConnection.TabIndex = 0;
            this.tabConnection.Text = "Conexão";
            this.tabConnection.UseVisualStyleBackColor = true;
            // 
            // btnLoadConfig
            // 
            this.btnLoadConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadConfig.Location = new System.Drawing.Point(471, 59);
            this.btnLoadConfig.Name = "btnLoadConfig";
            this.btnLoadConfig.Size = new System.Drawing.Size(109, 23);
            this.btnLoadConfig.TabIndex = 17;
            this.btnLoadConfig.Text = "Carregar .config";
            this.btnLoadConfig.UseVisualStyleBackColor = true;
            this.btnLoadConfig.Click += new System.EventHandler(this.btnLoadConfig_Click);
            // 
            // btnPasteString
            // 
            this.btnPasteString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPasteString.Location = new System.Drawing.Point(471, 30);
            this.btnPasteString.Name = "btnPasteString";
            this.btnPasteString.Size = new System.Drawing.Size(109, 23);
            this.btnPasteString.TabIndex = 16;
            this.btnPasteString.Text = "Colar String";
            this.btnPasteString.UseVisualStyleBackColor = true;
            this.btnPasteString.Click += new System.EventHandler(this.btnPasteString_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(119, 256);
            this.btnDisconnect.Margin = new System.Windows.Forms.Padding(2);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(96, 24);
            this.btnDisconnect.TabIndex = 15;
            this.btnDisconnect.Text = "Desconectar";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(19, 256);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(2);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(96, 24);
            this.btnConnect.TabIndex = 14;
            this.btnConnect.Text = "Conectar";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.AutoSize = true;
            this.lblConnectionStatus.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectionStatus.ForeColor = System.Drawing.Color.Red;
            this.lblConnectionStatus.Location = new System.Drawing.Point(15, 297);
            this.lblConnectionStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(172, 19);
            this.lblConnectionStatus.TabIndex = 12;
            this.lblConnectionStatus.Text = "Status: Desconectado";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(119, 211);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(2);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(174, 20);
            this.txtPassword.TabIndex = 11;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtConnectionField_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 214);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Senha:";
            // 
            // txtUserId
            // 
            this.txtUserId.Location = new System.Drawing.Point(119, 179);
            this.txtUserId.Margin = new System.Windows.Forms.Padding(2);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(174, 20);
            this.txtUserId.TabIndex = 9;
            this.txtUserId.TextChanged += new System.EventHandler(this.txtConnectionField_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 181);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Usuário:";
            // 
            // txtServiceName
            // 
            this.txtServiceName.Location = new System.Drawing.Point(119, 146);
            this.txtServiceName.Margin = new System.Windows.Forms.Padding(2);
            this.txtServiceName.Name = "txtServiceName";
            this.txtServiceName.Size = new System.Drawing.Size(174, 20);
            this.txtServiceName.TabIndex = 7;
            this.txtServiceName.TextChanged += new System.EventHandler(this.txtConnectionField_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 149);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Service Name/SID:";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(119, 114);
            this.txtPort.Margin = new System.Windows.Forms.Padding(2);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(174, 20);
            this.txtPort.TabIndex = 5;
            this.txtPort.Text = "1521";
            this.txtPort.TextChanged += new System.EventHandler(this.txtConnectionField_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 116);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Porta:";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(119, 81);
            this.txtHost.Margin = new System.Windows.Forms.Padding(2);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(174, 20);
            this.txtHost.TabIndex = 3;
            this.txtHost.TextChanged += new System.EventHandler(this.txtConnectionField_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 84);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Host:";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionString.Location = new System.Drawing.Point(15, 32);
            this.txtConnectionString.Margin = new System.Windows.Forms.Padding(2);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.ReadOnly = true;
            this.txtConnectionString.Size = new System.Drawing.Size(451, 20);
            this.txtConnectionString.TabIndex = 1;
            // 
            // lblConnectionString
            // 
            this.lblConnectionString.AutoSize = true;
            this.lblConnectionString.Location = new System.Drawing.Point(15, 14);
            this.lblConnectionString.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblConnectionString.Name = "lblConnectionString";
            this.lblConnectionString.Size = new System.Drawing.Size(97, 13);
            this.lblConnectionString.TabIndex = 0;
            this.lblConnectionString.Text = "String de Conexão:";
            // 
            // tabExport
            // 
            this.tabExport.Controls.Add(this.label7);
            this.tabExport.Controls.Add(this.txtSearchTable);
            this.tabExport.Controls.Add(this.gridTables);
            this.tabExport.Controls.Add(this.chkTruncate);
            this.tabExport.Controls.Add(this.cmbTableGroups);
            this.tabExport.Controls.Add(this.label6);
            this.tabExport.Controls.Add(this.btnDeselectAllExport);
            this.tabExport.Controls.Add(this.btnSelectAllExport);
            this.tabExport.Controls.Add(this.btnExport);
            this.tabExport.Controls.Add(this.txtWhereClause);
            this.tabExport.Controls.Add(this.lblWhereClause);
            this.tabExport.Controls.Add(this.chkZipOutput);
            this.tabExport.Controls.Add(this.lblExportTables);
            this.tabExport.Location = new System.Drawing.Point(4, 22);
            this.tabExport.Margin = new System.Windows.Forms.Padding(2);
            this.tabExport.Name = "tabExport";
            this.tabExport.Padding = new System.Windows.Forms.Padding(2);
            this.tabExport.Size = new System.Drawing.Size(592, 515);
            this.tabExport.TabIndex = 4;
            this.tabExport.Text = "Exportação DML";
            this.tabExport.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(306, 49);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Nome:";
            // 
            // txtSearchTable
            // 
            this.txtSearchTable.Location = new System.Drawing.Point(349, 46);
            this.txtSearchTable.Name = "txtSearchTable";
            this.txtSearchTable.Size = new System.Drawing.Size(226, 20);
            this.txtSearchTable.TabIndex = 14;
            // 
            // gridTables
            // 
            this.gridTables.AllowUserToAddRows = false;
            this.gridTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridTables.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTables.Location = new System.Drawing.Point(12, 72);
            this.gridTables.Name = "gridTables";
            this.gridTables.RowHeadersVisible = false;
            this.gridTables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTables.Size = new System.Drawing.Size(563, 316);
            this.gridTables.TabIndex = 13;
            // 
            // cmbTableGroups
            // 
            this.cmbTableGroups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTableGroups.FormattingEnabled = true;
            this.cmbTableGroups.Location = new System.Drawing.Point(113, 45);
            this.cmbTableGroups.Margin = new System.Windows.Forms.Padding(2);
            this.cmbTableGroups.Name = "cmbTableGroups";
            this.cmbTableGroups.Size = new System.Drawing.Size(189, 21);
            this.cmbTableGroups.TabIndex = 9;
            this.cmbTableGroups.SelectedIndexChanged += new System.EventHandler(this.cmbTableGroups_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 48);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Grupo de Tabelas:";
            // 
            // btnDeselectAllExport
            // 
            this.btnDeselectAllExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeselectAllExport.Location = new System.Drawing.Point(102, 393);
            this.btnDeselectAllExport.Margin = new System.Windows.Forms.Padding(2);
            this.btnDeselectAllExport.Name = "btnDeselectAllExport";
            this.btnDeselectAllExport.Size = new System.Drawing.Size(82, 24);
            this.btnDeselectAllExport.TabIndex = 7;
            this.btnDeselectAllExport.Text = "Desmarcar Todos";
            this.btnDeselectAllExport.UseVisualStyleBackColor = true;
            this.btnDeselectAllExport.Click += new System.EventHandler(this.btnDeselectAllExport_Click);
            // 
            // btnSelectAllExport
            // 
            this.btnSelectAllExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectAllExport.Location = new System.Drawing.Point(12, 393);
            this.btnSelectAllExport.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectAllExport.Name = "btnSelectAllExport";
            this.btnSelectAllExport.Size = new System.Drawing.Size(82, 24);
            this.btnSelectAllExport.TabIndex = 6;
            this.btnSelectAllExport.Text = "Marcar Todos";
            this.btnSelectAllExport.UseVisualStyleBackColor = true;
            this.btnSelectAllExport.Click += new System.EventHandler(this.btnSelectAllExport_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(444, 393);
            this.btnExport.Margin = new System.Windows.Forms.Padding(2);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(131, 24);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "Exportar DML";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // txtWhereClause
            // 
            this.txtWhereClause.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWhereClause.Location = new System.Drawing.Point(15, 447);
            this.txtWhereClause.Margin = new System.Windows.Forms.Padding(2);
            this.txtWhereClause.Multiline = true;
            this.txtWhereClause.Name = "txtWhereClause";
            this.txtWhereClause.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtWhereClause.Size = new System.Drawing.Size(564, 58);
            this.txtWhereClause.TabIndex = 4;
            // 
            // lblWhereClause
            // 
            this.lblWhereClause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblWhereClause.AutoSize = true;
            this.lblWhereClause.Location = new System.Drawing.Point(15, 428);
            this.lblWhereClause.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWhereClause.Name = "lblWhereClause";
            this.lblWhereClause.Size = new System.Drawing.Size(167, 13);
            this.lblWhereClause.TabIndex = 3;
            this.lblWhereClause.Text = "Cláusula WHERE (ex: ID >= 200):";
            // 
            // chkZipOutput
            // 
            this.chkZipOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkZipOutput.AutoSize = true;
            this.chkZipOutput.Checked = true;
            this.chkZipOutput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkZipOutput.Location = new System.Drawing.Point(193, 398);
            this.chkZipOutput.Name = "chkZipOutput";
            this.chkZipOutput.Size = new System.Drawing.Size(100, 17);
            this.chkZipOutput.TabIndex = 7;
            this.chkZipOutput.Text = "Compactar (.7z)";
            this.chkZipOutput.UseVisualStyleBackColor = true;
            // 
            // lblExportTables
            // 
            this.lblExportTables.AutoSize = true;
            this.lblExportTables.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExportTables.Location = new System.Drawing.Point(15, 16);
            this.lblExportTables.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblExportTables.Name = "lblExportTables";
            this.lblExportTables.Size = new System.Drawing.Size(161, 19);
            this.lblExportTables.TabIndex = 0;
            this.lblExportTables.Text = "Tabelas para Exportar:";
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 541);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip.Size = new System.Drawing.Size(600, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(43, 17);
            this.toolStripStatusLabel.Text = "Pronto";
            // 
            // ExportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 563);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusStrip);
            this.Icon = global::PLSQLExportFull.Properties.Resources.Logo;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ExportForm";
            this.Text = "SQL Export Full";
            this.tabControl.ResumeLayout(false);
            this.tabConnection.ResumeLayout(false);
            this.tabConnection.PerformLayout();
            this.tabExport.ResumeLayout(false);
            this.tabExport.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTables)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabConnection;
        private System.Windows.Forms.TabPage tabExport;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;

        // Botões
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnDeselectAllExport;
        private System.Windows.Forms.Button btnSelectAllExport;
        private System.Windows.Forms.Button btnPasteString;
        private System.Windows.Forms.Button btnLoadConfig;

        // Campos de Texto
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtServiceName;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.TextBox txtWhereClause;
        public System.Windows.Forms.TextBox txtUserId;

        // Labels
        private System.Windows.Forms.Label lblConnectionString;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.Label lblExportTables;
        private System.Windows.Forms.Label lblWhereClause;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbTableGroups;
        private System.Windows.Forms.CheckBox chkTruncate;
        private System.Windows.Forms.CheckBox chkZipOutput;
        private System.Windows.Forms.DataGridView gridTables;
        private System.Windows.Forms.TextBox txtSearchTable;
        private System.Windows.Forms.Label label7;
    }
}
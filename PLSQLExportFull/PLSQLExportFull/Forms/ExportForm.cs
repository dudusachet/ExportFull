using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PLSQLExportFull.Data;
using PLSQLExportFull.Business;
using PLSQLExportFull.Models;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Xml;

namespace PLSQLExportFull.Forms
{
    public partial class ExportForm : Form
    {
        // Variáveis de Dependência
        private readonly OracleConnectionManager _connectionManager;
        private readonly OracleQueryExecutor _queryExecutor;
        private readonly MetadataRepository _metadataRepository;
        private readonly ExportManager _exportManager;
        private ToolStripStatusLabel lblDbNameFooter;
        private bool _isBlueTheme;

        // Variáveis de Estado
        private List<TableInfo> _allTables;
        private List<TableGroup> _tableGroups;
        private SortOrder _sortOrder = SortOrder.Ascending;
        private string _sortBy = "Name";
        private bool _isParsingConnectionString = false;
        private bool _isUpdatingWhereFromGrid = false; // Trava para evitar loop

        public ExportForm()
        {
            InitializeComponent();


            _isBlueTheme = Properties.Settings.Default.IsBlueTheme;

            // --- 1. CONFIGURAÇÕES VISUAIS INICIAIS ---
            SetupGridTables();
            AplicarEstiloModerno(); // Aplica o tema cinza/flat
            ConfigurarRodapeBanco();


            this.KeyPreview = true; // Permite que o Form capture teclas antes dos botões
            this.KeyDown += ExportForm_KeyDown;
            this.MinimumSize = new System.Drawing.Size(620, 530);


            // --- 2. INICIALIZAÇÃO DAS DEPENDÊNCIAS ---
            _connectionManager = new OracleConnectionManager();
            _queryExecutor = new OracleQueryExecutor(_connectionManager);
            _metadataRepository = new MetadataRepository(_queryExecutor);
            _exportManager = new ExportManager(_queryExecutor, _metadataRepository);

            // --- 3. REGISTRO DE EVENTOS ---
            this.StartPosition = FormStartPosition.CenterScreen;
            this.tabControl.Selecting += new TabControlCancelEventHandler(this.tabControl_Selecting);
            this.FormClosing += new FormClosingEventHandler(this.ExportForm_FormClosing);

            // Eventos do Grid e Pesquisa
            this.gridTables.SelectionChanged += new EventHandler(gridTables_SelectionChanged);
            this.gridTables.CellPainting += new DataGridViewCellPaintingEventHandler(gridTables_CellPainting);
            //this.gridTables.CellContentClick += new DataGridViewCellContentClickEventHandler(gridTables_CellContentClick);
            this.gridTables.CurrentCellDirtyStateChanged += new EventHandler(gridTables_CurrentCellDirtyStateChanged);
            // Eventos de Texto
            this.txtWhereClause.TextChanged += new EventHandler(txtWhereClause_TextChanged);
            this.txtSearchTable.TextChanged += new EventHandler(txtSearchTable_TextChanged);
            this.gridTables.CellPainting += new DataGridViewCellPaintingEventHandler(gridTables_CellPainting);

            // --- 4. CARREGAR CONFIGURAÇÕES ---
            if (!string.IsNullOrEmpty(Properties.Settings.Default.LastHost))
            {
                txtHost.Text = Properties.Settings.Default.LastHost;
                txtPort.Text = Properties.Settings.Default.LastPort;
                txtServiceName.Text = Properties.Settings.Default.LastService;
                txtUserId.Text = Properties.Settings.Default.LastUser;
                txtPassword.Text = Properties.Settings.Default.LastPassword;
            }
            else
            {
#if DEBUG
                txtHost.Text = "172.25.100.205";
                txtPort.Text = "1521";
                txtServiceName.Text = "XE";
                txtUserId.Text = "r22sp15";
                txtPassword.Text = "r22sp15";
#endif
            }

            UpdateConnectionStatus();
            UpdateConnectionString();
            LoadTableGroups();
        }

        // ====================================================================
        // CONFIGURAÇÃO VISUAL E GRID
        // ====================================================================

        // ====================================================================
        // CONFIGURAÇÃO DO RODAPÉ
        // ====================================================================
        private void ConfigurarRodapeBanco()
        {
            // Verifica se a variável do designer existe (geralmente statusStrip ou statusStrip1)
            // Se der erro na linha abaixo, troque 'this.statusStrip' por 'this.statusStrip1'
            // 2. CRIAR LABEL DO RODAPÉ (Corrigido para ficar à Direita)
            if (this.statusStrip != null)
            {
                // --- PASSO IMPORTANTE: Empurrar tudo para a direita ---
                // Procura o primeiro Label existente (o que mostra "Pronto" ou "Conectado")
                // e diz para ele ocupar todo o espaço sobrando.
                foreach (ToolStripItem item in this.statusStrip.Items)
                {
                    if (item is ToolStripStatusLabel labelExistente)
                    {
                        labelExistente.Spring = true;
                        labelExistente.TextAlign = ContentAlignment.MiddleLeft; // Mantém o texto dele na esquerda
                        break; // Só precisa fazer no primeiro
                    }
                }
                // ------------------------------------------------------

                lblDbNameFooter = new ToolStripStatusLabel();
                lblDbNameFooter.Text = "";
                lblDbNameFooter.ForeColor = Color.FromArgb(49, 49, 48);
                lblDbNameFooter.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                lblDbNameFooter.BorderSides = ToolStripStatusLabelBorderSides.Left;
                lblDbNameFooter.BorderStyle = Border3DStyle.Etched;
                lblDbNameFooter.Padding = new Padding(10, 0, 0, 0);

                // Garante o alinhamento
                lblDbNameFooter.Alignment = ToolStripItemAlignment.Right;

                this.statusStrip.Items.Add(lblDbNameFooter);
            }
            else
            {
                MessageBox.Show("Erro: Não foi possível encontrar o componente statusStrip no formulário.");
            }
        }
        private void AplicarEstiloModerno()
        {

            Color corVermelha = Color.FromArgb(229, 35, 41);
            Color corAzul = Color.FromArgb(13, 128, 191);

            // Decide qual cor usar baseada na variável de controle
            Color corDestaque = _isBlueTheme ? corAzul : corVermelha;
            // --- DEFINIÇÃO DA PALETA DE CORES ---
            Color corTexto = Color.FromArgb(49, 49, 48);      // Cinza Chumbo (Textos)
            Color corFundo = Color.FromArgb(245, 246, 250);   // Off-White (Fundo)
            Color corSelecao = Color.FromArgb(230, 230, 230); // <--- CINZA PARA SELEÇÃO

            // Configuração Geral do Form
            this.BackColor = corFundo;
            this.ForeColor = corTexto;
            this.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular);

            // Configuração do Grid
            gridTables.BackgroundColor = Color.White;
            gridTables.BorderStyle = BorderStyle.None;
            gridTables.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            gridTables.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            gridTables.EnableHeadersVisualStyles = false;
            gridTables.ColumnHeadersDefaultCellStyle.BackColor = corDestaque;
            gridTables.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gridTables.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            gridTables.ColumnHeadersHeight = 35;
            gridTables.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // --- AQUI ESTÁ A MUDANÇA DA COR DE SELEÇÃO ---
            gridTables.DefaultCellStyle.SelectionBackColor = corSelecao; // Cinza
            gridTables.DefaultCellStyle.SelectionForeColor = corTexto;   // Mantém o texto escuro
                                                                         // ---------------------------------------------

            gridTables.DefaultCellStyle.BackColor = Color.White;
            gridTables.DefaultCellStyle.ForeColor = corTexto;
            gridTables.DefaultCellStyle.Padding = new Padding(2);
            gridTables.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 252);

            // Aplica nos botões e caixas de texto
            EstilizarControlesRecursivo(this, corDestaque, corTexto);
        }
        private void EstilizarControlesRecursivo(Control container, Color corDestaque, Color corTexto)
        {
            foreach (Control c in container.Controls)
            {
                if (c is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.BackColor = corDestaque; // Fundo Vermelho
                    btn.ForeColor = Color.White; // Texto Branco
                    btn.Cursor = Cursors.Hand;
                    btn.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);

                    // Botões de Cancelar/Desmarcar ficam cinza claro
                    if (btn.Name.Contains("Deselect") || btn.Name.Contains("Disconnect") || btn.Name.Contains("Cancel"))
                    {
                        btn.BackColor = Color.FromArgb(189, 195, 199);
                        btn.ForeColor = Color.Black;
                    }
                }
                else if (c is TextBox txt)
                {
                    txt.BorderStyle = BorderStyle.FixedSingle;
                    txt.BackColor = Color.White;
                    txt.ForeColor = corTexto; // Texto Cinza
                }
                else if (c is CheckBox chk)
                {
                    chk.FlatStyle = FlatStyle.Flat;
                    chk.ForeColor = corTexto; // Texto Cinza
                    chk.Cursor = Cursors.Hand;
                    chk.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular);
                }
                else if (c is Label lbl)
                {
                    lbl.ForeColor = corTexto; // Texto Cinza
                }
                else if (c is GroupBox grp)
                {
                    grp.ForeColor = corTexto; // Título do grupo Cinza
                }

                if (c.HasChildren) EstilizarControlesRecursivo(c, corDestaque, corTexto);
            }
        }
        private void SetupGridTables()
        {
            gridTables.Columns.Clear();
            gridTables.AutoGenerateColumns = false;
            gridTables.AllowUserToAddRows = false;
            gridTables.RowHeadersVisible = false;
            gridTables.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // --- TRAVAR ALTURA DAS LINHAS ---
            gridTables.AllowUserToResizeRows = false;
            gridTables.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            gridTables.RowTemplate.Height = 30; // Altura confortável

            // 1. Checkbox (Ajustado para ficar mais estreito)
            var colCheck = new DataGridViewCheckBoxColumn();
            colCheck.HeaderText = ""; // Deixe vazio para ficar mais limpo se for muito estreito
            colCheck.Name = "colCheck";
            colCheck.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            colCheck.Width = 40;
            colCheck.Resizable = DataGridViewTriState.False;


            colCheck.ReadOnly = false;
            gridTables.Columns.Add(colCheck);

            // 2. Linhas
            var colRows = new DataGridViewTextBoxColumn();
            colRows.HeaderText = "Linhas";
            colRows.Name = "colRows";
            colRows.ReadOnly = true;
            colRows.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            colRows.Width = 70;
            gridTables.Columns.Add(colRows);

            // 3. Tabela
            var colName = new DataGridViewTextBoxColumn();
            colName.HeaderText = "Tabela";
            colName.Name = "colTableName";
            colName.ReadOnly = true;
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            gridTables.Columns.Add(colName);

            // 4. Where
            var colWhere = new DataGridViewTextBoxColumn();
            colWhere.HeaderText = "Condição (Where)";
            colWhere.Name = "colWhere";
            colWhere.ReadOnly = false;
            colWhere.Width = 200;
            gridTables.Columns.Add(colWhere);


            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null,
                gridTables,
                new object[] { true });
        }
        // ====================================================================
        // EVENTOS DE DESENHO E CLIQUE DO GRID (MODERNO)
        // ====================================================================


        // Este método detecta que você clicou no checkbox e salva o valor IMEDIATAMENTE.
        private void gridTables_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (gridTables.IsCurrentCellDirty)
            {
                // Força o "Commit" (Salvar) da edição na hora que o clique acontece
                gridTables.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void ExportForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Verifica se Ctrl + Alt + G foram pressionados
            if (e.Control && e.Alt && e.KeyCode == Keys.G)
            {
                // 1. Inverte o estado
                _isBlueTheme = !_isBlueTheme;

                // 2. SALVA NA HORA
                Properties.Settings.Default.IsBlueTheme = _isBlueTheme;
                Properties.Settings.Default.Save(); 

                // 3. Reaplica o estilo e redesenha
                AplicarEstiloModerno();
                gridTables.Refresh();
            }
        }

        private void gridTables_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Verifica se é a coluna do Checkbox (índice 0 ou nome "colCheck") e se não é o cabeçalho
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                e.PaintBackground(e.ClipBounds, true);

                // --- CONFIGURAÇÕES DO CHECKBOX ---
                bool isChecked = (bool)e.Value;

                // COR: Use a mesma que você definiu no Estilo Moderno
                Color corAtiva = Color.FromArgb(45, 52, 54); // Cinza Chumbo
                                                             // Color corAtiva = Color.FromArgb(39, 174, 96); // Verde (Se escolheu verde antes)

                int boxSize = 14; // Tamanho do quadradinho (14x14 fica bom)

                // Calcula a posição para centralizar
                int x = e.CellBounds.X + (e.CellBounds.Width - boxSize) / 2;
                int y = e.CellBounds.Y + (e.CellBounds.Height - boxSize) / 2;
                Rectangle boxRect = new Rectangle(x, y, boxSize, boxSize);

                using (Pen penBorder = new Pen(corAtiva, 1))
                using (SolidBrush brushFill = new SolidBrush(corAtiva))
                {
                    // Se estiver marcado (Checked)
                    if (isChecked)
                    {
                        // 1. Pinta o quadrado cheio
                        e.Graphics.FillRectangle(brushFill, boxRect);

                        // 2. Desenha o "V" (check) branco
                        // Coordenadas manuais para fazer o V perfeitamente
                        Point p1 = new Point(x + 3, y + 6);
                        Point p2 = new Point(x + 5, y + 10); // Ponta de baixo
                        Point p3 = new Point(x + 11, y + 3); // Ponta de cima

                        using (Pen penCheck = new Pen(Color.White, 2))
                        {
                            e.Graphics.DrawLine(penCheck, p1, p2);
                            e.Graphics.DrawLine(penCheck, p2, p3);
                        }
                    }
                    else
                    {
                        // Se estiver desmarcado: Apenas a borda cinza/colorida
                        e.Graphics.DrawRectangle(penBorder, boxRect);
                    }
                }

                // Diz para o Windows: "Eu já desenhei, não desenhe o padrão por cima"
                e.Handled = true;
            }
        }
        private void gridTables_SelectionChanged(object sender, EventArgs e)
        {
            if (gridTables.SelectedRows.Count > 0)
            {
                var row = gridTables.SelectedRows[0];
                string whereValue = row.Cells["colWhere"].Value?.ToString() ?? "";
                _isUpdatingWhereFromGrid = true;
                txtWhereClause.Text = whereValue;
                txtWhereClause.Enabled = true;
                _isUpdatingWhereFromGrid = false;
            }
            else
            {
                _isUpdatingWhereFromGrid = true;
                txtWhereClause.Clear();
                txtWhereClause.Enabled = false;
                _isUpdatingWhereFromGrid = false;
            }
        }

        // ====================================================================
        // EVENTOS DE TEXTO E PESQUISA
        // ====================================================================

        private void txtWhereClause_TextChanged(object sender, EventArgs e)
        {
            if (_isUpdatingWhereFromGrid) return;
            if (gridTables.SelectedRows.Count > 0)
            {
                var row = gridTables.SelectedRows[0];
                row.Cells["colWhere"].Value = txtWhereClause.Text;
            }
        }

        private void txtSearchTable_TextChanged(object sender, EventArgs e)
        {
            SortAndDisplayTables(false);
        }

        // ====================================================================
        // CONEXÃO
        // ====================================================================

        private void UpdateConnectionString()
        {
            string host = txtHost.Text;
            string port = txtPort.Text;
            string serviceName = txtServiceName.Text;
            string userId = txtUserId.Text;
            string password = txtPassword.Text;

            string connectionString = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT={port}))(CONNECT_DATA=(SERVICE_NAME={serviceName})));User Id={userId};Password={password};";

            txtConnectionString.Text = connectionString;
            _connectionManager.ConnectionString = connectionString;
        }

        private void ExportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_connectionManager != null && _connectionManager.IsConnected)
                _connectionManager.Disconnect();

            Properties.Settings.Default.LastHost = txtHost.Text.Trim();
            Properties.Settings.Default.LastPort = txtPort.Text.Trim();
            Properties.Settings.Default.LastService = txtServiceName.Text.Trim();
            Properties.Settings.Default.LastUser = txtUserId.Text.Trim();
            Properties.Settings.Default.LastPassword = txtPassword.Text;
            Properties.Settings.Default.Save();
        }

        private void UpdateConnectionStatus()
        {
            bool isConnected = _connectionManager.IsConnected;

            if (isConnected)
            {
                lblConnectionStatus.Text = "Status: Conectado";
                lblConnectionStatus.ForeColor = Color.Green;
                if (lblDbNameFooter != null)
                {
                    string db = txtServiceName.Text.Trim().ToUpper();
                    string host = txtHost.Text.Trim();
                    // AQUI É ONDE O TEXTO APARECE DE VERDADE
                    lblDbNameFooter.Text = $"{txtUserId.Text.ToUpper()} | {db}@{host}";
                    lblDbNameFooter.ForeColor = Color.FromArgb(49, 49, 48);
                }
            
        }
            else
            {
                lblConnectionStatus.Text = "Status: Desconectado";
                lblConnectionStatus.ForeColor = Color.Red;
                if (tabControl.SelectedTab == tabExport) tabControl.SelectedTab = tabConnection;
                if (lblDbNameFooter != null)
                {
                    lblDbNameFooter.Text = "Sem conexão"; // Texto padrão quando desconectado
                    lblDbNameFooter.ForeColor = Color.Gray;
                }
            }

            bool enableInputs = !isConnected;
            txtHost.Enabled = enableInputs;
            txtPort.Enabled = enableInputs;
            txtServiceName.Enabled = enableInputs;
            txtUserId.Enabled = enableInputs;
            txtPassword.Enabled = enableInputs;
            txtConnectionString.Enabled = enableInputs;
            btnConnect.Enabled = enableInputs;
            btnPasteString.Enabled = enableInputs;
            btnLoadConfig.Enabled = enableInputs;
            btnDisconnect.Enabled = isConnected;
        }

        private void txtConnectionField_TextChanged(object sender, EventArgs e)
        {
            if (_isParsingConnectionString) return;
            UpdateConnectionString();
        }

        private void btnPasteString_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText()) ImportarStringConexao(Clipboard.GetText());
        }

        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tabExport && !_connectionManager.IsConnected)
            {
                e.Cancel = true;
                MessageBox.Show("Conecte-se ao banco de dados para acessar esta aba.", "Acesso Negado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ImportarStringConexao(string rawString)
        {
            string pattern = @"^(?<user>[^/]+)/(?<pass>[^@]+)@(?://)?(?<host>[^:/]+):(?<port>\d+)/(?<service>.+)$";
            var match = Regex.Match(rawString.Trim(), pattern);

            if (match.Success)
            {
                _isParsingConnectionString = true;
                txtUserId.Text = match.Groups["user"].Value;
                txtPassword.Text = match.Groups["pass"].Value;
                txtHost.Text = match.Groups["host"].Value;
                txtPort.Text = match.Groups["port"].Value;
                txtServiceName.Text = match.Groups["service"].Value;
                _isParsingConnectionString = false;
                UpdateConnectionString();
                //MessageBox.Show("Dados colados com sucesso!", "Importação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnConnect_Click(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Formato inválido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                toolStripStatusLabel.Text = "Conectando...";

                // Se já estiver conectado, desconecta antes para resetar
                if (_connectionManager.IsConnected) _connectionManager.Disconnect();

                // Limpa a lista visual e os dados em memória
                gridTables.Rows.Clear();
                if (_allTables != null) _allTables.Clear();

                // Realiza a conexão
                _connectionManager.Connect();

                // Atualiza a interface (Luz Verde, Botões, Rodapé)
                UpdateConnectionStatus();

                // Se houver grupos de tabelas configurados, carrega o padrão
                if (cmbTableGroups.Items.Count > 0)
                {
                    if (cmbTableGroups.SelectedIndex == -1) cmbTableGroups.SelectedIndex = 0;
                    else RefreshTables();
                }

                // --- ALTERAÇÃO: REMOVIDO O POPUP DE SUCESSO ---
                // MessageBox.Show("Conectado!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Apenas atualiza o texto na barra inferior
                toolStripStatusLabel.Text = "Conectado";
            }
            catch (Exception ex)
            {
                UpdateConnectionStatus();
                // Em caso de erro, o popup continua sendo importante
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolStripStatusLabel.Text = "Falha ao conectar";
            }
        }
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            _connectionManager.Disconnect();
            UpdateConnectionStatus();
            toolStripStatusLabel.Text = "Desconectado";
        }

        private void btnLoadConfig_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Arquivos de Configuração (*.config)|*.config|Todos os Arquivos (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(ofd.FileName);
                        var addNodes = doc.GetElementsByTagName("add");
                        string strConexaoFull = "";

                        foreach (XmlNode node in addNodes)
                        {
                            if (node.Attributes["key"]?.Value == "strConexaoBD")
                            {
                                strConexaoFull = node.Attributes["value"]?.Value;
                                break;
                            }
                        }

                        if (!string.IsNullOrEmpty(strConexaoFull))
                        {
                            ProcessarStringConexao(strConexaoFull);
                            //MessageBox.Show("Configuração importada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btnConnect_Click(this, EventArgs.Empty);
                        }
                        else MessageBox.Show("Chave 'strConexaoBD' não encontrada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
                }
            }
        }

        private void ProcessarStringConexao(string fullString)
        {
            try
            {
                // Limpa espaços
                fullString = fullString.Trim();

                // 1. Extração do USUÁRIO (User Id, UID ou User)
                var mUser = Regex.Match(fullString, @"(?:User Id|UID|User)\s*=\s*([^;]+)", RegexOptions.IgnoreCase);
                if (mUser.Success) txtUserId.Text = mUser.Groups[1].Value.Trim();

                // 2. Extração da SENHA (Password, PWD)
                var mPass = Regex.Match(fullString, @"(?:Password|PWD)\s*=\s*([^;]+)", RegexOptions.IgnoreCase);
                if (mPass.Success) txtPassword.Text = mPass.Groups[1].Value.Trim();

                // 3. Extração do HOST (Procura HOST=valor dentro dos parênteses)
                var mHost = Regex.Match(fullString, @"HOST\s*=\s*([^)\s]+)", RegexOptions.IgnoreCase);
                if (mHost.Success) txtHost.Text = mHost.Groups[1].Value.Trim();

                // 4. Extração da PORTA
                var mPort = Regex.Match(fullString, @"PORT\s*=\s*(\d+)", RegexOptions.IgnoreCase);
                if (mPort.Success) txtPort.Text = mPort.Groups[1].Value.Trim();
                else txtPort.Text = "1521"; // Padrão se não achar

                // 5. Extração do SERVIÇO (SERVICE_NAME ou SID)
                var mService = Regex.Match(fullString, @"(?:SERVICE_NAME|SID)\s*=\s*([^)\s]+)", RegexOptions.IgnoreCase);
                if (mService.Success) txtServiceName.Text = mService.Groups[1].Value.Trim();
                else txtServiceName.Text = "XE"; // Padrão

                // 6. Atualiza a string interna do sistema
                UpdateConnectionString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao processar texto da conexão: " + ex.Message);
            }
        }

        private void LoadTableGroups()
        {
            try
            {
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TableGroups.json");
                if (!File.Exists(jsonPath)) return;

                string jsonString = File.ReadAllText(jsonPath);
                _tableGroups = JsonConvert.DeserializeObject<List<TableGroup>>(jsonString);

                cmbTableGroups.Items.Clear();
                cmbTableGroups.Items.Add("Todos");
                foreach (var group in _tableGroups) cmbTableGroups.Items.Add(group.GroupName);
                cmbTableGroups.SelectedIndex = -1;
            }
            catch { /* Ignora erro se json falhar */ }
        }

        private void RefreshTables()
        {
            if (!_connectionManager.IsConnected) return;
            if (cmbTableGroups.SelectedIndex == -1)
            {
                gridTables.Rows.Clear();
                toolStripStatusLabel.Text = "Selecione um grupo.";
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                toolStripStatusLabel.Text = "Carregando...";

                var group = _tableGroups != null ? _tableGroups.FirstOrDefault(g => g.GroupName == cmbTableGroups.Text) : null;

                _allTables = _metadataRepository.GetAllTables(
                    group != null ? group.ToWhere() : null,
                    group != null ? group.Tables : null
                );

                SortAndDisplayTables(group == null);
                toolStripStatusLabel.Text = "Tabelas carregadas";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { this.Cursor = Cursors.Default; }
        }

        private void SortAndDisplayTables(bool ordenacao)
        {
            if (_allTables == null) return;

            // 1. Filtro
            var listaFiltrada = _allTables.ToList();
            if (!string.IsNullOrEmpty(txtSearchTable.Text))
            {
                string termo = txtSearchTable.Text.Trim().ToUpper();
                listaFiltrada = listaFiltrada.Where(t => t.TableName.ToUpper().Contains(termo)).ToList();
            }

            // 2. Ordenação
            if (_sortBy == "Name")
            {
                listaFiltrada = _sortOrder == SortOrder.Ascending
                           ? listaFiltrada.OrderBy(t => t.MinMax == null ? 1 : 0).ThenBy(t => t.TableName).ToList()
                           : listaFiltrada.OrderByDescending(t => t.TableName).ToList();
            }
            else if (_sortBy == "Rows")
            {
                listaFiltrada = _sortOrder == SortOrder.Ascending
                    ? listaFiltrada.OrderBy(t => t.MinMax == null ? 1 : 0).ThenBy(t => t.NumRows).ToList()
                    : listaFiltrada.OrderByDescending(t => t.NumRows).ToList();
            }

            // 3. Preenchimento (Se "Todos", vem desmarcado. Se grupo, vem marcado)
            gridTables.Rows.Clear();
            bool marcarPadrao = (cmbTableGroups.Text != "Todos");

            foreach (var table in listaFiltrada)
            {
                int rowIdx = gridTables.Rows.Add(marcarPadrao, table.NumRows, table.TableName, table.Where);
                gridTables.Rows[rowIdx].Tag = table;
            }

            gridTables.Enabled = true;
            if (gridTables.Columns.Contains("colRows"))
                gridTables.AutoResizeColumn(gridTables.Columns["colRows"].Index, DataGridViewAutoSizeColumnMode.AllCells);
        }

        private void cmbTableGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshTables();
            txtWhereClause.Enabled = true;
            if (gridTables.SelectedRows.Count == 0)
            {
                txtWhereClause.Clear();
                txtWhereClause.Enabled = false;
            }
        }

        // ====================================================================
        // BOTÕES AUXILIARES
        // ====================================================================

        private void btnSelectAllExport_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gridTables.Rows) row.Cells["colCheck"].Value = true;
        }

        private void btnDeselectAllExport_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gridTables.Rows) row.Cells["colCheck"].Value = false;
        }

        // ====================================================================
        // EXPORTAÇÃO (WORKER)
        // ====================================================================

        private class ExportArguments
        {
            public List<TableExportData> Tables { get; set; }
            public string FilePath { get; set; }
            public string GroupName { get; set; }
            public bool Truncate { get; set; }
            public string Servidor { get; set; }
            public bool ZipOutput { get; set; }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (!_connectionManager.IsConnected)
            {
                MessageBox.Show("Conecte-se primeiro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var exportDataList = new List<TableExportData>();

            foreach (DataGridViewRow row in gridTables.Rows)
            {
                bool isChecked = Convert.ToBoolean(row.Cells["colCheck"].Value);
                if (isChecked)
                {
                    string tableName = row.Cells["colTableName"].Value?.ToString() ?? "";
                    string rawWhere = row.Cells["colWhere"].Value?.ToString() ?? "";
                    string finalWhere = rawWhere.Trim();

                    if (!string.IsNullOrEmpty(finalWhere))
                    {
                        if (finalWhere.StartsWith("WHERE", StringComparison.OrdinalIgnoreCase))
                            finalWhere = finalWhere.Substring(5).Trim();
                        finalWhere = finalWhere.TrimEnd(';');
                        finalWhere = " WHERE " + finalWhere;
                    }

                    string minMax = "";
                    if (row.Tag is TableInfo info) minMax = info.MinMax;

                    exportDataList.Add(new TableExportData { TableName = tableName, WhereClause = finalWhere, MinMax = minMax });
                }
            }

            if (exportDataList.Count == 0)
            {
                MessageBox.Show("Selecione ao menos uma tabela.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "SQL Script (*.sql;*.pdc)|*.sql;*.pdc",
                Title = "Salvar Script",
                FileName = $"{txtUserId.Text}_{cmbTableGroups.Text.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.sql.pdc"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                ExportArguments args = new ExportArguments
                {
                    Tables = exportDataList,
                    FilePath = sfd.FileName,
                    GroupName = cmbTableGroups.Text,
                    Servidor = $"{txtUserId.Text}/@{txtHost.Text}:{txtPort.Text}/{txtServiceName.Text}",
                    Truncate = chkTruncate.Checked,
                    ZipOutput = chkZipOutput.Checked
                };

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += Worker_DoWork;
                worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

                this.Cursor = Cursors.WaitCursor;
                toolStripStatusLabel.Text = "Exportando...";
                btnExport.Enabled = false;
                worker.RunWorkerAsync(args);
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ExportArguments args = (ExportArguments)e.Argument;
            _exportManager.ExportTablesDML(args.Tables, args.FilePath, args.GroupName, args.Truncate, args.Servidor);
            e.Result = args.FilePath;

            if (args.ZipOutput)
            {
                try
                {
                    string sevenZipPath = Path.Combine(Application.StartupPath, "7za.exe");
                    if (File.Exists(sevenZipPath))
                    {
                        var targetName = args.FilePath + ".7z";
                        var p = new System.Diagnostics.ProcessStartInfo();
                        p.FileName = sevenZipPath;
                        p.Arguments = $"a -t7z -m0=lzma2 -mx=9 -sdel \"{targetName}\" \"{args.FilePath}\"";
                        p.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        p.CreateNoWindow = true;
                        p.UseShellExecute = false;
                        var x = System.Diagnostics.Process.Start(p);
                        x.WaitForExit();
                        if (x.ExitCode == 0) e.Result = targetName;
                    }
                }
                catch { }
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = Cursors.Default;
            btnExport.Enabled = true;

            if (e.Error != null)
            {
                MessageBox.Show($"Erro: {e.Error.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                toolStripStatusLabel.Text = "Erro";
            }
            else
            {
                MessageBox.Show($"Concluído!\nArquivo: {e.Result}", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                toolStripStatusLabel.Text = "Concluído";
            }
        }
    }
}
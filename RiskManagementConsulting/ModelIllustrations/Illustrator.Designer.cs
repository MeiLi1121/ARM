namespace ModelIllustrations
{
    partial class Illustrator
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.bsTab = new System.Windows.Forms.TabPage();
            this.bsGraphControl = new ZedGraph.ZedGraphControl();
            this.bsDataGridView = new System.Windows.Forms.DataGridView();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Delta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Gamma = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Theta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Vega = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsInputPanel = new System.Windows.Forms.Panel();
            this.volMaxLabel = new System.Windows.Forms.Label();
            this.volMinLabel = new System.Windows.Forms.Label();
            this.timeMaxLabel = new System.Windows.Forms.Label();
            this.timeMinLabel = new System.Windows.Forms.Label();
            this.divMaxLabel = new System.Windows.Forms.Label();
            this.divMinLabel = new System.Windows.Forms.Label();
            this.rateMaxLabel = new System.Windows.Forms.Label();
            this.rateMinLabel = new System.Windows.Forms.Label();
            this.strikeMaxLabel = new System.Windows.Forms.Label();
            this.strikeMinLabel = new System.Windows.Forms.Label();
            this.spotMaxLabel = new System.Windows.Forms.Label();
            this.spotMinLabel = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.playButton = new System.Windows.Forms.Button();
            this.timeLabel = new System.Windows.Forms.Label();
            this.divLabel = new System.Windows.Forms.Label();
            this.rateLabel = new System.Windows.Forms.Label();
            this.volLabel = new System.Windows.Forms.Label();
            this.strikeLabel = new System.Windows.Forms.Label();
            this.spotLabel = new System.Windows.Forms.Label();
            this.timeBar = new System.Windows.Forms.TrackBar();
            this.divBar = new System.Windows.Forms.TrackBar();
            this.rateBar = new System.Windows.Forms.TrackBar();
            this.volBar = new System.Windows.Forms.TrackBar();
            this.strikeBar = new System.Windows.Forms.TrackBar();
            this.spotBar = new System.Windows.Forms.TrackBar();
            this.bsInputBox = new System.Windows.Forms.GroupBox();
            this.callRadioButton = new System.Windows.Forms.RadioButton();
            this.putRadioButton = new System.Windows.Forms.RadioButton();
            this.deltaTab = new System.Windows.Forms.TabPage();
            this.zedGraphControl2 = new ZedGraph.ZedGraphControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.loadButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dayToExpTextBox = new System.Windows.Forms.TextBox();
            this.divdendTextBox = new System.Windows.Forms.TextBox();
            this.rateTextBox = new System.Windows.Forms.TextBox();
            this.volTextBox = new System.Windows.Forms.TextBox();
            this.moneynessTextBox = new System.Windows.Forms.TextBox();
            this.optTypeTextBox = new System.Windows.Forms.TextBox();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.volSurfTab = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.bsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsDataGridView)).BeginInit();
            this.bsInputPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.divBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rateBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.volBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.strikeBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spotBar)).BeginInit();
            this.bsInputBox.SuspendLayout();
            this.deltaTab.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.bsTab);
            this.tabControl1.Controls.Add(this.volSurfTab);
            this.tabControl1.Controls.Add(this.deltaTab);
            this.tabControl1.Location = new System.Drawing.Point(2, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(745, 464);
            this.tabControl1.TabIndex = 0;
            // 
            // bsTab
            // 
            this.bsTab.BackColor = System.Drawing.Color.WhiteSmoke;
            this.bsTab.Controls.Add(this.bsGraphControl);
            this.bsTab.Controls.Add(this.bsDataGridView);
            this.bsTab.Controls.Add(this.bsInputPanel);
            this.bsTab.Location = new System.Drawing.Point(4, 22);
            this.bsTab.Name = "bsTab";
            this.bsTab.Padding = new System.Windows.Forms.Padding(3);
            this.bsTab.Size = new System.Drawing.Size(737, 438);
            this.bsTab.TabIndex = 0;
            this.bsTab.Text = "Black Scholes Basic";
            // 
            // bsGraphControl
            // 
            this.bsGraphControl.Location = new System.Drawing.Point(316, 71);
            this.bsGraphControl.Name = "bsGraphControl";
            this.bsGraphControl.ScrollGrace = 0D;
            this.bsGraphControl.ScrollMaxX = 0D;
            this.bsGraphControl.ScrollMaxY = 0D;
            this.bsGraphControl.ScrollMaxY2 = 0D;
            this.bsGraphControl.ScrollMinX = 0D;
            this.bsGraphControl.ScrollMinY = 0D;
            this.bsGraphControl.ScrollMinY2 = 0D;
            this.bsGraphControl.Size = new System.Drawing.Size(418, 364);
            this.bsGraphControl.TabIndex = 2;
            // 
            // bsDataGridView
            // 
            this.bsDataGridView.AllowUserToDeleteRows = false;
            this.bsDataGridView.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.bsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.bsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Price,
            this.Delta,
            this.Gamma,
            this.Theta,
            this.Vega});
            this.bsDataGridView.GridColor = System.Drawing.Color.WhiteSmoke;
            this.bsDataGridView.Location = new System.Drawing.Point(316, 21);
            this.bsDataGridView.Name = "bsDataGridView";
            this.bsDataGridView.RowHeadersVisible = false;
            this.bsDataGridView.Size = new System.Drawing.Size(415, 44);
            this.bsDataGridView.TabIndex = 1;
            // 
            // Price
            // 
            this.Price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Price.HeaderText = "Price";
            this.Price.Name = "Price";
            // 
            // Delta
            // 
            this.Delta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Delta.HeaderText = "Delta";
            this.Delta.Name = "Delta";
            // 
            // Gamma
            // 
            this.Gamma.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Gamma.HeaderText = "Gamma";
            this.Gamma.Name = "Gamma";
            // 
            // Theta
            // 
            this.Theta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Theta.HeaderText = "Theta";
            this.Theta.Name = "Theta";
            // 
            // Vega
            // 
            this.Vega.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Vega.HeaderText = "Vega";
            this.Vega.Name = "Vega";
            // 
            // bsInputPanel
            // 
            this.bsInputPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.bsInputPanel.Controls.Add(this.volMaxLabel);
            this.bsInputPanel.Controls.Add(this.volMinLabel);
            this.bsInputPanel.Controls.Add(this.timeMaxLabel);
            this.bsInputPanel.Controls.Add(this.timeMinLabel);
            this.bsInputPanel.Controls.Add(this.divMaxLabel);
            this.bsInputPanel.Controls.Add(this.divMinLabel);
            this.bsInputPanel.Controls.Add(this.rateMaxLabel);
            this.bsInputPanel.Controls.Add(this.rateMinLabel);
            this.bsInputPanel.Controls.Add(this.strikeMaxLabel);
            this.bsInputPanel.Controls.Add(this.strikeMinLabel);
            this.bsInputPanel.Controls.Add(this.spotMaxLabel);
            this.bsInputPanel.Controls.Add(this.spotMinLabel);
            this.bsInputPanel.Controls.Add(this.stopButton);
            this.bsInputPanel.Controls.Add(this.playButton);
            this.bsInputPanel.Controls.Add(this.timeLabel);
            this.bsInputPanel.Controls.Add(this.divLabel);
            this.bsInputPanel.Controls.Add(this.rateLabel);
            this.bsInputPanel.Controls.Add(this.volLabel);
            this.bsInputPanel.Controls.Add(this.strikeLabel);
            this.bsInputPanel.Controls.Add(this.spotLabel);
            this.bsInputPanel.Controls.Add(this.timeBar);
            this.bsInputPanel.Controls.Add(this.divBar);
            this.bsInputPanel.Controls.Add(this.rateBar);
            this.bsInputPanel.Controls.Add(this.volBar);
            this.bsInputPanel.Controls.Add(this.strikeBar);
            this.bsInputPanel.Controls.Add(this.spotBar);
            this.bsInputPanel.Controls.Add(this.bsInputBox);
            this.bsInputPanel.Location = new System.Drawing.Point(1, 2);
            this.bsInputPanel.Name = "bsInputPanel";
            this.bsInputPanel.Size = new System.Drawing.Size(309, 435);
            this.bsInputPanel.TabIndex = 0;
            // 
            // volMaxLabel
            // 
            this.volMaxLabel.AutoSize = true;
            this.volMaxLabel.Location = new System.Drawing.Point(252, 185);
            this.volMaxLabel.Name = "volMaxLabel";
            this.volMaxLabel.Size = new System.Drawing.Size(35, 13);
            this.volMaxLabel.TabIndex = 26;
            this.volMaxLabel.Text = "label1";
            // 
            // volMinLabel
            // 
            this.volMinLabel.AutoSize = true;
            this.volMinLabel.Location = new System.Drawing.Point(17, 185);
            this.volMinLabel.Name = "volMinLabel";
            this.volMinLabel.Size = new System.Drawing.Size(35, 13);
            this.volMinLabel.TabIndex = 25;
            this.volMinLabel.Text = "label1";
            // 
            // timeMaxLabel
            // 
            this.timeMaxLabel.AutoSize = true;
            this.timeMaxLabel.Location = new System.Drawing.Point(252, 339);
            this.timeMaxLabel.Name = "timeMaxLabel";
            this.timeMaxLabel.Size = new System.Drawing.Size(35, 13);
            this.timeMaxLabel.TabIndex = 24;
            this.timeMaxLabel.Text = "label1";
            // 
            // timeMinLabel
            // 
            this.timeMinLabel.AutoSize = true;
            this.timeMinLabel.Location = new System.Drawing.Point(17, 339);
            this.timeMinLabel.Name = "timeMinLabel";
            this.timeMinLabel.Size = new System.Drawing.Size(35, 13);
            this.timeMinLabel.TabIndex = 23;
            this.timeMinLabel.Text = "label1";
            // 
            // divMaxLabel
            // 
            this.divMaxLabel.AutoSize = true;
            this.divMaxLabel.Location = new System.Drawing.Point(252, 287);
            this.divMaxLabel.Name = "divMaxLabel";
            this.divMaxLabel.Size = new System.Drawing.Size(35, 13);
            this.divMaxLabel.TabIndex = 22;
            this.divMaxLabel.Text = "label1";
            // 
            // divMinLabel
            // 
            this.divMinLabel.AutoSize = true;
            this.divMinLabel.Location = new System.Drawing.Point(17, 287);
            this.divMinLabel.Name = "divMinLabel";
            this.divMinLabel.Size = new System.Drawing.Size(35, 13);
            this.divMinLabel.TabIndex = 21;
            this.divMinLabel.Text = "label1";
            // 
            // rateMaxLabel
            // 
            this.rateMaxLabel.AutoSize = true;
            this.rateMaxLabel.Location = new System.Drawing.Point(252, 236);
            this.rateMaxLabel.Name = "rateMaxLabel";
            this.rateMaxLabel.Size = new System.Drawing.Size(35, 13);
            this.rateMaxLabel.TabIndex = 20;
            this.rateMaxLabel.Text = "label1";
            // 
            // rateMinLabel
            // 
            this.rateMinLabel.AutoSize = true;
            this.rateMinLabel.Location = new System.Drawing.Point(17, 236);
            this.rateMinLabel.Name = "rateMinLabel";
            this.rateMinLabel.Size = new System.Drawing.Size(35, 13);
            this.rateMinLabel.TabIndex = 19;
            this.rateMinLabel.Text = "label1";
            // 
            // strikeMaxLabel
            // 
            this.strikeMaxLabel.AutoSize = true;
            this.strikeMaxLabel.Location = new System.Drawing.Point(252, 135);
            this.strikeMaxLabel.Name = "strikeMaxLabel";
            this.strikeMaxLabel.Size = new System.Drawing.Size(35, 13);
            this.strikeMaxLabel.TabIndex = 18;
            this.strikeMaxLabel.Text = "label1";
            // 
            // strikeMinLabel
            // 
            this.strikeMinLabel.AutoSize = true;
            this.strikeMinLabel.Location = new System.Drawing.Point(17, 135);
            this.strikeMinLabel.Name = "strikeMinLabel";
            this.strikeMinLabel.Size = new System.Drawing.Size(35, 13);
            this.strikeMinLabel.TabIndex = 17;
            this.strikeMinLabel.Text = "label1";
            // 
            // spotMaxLabel
            // 
            this.spotMaxLabel.AutoSize = true;
            this.spotMaxLabel.Location = new System.Drawing.Point(252, 84);
            this.spotMaxLabel.Name = "spotMaxLabel";
            this.spotMaxLabel.Size = new System.Drawing.Size(35, 13);
            this.spotMaxLabel.TabIndex = 16;
            this.spotMaxLabel.Text = "label1";
            // 
            // spotMinLabel
            // 
            this.spotMinLabel.AutoSize = true;
            this.spotMinLabel.Location = new System.Drawing.Point(17, 84);
            this.spotMinLabel.Name = "spotMinLabel";
            this.spotMinLabel.Size = new System.Drawing.Size(35, 13);
            this.spotMinLabel.TabIndex = 15;
            this.spotMinLabel.Text = "label1";
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(156, 371);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(90, 41);
            this.stopButton.TabIndex = 14;
            this.stopButton.Text = "STOP";
            this.stopButton.UseVisualStyleBackColor = true;
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(52, 371);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(90, 41);
            this.playButton.TabIndex = 13;
            this.playButton.Text = "PLAY";
            this.playButton.UseVisualStyleBackColor = true;
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(133, 319);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(30, 13);
            this.timeLabel.TabIndex = 12;
            this.timeLabel.Text = "Time";
            // 
            // divLabel
            // 
            this.divLabel.AutoSize = true;
            this.divLabel.Location = new System.Drawing.Point(123, 268);
            this.divLabel.Name = "divLabel";
            this.divLabel.Size = new System.Drawing.Size(49, 13);
            this.divLabel.TabIndex = 11;
            this.divLabel.Text = "Dividend";
            // 
            // rateLabel
            // 
            this.rateLabel.AutoSize = true;
            this.rateLabel.Location = new System.Drawing.Point(133, 219);
            this.rateLabel.Name = "rateLabel";
            this.rateLabel.Size = new System.Drawing.Size(30, 13);
            this.rateLabel.TabIndex = 10;
            this.rateLabel.Text = "Rate";
            // 
            // volLabel
            // 
            this.volLabel.AutoSize = true;
            this.volLabel.Location = new System.Drawing.Point(127, 168);
            this.volLabel.Name = "volLabel";
            this.volLabel.Size = new System.Drawing.Size(45, 13);
            this.volLabel.TabIndex = 9;
            this.volLabel.Text = "Volatility";
            // 
            // strikeLabel
            // 
            this.strikeLabel.AutoSize = true;
            this.strikeLabel.Location = new System.Drawing.Point(132, 119);
            this.strikeLabel.Name = "strikeLabel";
            this.strikeLabel.Size = new System.Drawing.Size(34, 13);
            this.strikeLabel.TabIndex = 8;
            this.strikeLabel.Text = "Strike";
            // 
            // spotLabel
            // 
            this.spotLabel.AutoSize = true;
            this.spotLabel.Location = new System.Drawing.Point(136, 67);
            this.spotLabel.Name = "spotLabel";
            this.spotLabel.Size = new System.Drawing.Size(29, 13);
            this.spotLabel.TabIndex = 7;
            this.spotLabel.Text = "Spot";
            // 
            // timeBar
            // 
            this.timeBar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.timeBar.Location = new System.Drawing.Point(52, 335);
            this.timeBar.Name = "timeBar";
            this.timeBar.Size = new System.Drawing.Size(194, 45);
            this.timeBar.TabIndex = 6;
            this.timeBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.timeBar.Value = 10;
            this.timeBar.ValueChanged += new System.EventHandler(this.timeBar_Scroll);
            this.timeBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.timeBarReset);
            // 
            // divBar
            // 
            this.divBar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.divBar.Location = new System.Drawing.Point(52, 284);
            this.divBar.Name = "divBar";
            this.divBar.Size = new System.Drawing.Size(194, 45);
            this.divBar.TabIndex = 5;
            this.divBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.divBar.Value = 10;
            this.divBar.ValueChanged += new System.EventHandler(this.divBar_Scroll);
            this.divBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.divBarReset);
            // 
            // rateBar
            // 
            this.rateBar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rateBar.Location = new System.Drawing.Point(52, 233);
            this.rateBar.Name = "rateBar";
            this.rateBar.Size = new System.Drawing.Size(194, 45);
            this.rateBar.TabIndex = 4;
            this.rateBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.rateBar.Value = 10;
            this.rateBar.ValueChanged += new System.EventHandler(this.rateBar_Scroll);
            this.rateBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.rateBarReset);
            // 
            // volBar
            // 
            this.volBar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.volBar.Location = new System.Drawing.Point(52, 182);
            this.volBar.Name = "volBar";
            this.volBar.Size = new System.Drawing.Size(194, 45);
            this.volBar.TabIndex = 3;
            this.volBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.volBar.Value = 10;
            this.volBar.ValueChanged += new System.EventHandler(this.volBar_Scroll);
            this.volBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.volBarReset);
            // 
            // strikeBar
            // 
            this.strikeBar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.strikeBar.Location = new System.Drawing.Point(52, 131);
            this.strikeBar.Name = "strikeBar";
            this.strikeBar.Size = new System.Drawing.Size(194, 45);
            this.strikeBar.TabIndex = 2;
            this.strikeBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.strikeBar.Value = 10;
            this.strikeBar.ValueChanged += new System.EventHandler(this.strikeBar_Scroll);
            this.strikeBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.strikeBarReset);
            // 
            // spotBar
            // 
            this.spotBar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.spotBar.Location = new System.Drawing.Point(52, 80);
            this.spotBar.Name = "spotBar";
            this.spotBar.Size = new System.Drawing.Size(194, 45);
            this.spotBar.TabIndex = 1;
            this.spotBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.spotBar.Value = 10;
            this.spotBar.ValueChanged += new System.EventHandler(this.spotBar_Scroll);
            this.spotBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.spotBarReset);
            // 
            // bsInputBox
            // 
            this.bsInputBox.BackColor = System.Drawing.Color.Transparent;
            this.bsInputBox.Controls.Add(this.callRadioButton);
            this.bsInputBox.Controls.Add(this.putRadioButton);
            this.bsInputBox.Location = new System.Drawing.Point(52, 7);
            this.bsInputBox.Name = "bsInputBox";
            this.bsInputBox.Size = new System.Drawing.Size(194, 51);
            this.bsInputBox.TabIndex = 0;
            this.bsInputBox.TabStop = false;
            // 
            // callRadioButton
            // 
            this.callRadioButton.AutoSize = true;
            this.callRadioButton.Location = new System.Drawing.Point(115, 20);
            this.callRadioButton.Name = "callRadioButton";
            this.callRadioButton.Size = new System.Drawing.Size(51, 17);
            this.callRadioButton.TabIndex = 1;
            this.callRadioButton.Text = "CALL";
            this.callRadioButton.UseVisualStyleBackColor = true;
            this.callRadioButton.CheckedChanged += new System.EventHandler(this.callRadioButton_CheckedChanged);
            // 
            // putRadioButton
            // 
            this.putRadioButton.AutoSize = true;
            this.putRadioButton.Checked = true;
            this.putRadioButton.Location = new System.Drawing.Point(31, 20);
            this.putRadioButton.Name = "putRadioButton";
            this.putRadioButton.Size = new System.Drawing.Size(47, 17);
            this.putRadioButton.TabIndex = 0;
            this.putRadioButton.TabStop = true;
            this.putRadioButton.Text = "PUT";
            this.putRadioButton.UseVisualStyleBackColor = true;
            this.putRadioButton.CheckedChanged += new System.EventHandler(this.putRadioButton_CheckedChanged);
            // 
            // deltaTab
            // 
            this.deltaTab.BackColor = System.Drawing.Color.LightGray;
            this.deltaTab.Controls.Add(this.zedGraphControl2);
            this.deltaTab.Controls.Add(this.groupBox1);
            this.deltaTab.Controls.Add(this.zedGraphControl1);
            this.deltaTab.Location = new System.Drawing.Point(4, 22);
            this.deltaTab.Name = "deltaTab";
            this.deltaTab.Padding = new System.Windows.Forms.Padding(3);
            this.deltaTab.Size = new System.Drawing.Size(737, 438);
            this.deltaTab.TabIndex = 1;
            this.deltaTab.Text = "Delta Hedging Simulator";
            // 
            // zedGraphControl2
            // 
            this.zedGraphControl2.Location = new System.Drawing.Point(171, 6);
            this.zedGraphControl2.Name = "zedGraphControl2";
            this.zedGraphControl2.ScrollGrace = 0D;
            this.zedGraphControl2.ScrollMaxX = 0D;
            this.zedGraphControl2.ScrollMaxY = 0D;
            this.zedGraphControl2.ScrollMaxY2 = 0D;
            this.zedGraphControl2.ScrollMinX = 0D;
            this.zedGraphControl2.ScrollMinY = 0D;
            this.zedGraphControl2.ScrollMinY2 = 0D;
            this.zedGraphControl2.Size = new System.Drawing.Size(565, 357);
            this.zedGraphControl2.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.LightGray;
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox1.Controls.Add(this.loadButton);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dayToExpTextBox);
            this.groupBox1.Controls.Add(this.divdendTextBox);
            this.groupBox1.Controls.Add(this.rateTextBox);
            this.groupBox1.Controls.Add(this.volTextBox);
            this.groupBox1.Controls.Add(this.moneynessTextBox);
            this.groupBox1.Controls.Add(this.optTypeTextBox);
            this.groupBox1.Location = new System.Drawing.Point(7, -1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(158, 436);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(15, 19);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(127, 43);
            this.loadButton.TabIndex = 12;
            this.loadButton.Text = "Load Data";
            this.loadButton.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 206);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Days Expire";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 180);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Dividend";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Rate";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Volatility";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Moneyness";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Option Type";
            // 
            // dayToExpTextBox
            // 
            this.dayToExpTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.dayToExpTextBox.Location = new System.Drawing.Point(81, 203);
            this.dayToExpTextBox.Name = "dayToExpTextBox";
            this.dayToExpTextBox.Size = new System.Drawing.Size(71, 20);
            this.dayToExpTextBox.TabIndex = 5;
            // 
            // divdendTextBox
            // 
            this.divdendTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.divdendTextBox.Location = new System.Drawing.Point(81, 177);
            this.divdendTextBox.Name = "divdendTextBox";
            this.divdendTextBox.Size = new System.Drawing.Size(71, 20);
            this.divdendTextBox.TabIndex = 4;
            // 
            // rateTextBox
            // 
            this.rateTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.rateTextBox.Location = new System.Drawing.Point(81, 151);
            this.rateTextBox.Name = "rateTextBox";
            this.rateTextBox.Size = new System.Drawing.Size(71, 20);
            this.rateTextBox.TabIndex = 3;
            // 
            // volTextBox
            // 
            this.volTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.volTextBox.Location = new System.Drawing.Point(81, 125);
            this.volTextBox.Name = "volTextBox";
            this.volTextBox.Size = new System.Drawing.Size(71, 20);
            this.volTextBox.TabIndex = 2;
            // 
            // moneynessTextBox
            // 
            this.moneynessTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.moneynessTextBox.Location = new System.Drawing.Point(81, 99);
            this.moneynessTextBox.Name = "moneynessTextBox";
            this.moneynessTextBox.Size = new System.Drawing.Size(71, 20);
            this.moneynessTextBox.TabIndex = 1;
            // 
            // optTypeTextBox
            // 
            this.optTypeTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.optTypeTextBox.Location = new System.Drawing.Point(81, 73);
            this.optTypeTextBox.Name = "optTypeTextBox";
            this.optTypeTextBox.Size = new System.Drawing.Size(71, 20);
            this.optTypeTextBox.TabIndex = 0;
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Location = new System.Drawing.Point(171, 369);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(565, 68);
            this.zedGraphControl1.TabIndex = 0;
            // 
            // volSurfTab
            // 
            this.volSurfTab.Location = new System.Drawing.Point(4, 22);
            this.volSurfTab.Name = "volSurfTab";
            this.volSurfTab.Padding = new System.Windows.Forms.Padding(3);
            this.volSurfTab.Size = new System.Drawing.Size(737, 438);
            this.volSurfTab.TabIndex = 2;
            this.volSurfTab.Text = "Volatility Surface";
            this.volSurfTab.UseVisualStyleBackColor = true;
            // 
            // Illustrator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 466);
            this.Controls.Add(this.tabControl1);
            this.Name = "Illustrator";
            this.Text = "Model Illustrations";
            this.tabControl1.ResumeLayout(false);
            this.bsTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsDataGridView)).EndInit();
            this.bsInputPanel.ResumeLayout(false);
            this.bsInputPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.divBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rateBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.volBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.strikeBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spotBar)).EndInit();
            this.bsInputBox.ResumeLayout(false);
            this.bsInputBox.PerformLayout();
            this.deltaTab.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage bsTab;
        private System.Windows.Forms.TabPage deltaTab;
        private System.Windows.Forms.Panel bsInputPanel;
        private System.Windows.Forms.GroupBox bsInputBox;
        private System.Windows.Forms.RadioButton callRadioButton;
        private System.Windows.Forms.RadioButton putRadioButton;
        private System.Windows.Forms.TrackBar timeBar;
        private System.Windows.Forms.TrackBar divBar;
        private System.Windows.Forms.TrackBar rateBar;
        private System.Windows.Forms.TrackBar volBar;
        private System.Windows.Forms.TrackBar strikeBar;
        private System.Windows.Forms.TrackBar spotBar;
        private System.Windows.Forms.Label volLabel;
        private System.Windows.Forms.Label strikeLabel;
        private System.Windows.Forms.Label spotLabel;
        private System.Windows.Forms.Label divLabel;
        private System.Windows.Forms.Label rateLabel;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.DataGridView bsDataGridView;
        private ZedGraph.ZedGraphControl bsGraphControl;
        private System.Windows.Forms.Label volMaxLabel;
        private System.Windows.Forms.Label volMinLabel;
        private System.Windows.Forms.Label timeMaxLabel;
        private System.Windows.Forms.Label timeMinLabel;
        private System.Windows.Forms.Label divMaxLabel;
        private System.Windows.Forms.Label divMinLabel;
        private System.Windows.Forms.Label rateMaxLabel;
        private System.Windows.Forms.Label rateMinLabel;
        private System.Windows.Forms.Label strikeMaxLabel;
        private System.Windows.Forms.Label strikeMinLabel;
        private System.Windows.Forms.Label spotMaxLabel;
        private System.Windows.Forms.Label spotMinLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn Delta;
        private System.Windows.Forms.DataGridViewTextBoxColumn Gamma;
        private System.Windows.Forms.DataGridViewTextBoxColumn Theta;
        private System.Windows.Forms.DataGridViewTextBoxColumn Vega;
        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox optTypeTextBox;
        private System.Windows.Forms.TextBox volTextBox;
        private System.Windows.Forms.TextBox moneynessTextBox;
        private System.Windows.Forms.TextBox dayToExpTextBox;
        private System.Windows.Forms.TextBox divdendTextBox;
        private System.Windows.Forms.TextBox rateTextBox;
        private ZedGraph.ZedGraphControl zedGraphControl2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.TabPage volSurfTab;

       
    }
}


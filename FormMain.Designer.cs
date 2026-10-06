namespace EarthquakeTracker
{
  partial class FormMain
  {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

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
      panelTop = new Panel();
      dateTimePickerEndingDate = new DateTimePicker();
      label12 = new Label();
      buttonGetEarthquakes = new Button();
      numericUpDownLowestMagnitude = new NumericUpDown();
      label2 = new Label();
      dateTimePickerStartingDate = new DateTimePicker();
      label1 = new Label();
      panelBottom = new Panel();
      labelStatusMessage = new Label();
      labelZoomLevel = new Label();
      labelRefreshedAt = new Label();
      panelMiddle = new Panel();
      earthquakeMap = new EarthquakeMap();
      panelMagnitudeColors = new Panel();
      label11 = new Label();
      label10 = new Label();
      label9 = new Label();
      label4 = new Label();
      label8 = new Label();
      label3 = new Label();
      label7 = new Label();
      label6 = new Label();
      label5 = new Label();
      panelTop.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)numericUpDownLowestMagnitude).BeginInit();
      panelBottom.SuspendLayout();
      panelMiddle.SuspendLayout();
      panelMagnitudeColors.SuspendLayout();
      SuspendLayout();
      // 
      // panelTop
      // 
      panelTop.BorderStyle = BorderStyle.Fixed3D;
      panelTop.Controls.Add(dateTimePickerEndingDate);
      panelTop.Controls.Add(label12);
      panelTop.Controls.Add(buttonGetEarthquakes);
      panelTop.Controls.Add(numericUpDownLowestMagnitude);
      panelTop.Controls.Add(label2);
      panelTop.Controls.Add(dateTimePickerStartingDate);
      panelTop.Controls.Add(label1);
      panelTop.Dock = DockStyle.Top;
      panelTop.Location = new Point(0, 0);
      panelTop.Name = "panelTop";
      panelTop.Size = new Size(979, 39);
      panelTop.TabIndex = 0;
      // 
      // dateTimePickerEndingDate
      // 
      dateTimePickerEndingDate.Format = DateTimePickerFormat.Short;
      dateTimePickerEndingDate.Location = new Point(318, 8);
      dateTimePickerEndingDate.Name = "dateTimePickerEndingDate";
      dateTimePickerEndingDate.Size = new Size(101, 23);
      dateTimePickerEndingDate.TabIndex = 3;
      // 
      // label12
      // 
      label12.AutoSize = true;
      label12.Location = new Point(245, 12);
      label12.Name = "label12";
      label12.Size = new Size(71, 15);
      label12.TabIndex = 2;
      label12.Text = "Ending Date";
      // 
      // buttonGetEarthquakes
      // 
      buttonGetEarthquakes.Location = new Point(703, 9);
      buttonGetEarthquakes.Name = "buttonGetEarthquakes";
      buttonGetEarthquakes.Size = new Size(124, 23);
      buttonGetEarthquakes.TabIndex = 6;
      buttonGetEarthquakes.Text = "Get Earthquakes";
      buttonGetEarthquakes.UseVisualStyleBackColor = true;
      buttonGetEarthquakes.Click += ButtonGetEarthquakes_Click;
      // 
      // numericUpDownLowestMagnitude
      // 
      numericUpDownLowestMagnitude.DecimalPlaces = 1;
      numericUpDownLowestMagnitude.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
      numericUpDownLowestMagnitude.Location = new Point(595, 10);
      numericUpDownLowestMagnitude.Maximum = new decimal(new int[] { 8, 0, 0, 0 });
      numericUpDownLowestMagnitude.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
      numericUpDownLowestMagnitude.Name = "numericUpDownLowestMagnitude";
      numericUpDownLowestMagnitude.Size = new Size(54, 23);
      numericUpDownLowestMagnitude.TabIndex = 5;
      numericUpDownLowestMagnitude.Value = new decimal(new int[] { 25, 0, 0, 65536 });
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new Point(487, 12);
      label2.Name = "label2";
      label2.Size = new Size(105, 15);
      label2.TabIndex = 4;
      label2.Text = "Lowest Magnitude";
      // 
      // dateTimePickerStartingDate
      // 
      dateTimePickerStartingDate.Format = DateTimePickerFormat.Short;
      dateTimePickerStartingDate.Location = new Point(98, 8);
      dateTimePickerStartingDate.Name = "dateTimePickerStartingDate";
      dateTimePickerStartingDate.Size = new Size(101, 23);
      dateTimePickerStartingDate.TabIndex = 1;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new Point(20, 12);
      label1.Name = "label1";
      label1.Size = new Size(75, 15);
      label1.TabIndex = 0;
      label1.Text = "Starting Date";
      // 
      // panelBottom
      // 
      panelBottom.BorderStyle = BorderStyle.FixedSingle;
      panelBottom.Controls.Add(labelStatusMessage);
      panelBottom.Controls.Add(labelZoomLevel);
      panelBottom.Controls.Add(labelRefreshedAt);
      panelBottom.Dock = DockStyle.Bottom;
      panelBottom.Location = new Point(0, 573);
      panelBottom.Name = "panelBottom";
      panelBottom.Size = new Size(979, 25);
      panelBottom.TabIndex = 1;
      // 
      // labelStatusMessage
      // 
      labelStatusMessage.AutoEllipsis = true;
      labelStatusMessage.Dock = DockStyle.Fill;
      labelStatusMessage.Location = new Point(0, 0);
      labelStatusMessage.Name = "labelStatusMessage";
      labelStatusMessage.Size = new Size(751, 23);
      labelStatusMessage.TabIndex = 2;
      labelStatusMessage.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // labelZoomLevel
      // 
      labelZoomLevel.Dock = DockStyle.Right;
      labelZoomLevel.Location = new Point(751, 0);
      labelZoomLevel.Name = "labelZoomLevel";
      labelZoomLevel.Size = new Size(100, 23);
      labelZoomLevel.TabIndex = 1;
      labelZoomLevel.Text = "Zoom Level: 4";
      labelZoomLevel.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // labelRefreshedAt
      // 
      labelRefreshedAt.Dock = DockStyle.Right;
      labelRefreshedAt.Location = new Point(851, 0);
      labelRefreshedAt.Name = "labelRefreshedAt";
      labelRefreshedAt.Size = new Size(126, 23);
      labelRefreshedAt.TabIndex = 0;
      labelRefreshedAt.Text = "Refreshed at: 18:02:34";
      labelRefreshedAt.TextAlign = ContentAlignment.MiddleLeft;
      // 
      // panelMiddle
      // 
      panelMiddle.Controls.Add(earthquakeMap);
      panelMiddle.Controls.Add(panelMagnitudeColors);
      panelMiddle.Dock = DockStyle.Fill;
      panelMiddle.Location = new Point(0, 39);
      panelMiddle.Name = "panelMiddle";
      panelMiddle.Size = new Size(979, 534);
      panelMiddle.TabIndex = 2;
      // 
      // earthquakeMap
      // 
      earthquakeMap.BackColor = Color.FromArgb(170, 211, 223);
      earthquakeMap.Dock = DockStyle.Fill;
      earthquakeMap.Location = new Point(0, 34);
      earthquakeMap.MaxZoom = 14;
      earthquakeMap.MinZoom = 3;
      earthquakeMap.Name = "earthquakeMap";
      earthquakeMap.Size = new Size(979, 500);
      earthquakeMap.TabIndex = 0;
      earthquakeMap.StatusChanged += EarthquakeMap_StatusChanged;
      earthquakeMap.ZoomLevelChanged += EarthquakeMap_ZoomLevelChanged;
      // 
      // panelMagnitudeColors
      // 
      panelMagnitudeColors.BorderStyle = BorderStyle.FixedSingle;
      panelMagnitudeColors.Controls.Add(label11);
      panelMagnitudeColors.Controls.Add(label10);
      panelMagnitudeColors.Controls.Add(label9);
      panelMagnitudeColors.Controls.Add(label4);
      panelMagnitudeColors.Controls.Add(label8);
      panelMagnitudeColors.Controls.Add(label3);
      panelMagnitudeColors.Controls.Add(label7);
      panelMagnitudeColors.Controls.Add(label6);
      panelMagnitudeColors.Controls.Add(label5);
      panelMagnitudeColors.Dock = DockStyle.Top;
      panelMagnitudeColors.Location = new Point(0, 0);
      panelMagnitudeColors.Name = "panelMagnitudeColors";
      panelMagnitudeColors.Size = new Size(979, 34);
      panelMagnitudeColors.TabIndex = 0;
      panelMagnitudeColors.Visible = false;
      // 
      // label11
      // 
      label11.AutoSize = true;
      label11.Location = new Point(527, 9);
      label11.Name = "label11";
      label11.Size = new Size(24, 15);
      label11.TabIndex = 14;
      label11.Text = "> 7";
      // 
      // label10
      // 
      label10.AutoSize = true;
      label10.Location = new Point(421, 9);
      label10.Name = "label10";
      label10.Size = new Size(36, 15);
      label10.TabIndex = 13;
      label10.Text = "5 to 7";
      // 
      // label9
      // 
      label9.AutoSize = true;
      label9.Location = new Point(305, 9);
      label9.Name = "label9";
      label9.Size = new Size(36, 15);
      label9.TabIndex = 12;
      label9.Text = "3 to 5";
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new Point(207, 9);
      label4.Name = "label4";
      label4.Size = new Size(24, 15);
      label4.TabIndex = 11;
      label4.Text = "< 3";
      // 
      // label8
      // 
      label8.BackColor = Color.Gold;
      label8.BorderStyle = BorderStyle.FixedSingle;
      label8.Location = new Point(181, 4);
      label8.Name = "label8";
      label8.Size = new Size(23, 23);
      label8.TabIndex = 10;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.ForeColor = Color.Teal;
      label3.Location = new Point(20, 7);
      label3.Name = "label3";
      label3.Size = new Size(102, 15);
      label3.TabIndex = 5;
      label3.Text = "Magnitude Colors";
      // 
      // label7
      // 
      label7.BackColor = Color.Orange;
      label7.BorderStyle = BorderStyle.FixedSingle;
      label7.Location = new Point(279, 4);
      label7.Name = "label7";
      label7.Size = new Size(23, 23);
      label7.TabIndex = 9;
      // 
      // label6
      // 
      label6.BackColor = Color.OrangeRed;
      label6.BorderStyle = BorderStyle.FixedSingle;
      label6.Location = new Point(395, 4);
      label6.Name = "label6";
      label6.Size = new Size(23, 23);
      label6.TabIndex = 8;
      // 
      // label5
      // 
      label5.BackColor = Color.Firebrick;
      label5.BorderStyle = BorderStyle.FixedSingle;
      label5.Location = new Point(501, 4);
      label5.Name = "label5";
      label5.Size = new Size(23, 23);
      label5.TabIndex = 7;
      // 
      // FormMain
      // 
      AcceptButton = buttonGetEarthquakes;
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(979, 598);
      Controls.Add(panelMiddle);
      Controls.Add(panelBottom);
      Controls.Add(panelTop);
      MinimumSize = new Size(300, 300);
      Name = "FormMain";
      StartPosition = FormStartPosition.CenterScreen;
      Text = "Earthquake Tracker";
      panelTop.ResumeLayout(false);
      panelTop.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)numericUpDownLowestMagnitude).EndInit();
      panelBottom.ResumeLayout(false);
      panelMiddle.ResumeLayout(false);
      panelMagnitudeColors.ResumeLayout(false);
      panelMagnitudeColors.PerformLayout();
      ResumeLayout(false);
    }

    #endregion

    private Panel panelTop;
    private Button buttonGetEarthquakes;
    private NumericUpDown numericUpDownLowestMagnitude;
    private Label label2;
    private DateTimePicker dateTimePickerStartingDate;
    private Label label1;
    private Panel panelBottom;
    private Panel panelMiddle;
    private Label labelStatusMessage;
    private Label labelZoomLevel;
    private Label labelRefreshedAt;
    private EarthquakeMap earthquakeMap;
    private Label label3;
    private Label label8;
    private Label label7;
    private Label label6;
    private Label label5;
    private Panel panelMagnitudeColors;
    private Label label4;
    private Label label11;
    private Label label10;
    private Label label9;
    private DateTimePicker dateTimePickerEndingDate;
    private Label label12;
  }
}

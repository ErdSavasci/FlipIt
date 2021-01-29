namespace ScreenSaver
{
    partial class SettingsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cityTextbox = new System.Windows.Forms.TextBox();
            this.addButton = new System.Windows.Forms.Button();
            this.cityListview = new System.Windows.Forms.ListView();
            this.cityName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timezone = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.removeBtn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timezonesCombo = new System.Windows.Forms.ComboBox();
            this.persistentFrameImageBox = new System.Windows.Forms.PictureBox();
            this.amPm12HoursIndicatorCheckbox = new System.Windows.Forms.CheckBox();
            this.showSecondsCheckbox = new System.Windows.Forms.CheckBox();
            this.sortByTimeCheckbox = new System.Windows.Forms.RadioButton();
            this.sortByNameCheckbox = new System.Windows.Forms.RadioButton();
            this.timeBoxScaleTrackBar = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.multiMonitorStatusLabel = new System.Windows.Forms.Label();
            this.multiMonitorStatusResultLabel = new System.Windows.Forms.Label();
            this.previewTimer = new System.Windows.Forms.Timer(this.components);
            this.showWorldCityClockAtMainCheckbox = new System.Windows.Forms.CheckBox();
            this.manualTimezoneCheckbox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.persistentFrameImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeBoxScaleTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(15, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "Flip It v1.3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "By Phil Haselden";
            // 
            // saveButton
            // 
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.Location = new System.Drawing.Point(20, 274);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(70, 23);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.CausesValidation = false;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cancelButton.Location = new System.Drawing.Point(98, 274);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(70, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "(Modified by Erdem Savasci)";
            // 
            // cityTextbox
            // 
            this.cityTextbox.Location = new System.Drawing.Point(20, 100);
            this.cityTextbox.Name = "cityTextbox";
            this.cityTextbox.Size = new System.Drawing.Size(106, 22);
            this.cityTextbox.TabIndex = 8;
            this.cityTextbox.Enter += new System.EventHandler(this.CityTextbox_Enter);
            this.cityTextbox.Leave += new System.EventHandler(this.CityTextbox_Leave);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(395, 99);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(43, 23);
            this.addButton.TabIndex = 9;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // cityListview
            // 
            this.cityListview.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cityName,
            this.timezone,
            this.removeBtn});
            this.cityListview.FullRowSelect = true;
            this.cityListview.GridLines = true;
            this.cityListview.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.cityListview.HideSelection = false;
            this.cityListview.Location = new System.Drawing.Point(20, 128);
            this.cityListview.Name = "cityListview";
            this.cityListview.Size = new System.Drawing.Size(418, 140);
            this.cityListview.TabIndex = 10;
            this.cityListview.UseCompatibleStateImageBehavior = false;
            this.cityListview.View = System.Windows.Forms.View.Details;
            // 
            // cityName
            // 
            this.cityName.Text = "City Name";
            this.cityName.Width = 100;
            // 
            // timezone
            // 
            this.timezone.Text = "Timezone";
            this.timezone.Width = 200;
            // 
            // removeBtn
            // 
            this.removeBtn.Text = "Action";
            this.removeBtn.Width = 90;
            // 
            // timezonesCombo
            // 
            this.timezonesCombo.FormattingEnabled = true;
            this.timezonesCombo.Location = new System.Drawing.Point(203, 100);
            this.timezonesCombo.Name = "timezonesCombo";
            this.timezonesCombo.Size = new System.Drawing.Size(186, 21);
            this.timezonesCombo.TabIndex = 11;
            // 
            // persistentFrameImageBox
            // 
            this.persistentFrameImageBox.Image = global::ScreenSaver.Properties.Resources.black;
            this.persistentFrameImageBox.Location = new System.Drawing.Point(444, 11);
            this.persistentFrameImageBox.Name = "persistentFrameImageBox";
            this.persistentFrameImageBox.Size = new System.Drawing.Size(123, 75);
            this.persistentFrameImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.persistentFrameImageBox.TabIndex = 12;
            this.persistentFrameImageBox.TabStop = false;
            this.persistentFrameImageBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PersistentFrameImageBox_MouseClick);
            // 
            // amPm12HoursIndicatorCheckbox
            // 
            this.amPm12HoursIndicatorCheckbox.AutoSize = true;
            this.amPm12HoursIndicatorCheckbox.Location = new System.Drawing.Point(446, 128);
            this.amPm12HoursIndicatorCheckbox.Name = "amPm12HoursIndicatorCheckbox";
            this.amPm12HoursIndicatorCheckbox.Size = new System.Drawing.Size(95, 17);
            this.amPm12HoursIndicatorCheckbox.TabIndex = 13;
            this.amPm12HoursIndicatorCheckbox.Text = "Show AM/PM";
            this.amPm12HoursIndicatorCheckbox.UseVisualStyleBackColor = true;
            this.amPm12HoursIndicatorCheckbox.CheckedChanged += new System.EventHandler(this.AmPm12HoursIndicatorCheckbox_CheckedChanged);
            // 
            // showSecondsCheckbox
            // 
            this.showSecondsCheckbox.AutoSize = true;
            this.showSecondsCheckbox.Location = new System.Drawing.Point(446, 151);
            this.showSecondsCheckbox.Name = "showSecondsCheckbox";
            this.showSecondsCheckbox.Size = new System.Drawing.Size(101, 17);
            this.showSecondsCheckbox.TabIndex = 14;
            this.showSecondsCheckbox.Text = "Show Seconds";
            this.showSecondsCheckbox.UseVisualStyleBackColor = true;
            this.showSecondsCheckbox.CheckedChanged += new System.EventHandler(this.ShowSecondsCheckbox_CheckedChanged);
            // 
            // sortByTimeCheckbox
            // 
            this.sortByTimeCheckbox.AutoSize = true;
            this.sortByTimeCheckbox.Location = new System.Drawing.Point(446, 250);
            this.sortByTimeCheckbox.Name = "sortByTimeCheckbox";
            this.sortByTimeCheckbox.Size = new System.Drawing.Size(118, 17);
            this.sortByTimeCheckbox.TabIndex = 15;
            this.sortByTimeCheckbox.TabStop = true;
            this.sortByTimeCheckbox.Text = "Sort Cities By Time";
            this.sortByTimeCheckbox.UseVisualStyleBackColor = true;
            this.sortByTimeCheckbox.CheckedChanged += new System.EventHandler(this.SortByTimeCheckbox_CheckedChanged);
            // 
            // sortByNameCheckbox
            // 
            this.sortByNameCheckbox.AutoSize = true;
            this.sortByNameCheckbox.Location = new System.Drawing.Point(446, 227);
            this.sortByNameCheckbox.Name = "sortByNameCheckbox";
            this.sortByNameCheckbox.Size = new System.Drawing.Size(123, 17);
            this.sortByNameCheckbox.TabIndex = 16;
            this.sortByNameCheckbox.TabStop = true;
            this.sortByNameCheckbox.Text = "Sort Cities By Name";
            this.sortByNameCheckbox.UseVisualStyleBackColor = true;
            this.sortByNameCheckbox.CheckedChanged += new System.EventHandler(this.SortByNameCheckbox_CheckedChanged);
            // 
            // timeBoxScaleTrackBar
            // 
            this.timeBoxScaleTrackBar.LargeChange = 1;
            this.timeBoxScaleTrackBar.Location = new System.Drawing.Point(438, 184);
            this.timeBoxScaleTrackBar.Maximum = 5;
            this.timeBoxScaleTrackBar.Minimum = 1;
            this.timeBoxScaleTrackBar.Name = "timeBoxScaleTrackBar";
            this.timeBoxScaleTrackBar.Size = new System.Drawing.Size(129, 45);
            this.timeBoxScaleTrackBar.TabIndex = 17;
            this.timeBoxScaleTrackBar.Value = 1;
            this.timeBoxScaleTrackBar.ValueChanged += new System.EventHandler(this.TimeBoxScaleTrackBar_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(443, 171);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Scale: ";
            // 
            // multiMonitorStatusLabel
            // 
            this.multiMonitorStatusLabel.AutoSize = true;
            this.multiMonitorStatusLabel.Location = new System.Drawing.Point(321, 11);
            this.multiMonitorStatusLabel.Name = "multiMonitorStatusLabel";
            this.multiMonitorStatusLabel.Size = new System.Drawing.Size(117, 13);
            this.multiMonitorStatusLabel.TabIndex = 19;
            this.multiMonitorStatusLabel.Text = "Multi Monitor Status:";
            // 
            // multiMonitorStatusResultLabel
            // 
            this.multiMonitorStatusResultLabel.AutoSize = true;
            this.multiMonitorStatusResultLabel.Location = new System.Drawing.Point(321, 29);
            this.multiMonitorStatusResultLabel.Name = "multiMonitorStatusResultLabel";
            this.multiMonitorStatusResultLabel.Size = new System.Drawing.Size(11, 13);
            this.multiMonitorStatusResultLabel.TabIndex = 20;
            this.multiMonitorStatusResultLabel.Text = "-";
            // 
            // previewTimer
            // 
            this.previewTimer.Interval = 3000;
            this.previewTimer.Tick += new System.EventHandler(this.PreviewTimer_Tick);
            // 
            // showWorldCityClockAtMainCheckbox
            // 
            this.showWorldCityClockAtMainCheckbox.AutoSize = true;
            this.showWorldCityClockAtMainCheckbox.Location = new System.Drawing.Point(197, 278);
            this.showWorldCityClockAtMainCheckbox.Name = "showWorldCityClockAtMainCheckbox";
            this.showWorldCityClockAtMainCheckbox.Size = new System.Drawing.Size(241, 17);
            this.showWorldCityClockAtMainCheckbox.TabIndex = 21;
            this.showWorldCityClockAtMainCheckbox.Text = "Show Time In World Cities At Main Screen";
            this.showWorldCityClockAtMainCheckbox.UseVisualStyleBackColor = true;
            // 
            // manualTimezoneCheckbox
            // 
            this.manualTimezoneCheckbox.AutoSize = true;
            this.manualTimezoneCheckbox.Location = new System.Drawing.Point(132, 103);
            this.manualTimezoneCheckbox.Name = "manualTimezoneCheckbox";
            this.manualTimezoneCheckbox.Size = new System.Drawing.Size(65, 17);
            this.manualTimezoneCheckbox.TabIndex = 22;
            this.manualTimezoneCheckbox.Text = "Manual";
            this.manualTimezoneCheckbox.UseVisualStyleBackColor = true;
            this.manualTimezoneCheckbox.CheckedChanged += new System.EventHandler(this.ManualTimezoneCheckbox_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 320);
            this.ControlBox = false;
            this.Controls.Add(this.manualTimezoneCheckbox);
            this.Controls.Add(this.showWorldCityClockAtMainCheckbox);
            this.Controls.Add(this.multiMonitorStatusResultLabel);
            this.Controls.Add(this.multiMonitorStatusLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.timeBoxScaleTrackBar);
            this.Controls.Add(this.sortByNameCheckbox);
            this.Controls.Add(this.sortByTimeCheckbox);
            this.Controls.Add(this.showSecondsCheckbox);
            this.Controls.Add(this.amPm12HoursIndicatorCheckbox);
            this.Controls.Add(this.persistentFrameImageBox);
            this.Controls.Add(this.timezonesCombo);
            this.Controls.Add(this.cityListview);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.cityTextbox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "FlipIt Settings";
            this.TopMost = true;
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SettingsForm_MouseClick);
            ((System.ComponentModel.ISupportInitialize)(this.persistentFrameImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeBoxScaleTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox cityTextbox;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.ListView cityListview;
        private System.Windows.Forms.ColumnHeader cityName;
        private System.Windows.Forms.ColumnHeader removeBtn;
        private System.Windows.Forms.ComboBox timezonesCombo;
        private System.Windows.Forms.ColumnHeader timezone;
        private System.Windows.Forms.PictureBox persistentFrameImageBox;
        private System.Windows.Forms.CheckBox amPm12HoursIndicatorCheckbox;
        private System.Windows.Forms.CheckBox showSecondsCheckbox;
        private System.Windows.Forms.RadioButton sortByTimeCheckbox;
        private System.Windows.Forms.RadioButton sortByNameCheckbox;
        private System.Windows.Forms.TrackBar timeBoxScaleTrackBar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label multiMonitorStatusLabel;
        private System.Windows.Forms.Label multiMonitorStatusResultLabel;
        private System.Windows.Forms.Timer previewTimer;
        private System.Windows.Forms.CheckBox showWorldCityClockAtMainCheckbox;
        private System.Windows.Forms.CheckBox manualTimezoneCheckbox;
    }
}
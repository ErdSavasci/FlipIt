/* Originally based on project by Frank McCown in 2010 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Xml;

namespace ScreenSaver
{
    public sealed partial class SettingsForm : Form
    {
        private readonly string userApplicationDataFolderPath;
        private readonly string configFilePath;
        private readonly ListViewColumnSorter listViewColumnSorter = new ListViewColumnSorter();

        private MainForm persistentFrame;
        private Dictionary<string, City> configCities = new Dictionary<string, City>();
        private int oldTimeBoxScaleTrackBarValue;
        private int tickValue;

        public SettingsForm()
        {
            InitializeComponent();

            DisableFocusAll();

            userApplicationDataFolderPath = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                userApplicationDataFolderPath = Directory.GetParent(userApplicationDataFolderPath).ToString();
            }

            configFilePath = userApplicationDataFolderPath + Path.DirectorySeparatorChar + "flipit_config.xml";

            LoadSettings();
        }

        /// <summary>
        /// Load display text from the Registry
        /// </summary>
        //        private void LoadSettings()
        //        {
        //            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Demo_ScreenSaver");
        //            if (key == null)
        //                textBox.Text = "C# Screen Saver";
        //            else
        //                textBox.Text = (string)key.GetValue("text");
        //        }

        /// <summary>
        /// Save text into the Registry.
        /// </summary>
        //        private void SaveSettings()
        //        {
        //            // Create or get existing subkey
        //            RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Demo_ScreenSaver");
        //
        //            key.SetValue("text", textBox.Text);
        //        }

        private void LoadSettings()
        {
            XmlDocument xmlDoc = new XmlDocument();

            if (!File.Exists(configFilePath))
            {
                using (FileStream fs = File.Create(configFilePath))
                {
                    XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
                    using (XmlWriter writer = XmlWriter.Create(fs, settings))
                    {                       
                        string xmlConf = "<config>";
                        xmlConf += "<cities>";

                        configCities = Utilities.GetDefaultCitiesWithIndexes();

                        if (configCities.Count > 0)
                        {
                            foreach (City city in configCities.Values)
                            {
                                xmlConf += "<city>";

                                xmlConf += "<name>";
                                xmlConf += city.DisplayName;
                                xmlConf += "</name>";

                                xmlConf += "<timezone>";
                                xmlConf += city.TimeZoneID;
                                xmlConf += "</timezone>";

                                xmlConf += "</city>";
                            }
                        }

                        xmlConf += "</cities>";
                        xmlConf += "<Show24Hours>";
                        xmlConf += "False";
                        xmlConf += "</Show24Hours>";
                        xmlConf += "<ShowSeconds>";
                        xmlConf += "False";
                        xmlConf += "</ShowSeconds>";
                        xmlConf += "<CitiesSortOrder>";
                        xmlConf += "Name";
                        xmlConf += "</CitiesSortOrder>";
                        xmlConf += "<TimeBoxScale>";
                        xmlConf += "2";
                        xmlConf += "</TimeBoxScale>";
                        xmlConf += "<MainScreen>";
                        xmlConf += "SingleClock";
                        xmlConf += "</MainScreen>";
                        xmlConf += "<!-- Replace <API_KEY> field with your API KEY -->";
                        xmlConf += "<LatLonAPI>";
                        xmlConf += HttpUtility.UrlEncode(APIs.defaultLatLonAPI);
                        xmlConf += "</LatLonAPI>";
                        xmlConf += "<!-- Replace <USERNAME> field with your USERNAME -->";
                        xmlConf += "<TimezoneAPI>";
                        xmlConf += HttpUtility.UrlEncode(APIs.defaultTimezoneAPI);
                        xmlConf += "</TimezoneAPI>";
                        xmlConf += "</config>";

                        using (Stream stream = xmlConf.ToStream())
                        {
                            using (XmlReader reader = XmlReader.Create(stream, new XmlReaderSettings()))
                            {
                                xmlDoc.Load(reader);
                                xmlDoc.Save(writer);
                            }
                        }

                        amPm12HoursIndicatorCheckbox.Checked = true;
                        showSecondsCheckbox.Checked = false;
                        sortByNameCheckbox.Checked = true;
                        sortByTimeCheckbox.Checked = false;
                        timeBoxScaleTrackBar.Value = 2;
                        showWorldCityClockAtMainCheckbox.Checked = false;
                        APIs.LatLonAPIUrl = APIs.defaultLatLonAPI;
                        APIs.TimezoneAPIUrl = APIs.defaultTimezoneAPI;
                    }
                }
            }
            else
            {
                using (StreamReader streamReader = new StreamReader(configFilePath, Encoding.UTF8))
                {
                    using (XmlReader reader = XmlReader.Create(streamReader, new XmlReaderSettings()))
                    {
                        xmlDoc.Load(reader);
                        XmlNodeList cityNodeList = xmlDoc.GetElementsByTagName("cities")[0].ChildNodes;

                        int index = 1;
                        foreach (XmlNode cityNode in cityNodeList)
                        {
                            configCities.Add(index.ToString(CultureInfo.InvariantCulture), new City(cityNode.FirstChild.NextSibling.InnerText, cityNode.FirstChild.InnerText));
                            index++;
                        }

                        amPm12HoursIndicatorCheckbox.Checked = xmlDoc.GetElementsByTagName("Show24Hours")[0].InnerText.ToUpperInvariant().Equals("FALSE");
                        showSecondsCheckbox.Checked = xmlDoc.GetElementsByTagName("ShowSeconds")[0].InnerText.ToUpperInvariant().Equals("TRUE");
                        sortByNameCheckbox.Checked = xmlDoc.GetElementsByTagName("CitiesSortOrder")[0].InnerText.ToUpperInvariant().Equals("NAME");
                        sortByTimeCheckbox.Checked = !sortByNameCheckbox.Checked;
                        try
                        {
                            timeBoxScaleTrackBar.Value = int.Parse(xmlDoc.GetElementsByTagName("TimeBoxScale")[0].InnerText, CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex) when (ex is FormatException || ex is OverflowException)
                        {
                            timeBoxScaleTrackBar.Value = 2;
                        }
                        showWorldCityClockAtMainCheckbox.Checked = xmlDoc.GetElementsByTagName("MainScreen")[0].InnerText.ToUpperInvariant().Equals("WORLDCITIES");
                        APIs.LatLonAPIUrl = HttpUtility.UrlDecode(xmlDoc.GetElementsByTagName("LatLonAPI")[0].InnerText);
                        APIs.TimezoneAPIUrl = HttpUtility.UrlDecode(xmlDoc.GetElementsByTagName("TimezoneAPI")[0].InnerText);
                    }
                }
            }

            if (configCities.Count > 0)
            {
                listViewColumnSorter.Order = SortOrder.Ascending;
                listViewColumnSorter.SortColumn = 0;
                cityListview.ListViewItemSorter = listViewColumnSorter;

                using (ListViewExtender extender = new ListViewExtender(cityListview))
                {
                    ListViewButtonColumn buttonAction = new ListViewButtonColumn(2);
                    buttonAction.Click += OnButtonActionClick;
                    buttonAction.FixedWidth = true;


                    extender.AddColumn(buttonAction);

                    foreach (City city in configCities.Values)
                    {
                        ListViewItem item = cityListview.Items.Add(city.DisplayName, city.DisplayName, 0);
                        item.SubItems.Add(city.TimeZoneID);
                        item.SubItems.Add("Remove");
                    }
                }
            }

            foreach (string timezone in Utilities.GetAllTimeZones())
            {
                timezonesCombo.Items.Add(timezone);
                timezonesCombo.SelectedIndex = 0;
            }
            timezonesCombo.Enabled = false;

            multiMonitorStatusResultLabel.Text = Screen.AllScreens.Length > 1 ? Properties.Resources.ResourceManager.GetString("OK", CultureInfo.InvariantCulture) : Properties.Resources.ResourceManager.GetString("NOT_FOUND", CultureInfo.InvariantCulture);
            multiMonitorStatusResultLabel.ForeColor = Screen.AllScreens.Length > 1 ? Color.Green : Color.Red;

            cityTextbox.Text = Properties.Resources.ResourceManager.GetString("PLACEHOLDER", CultureInfo.InvariantCulture);

            ShowOrUpdatePersistentFrame();
        }

        private void OnButtonActionClick(object sender, ListViewColumnMouseEventArgs e)
        { 
            configCities.Remove((cityListview.Items[e.Item.Text].Index + 1).ToString(CultureInfo.InvariantCulture));
            cityListview.Items.Remove(e.Item);
            cityListview.Sort();
        }

        private void SaveSettings()
        {
            XmlDocument xmlDoc = new XmlDocument();

            File.Delete(configFilePath);

            using (FileStream fs = File.Create(configFilePath))
            {
                XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
                using (XmlWriter writer = XmlWriter.Create(fs, settings))
                {
                    string xmlConf = "<config>";
                    xmlConf += "<cities>";

                    foreach (City configCity in configCities.Values)
                    {
                        xmlConf += "<city>";

                        xmlConf += "<name>";
                        xmlConf += configCity.DisplayName;
                        xmlConf += "</name>";

                        xmlConf += "<timezone>";
                        xmlConf += configCity.TimeZoneID;
                        xmlConf += "</timezone>";

                        xmlConf += "</city>";
                    }

                    xmlConf += "</cities>";
                    xmlConf += "<Show24Hours>";
                    xmlConf += amPm12HoursIndicatorCheckbox.Checked ? "False" : "True";
                    xmlConf += "</Show24Hours>";
                    xmlConf += "<ShowSeconds>";
                    xmlConf += showSecondsCheckbox.Checked ? "True" : "False";
                    xmlConf += "</ShowSeconds>";
                    xmlConf += "<CitiesSortOrder>";
                    xmlConf += sortByTimeCheckbox.Checked ? "Time" : "Name";
                    xmlConf += "</CitiesSortOrder>";
                    xmlConf += "<TimeBoxScale>";
                    xmlConf += timeBoxScaleTrackBar.Value.ToString(CultureInfo.InvariantCulture);
                    xmlConf += "</TimeBoxScale>";
                    xmlConf += "<MainScreen>";
                    xmlConf += showWorldCityClockAtMainCheckbox.Checked ? "WorldCities" : "SingleClock";
                    xmlConf += "</MainScreen>";
                    xmlConf += "<!-- Replace <API_KEY> field with your API KEY -->";
                    xmlConf += "<LatLonAPI>";
                    xmlConf += HttpUtility.UrlEncode(APIs.LatLonAPIUrl);
                    xmlConf += "</LatLonAPI>";
                    xmlConf += "<!-- Replace <USERNAME> field with your USERNAME -->";
                    xmlConf += "<TimezoneAPI>";
                    xmlConf += HttpUtility.UrlEncode(APIs.TimezoneAPIUrl);
                    xmlConf += "</TimezoneAPI>";
                    xmlConf += "</config>";

                    using (Stream stream = xmlConf.ToStream())
                    {
                        using (XmlReader reader = XmlReader.Create(stream, new XmlReaderSettings()))
                        {
                            xmlDoc.Load(reader);
                            xmlDoc.Save(writer);
                        }
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (cityListview.Items.ContainsKey(cityTextbox.Text))
            {
                MessageBox.Show("This city is already added.");
                return;
            }

            City city;
            if (manualTimezoneCheckbox.Checked)
            {
                city = new City(timezonesCombo.SelectedItem.ToString(), cityTextbox.Text);
            }
            else
            {
                city = new City(cityTextbox.Text);
            }

            ListViewItem item = cityListview.Items.Add(city.DisplayName, city.DisplayName, 0);
            item.SubItems.Add(city.TimeZoneID);
            item.SubItems.Add("Remove");

            configCities.Add((configCities.Count + 1).ToString(CultureInfo.InvariantCulture), city);

            List<KeyValuePair<string, City>> tempList = configCities.ToList();
            tempList.Sort((keyValuePair1, keyValuePair2) => string.Compare(keyValuePair1.Value.DisplayName, keyValuePair2.Value.DisplayName, true, CultureInfo.InvariantCulture));

            configCities.Clear();
            int index = 1;
            foreach (KeyValuePair<string, City> keyValuePair in tempList)
            {
                configCities.Add(index.ToString(CultureInfo.InvariantCulture), keyValuePair.Value);
                index++;
            }

            cityListview.Sort();
        }

        private void SortByNameCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton) sender).Checked)
            {
                sortByTimeCheckbox.Checked = false;
            }
            else
            {
                sortByTimeCheckbox.Checked = true;
            }
        }

        private void SortByTimeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                sortByNameCheckbox.Checked = false;
            }
            else
            {
                sortByNameCheckbox.Checked = true;
            }
        }

        private new void Close()
        {
            Dispose();
            base.Close();
        }

        private new void Dispose()
        {
            CustomDispose(true);
        }

        private void CustomDispose(bool disposing)
        {
            if (disposing)
            {
                if (persistentFrame != null)
                {
                    persistentFrame.Dispose();
                }
            }

            Dispose(true);
        }

        private void ShowOrUpdatePersistentFrame()
        {
            if (persistentFrame != null)
            {
                persistentFrame.Close();
            }

            // if (restartTimer && previewTimer.Enabled)
            // {
            //    previewTimer.Stop();
            //    tickValue = 0;
            // }

            if (tickValue % 2 == 0)
            {
                #pragma warning disable CA2000 // Dispose objects before losing scope
                persistentFrame = new MainForm(this, persistentFrameImageBox.Handle, false, true, new object[3] { (!amPm12HoursIndicatorCheckbox.Checked).ToString(CultureInfo.InvariantCulture), showSecondsCheckbox.Checked.ToString(CultureInfo.InvariantCulture), (timeBoxScaleTrackBar.Value + ((3 - timeBoxScaleTrackBar.Value) * (float)0.75)).ToString(CultureInfo.InvariantCulture) });
                persistentFrame.Show();
                persistentFrame.MouseClick += PersistentFrameImageBox_MouseClick;
                persistentFrame.Cursor = Cursors.Hand;
                #pragma warning restore CA2000 // Dispose objects before losing scope
            }
            else
            {
                #pragma warning disable CA2000 // Dispose objects before losing scope
                persistentFrame = new MainForm(this, persistentFrameImageBox.Handle, true, true, new object[3] { (!amPm12HoursIndicatorCheckbox.Checked).ToString(CultureInfo.InvariantCulture), showSecondsCheckbox.Checked.ToString(CultureInfo.InvariantCulture), (timeBoxScaleTrackBar.Value + ((3 - timeBoxScaleTrackBar.Value) * (float)0.75)).ToString(CultureInfo.InvariantCulture) });
                persistentFrame.Show();
                persistentFrame.MouseClick += PersistentFrameImageBox_MouseClick;
                persistentFrame.Cursor = Cursors.Hand;
                #pragma warning restore CA2000 // Dispose objects before losing scope
            }

            // if (!previewTimer.Enabled)
            // {
            //    previewTimer.Enabled = true;
            //    previewTimer.Start();
            // }
        }

        private void AmPm12HoursIndicatorCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ShowOrUpdatePersistentFrame();
        }

        private void ShowSecondsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox) sender).Checked)
            {
                oldTimeBoxScaleTrackBarValue = timeBoxScaleTrackBar.Value;
                timeBoxScaleTrackBar.Maximum = 2;
                timeBoxScaleTrackBar.Value = timeBoxScaleTrackBar.Maximum;
            }
            else
            {
                timeBoxScaleTrackBar.Maximum = 5;
                timeBoxScaleTrackBar.Value = oldTimeBoxScaleTrackBarValue;
            }

            ShowOrUpdatePersistentFrame();
        }

        private void TimeBoxScaleTrackBar_ValueChanged(object sender, EventArgs e)
        {
            ShowOrUpdatePersistentFrame();
        }

        private void PreviewTimer_Tick(object sender, EventArgs e)
        {
            tickValue = (tickValue + 1) % 2;

            ShowOrUpdatePersistentFrame();
        }

        private void ManualTimezoneCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                timezonesCombo.Enabled = true;
            }
            else
            {
                timezonesCombo.Enabled = false;
            }
        }

        private void CityTextbox_Enter(object sender, EventArgs e)
        {
            if (string.Equals(cityTextbox.Text, Properties.Resources.ResourceManager.GetString("PLACEHOLDER", CultureInfo.InvariantCulture), StringComparison.InvariantCulture))
            {
                cityTextbox.Text = "";
            }
        }

        private void CityTextbox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cityTextbox.Text))
            {
                cityTextbox.Text = Properties.Resources.ResourceManager.GetString("PLACEHOLDER", CultureInfo.InvariantCulture);
            }
        }

        private void PersistentFrameImageBox_MouseClick(object sender, MouseEventArgs e)
        {
            tickValue = (tickValue + 1) % 2;

            ShowOrUpdatePersistentFrame();
        }

        private void SettingsForm_MouseClick(object sender, MouseEventArgs e)
        {
            persistentFrameImageBox.Focus();
        }

        private void DisableFocusAll()
        {
            saveButton.TabStop = false;
            cancelButton.TabStop = false;
            cityTextbox.TabStop = false;
            timezonesCombo.TabStop = false;
            addButton.TabStop = false;
            cityListview.TabStop = false;
            amPm12HoursIndicatorCheckbox.TabStop = false;
            showSecondsCheckbox.TabStop = false;
            timeBoxScaleTrackBar.TabStop = false;
            sortByNameCheckbox.TabStop = false;
            sortByTimeCheckbox.TabStop = false;
        }
    }
}

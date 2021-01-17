/* Originally based on project by Frank McCown in 2010 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ScreenSaver
{
    public sealed partial class SettingsForm : Form
    {
        private readonly string userApplicationDataFolderPath;
        private readonly string configFilePath;
        private readonly ListViewColumnSorter listViewColumnSorter = new ListViewColumnSorter();
        private Dictionary<string, City> configCities = new Dictionary<string, City>();

        public SettingsForm()
        {
            InitializeComponent();

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
        }

        private void OnButtonActionClick(object sender, ListViewColumnMouseEventArgs e)
        {
            cityListview.Items.Remove(e.Item);
            configCities.Remove((e.Item.Index + 1).ToString(CultureInfo.InvariantCulture));
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

            City city = new City(timezonesCombo.SelectedItem.ToString(), cityTextbox.Text);

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
    }
}

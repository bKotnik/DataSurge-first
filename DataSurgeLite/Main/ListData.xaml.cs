﻿using DataSurgeLite.Classes;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace DataSurgeLite.Main
{
    /// <summary>
    /// Interaction logic for ListData.xaml
    /// </summary>
    public partial class ListData : Window
    {
        public ListData()
        {
            InitializeComponent();
            Utility.LDClass = new ObservableCollection<ListDataClass>();

            int i = 1;
            Stream stream = File.OpenRead(Environment.CurrentDirectory + "\\Data.xml");
            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DataClass>));

            try
            {
                Utility.ListData = (ObservableCollection<DataClass>)xs.Deserialize(stream);

                foreach (DataClass data in Utility.ListData)
                {
                    if (WhichButton.buttonContent == "EMAILS" || WhichButton.buttonContent == "E-NASLOVI")
                    {
                        Title = WhichButton.listLabel + "s";
                        label.Text = WhichButton.listLabel;

                        ListDataClass ldc = new ListDataClass();
                        ldc.Number = i;
                        ldc.Data = data.Email;
                        Utility.LDClass.Add(ldc);
                    }

                    else if (WhichButton.buttonContent == "USERNAMES" || WhichButton.buttonContent == "UPORABNIŠKA IMENA")
                    {
                        Title = WhichButton.listLabel + "s";
                        label.Text = WhichButton.listLabel;

                        ListDataClass ldc = new ListDataClass();
                        ldc.Number = i;
                        ldc.Data = data.Username;
                        Utility.LDClass.Add(ldc);
                    }

                    else if (WhichButton.buttonContent == "PASSWORDS" || WhichButton.buttonContent == "GESLA")
                    {
                        Title = WhichButton.listLabel + "s";
                        label.Text = WhichButton.listLabel;

                        ListDataClass ldc = new ListDataClass();
                        ldc.Number = i;
                        ldc.Data = data.Password;
                        Utility.LDClass.Add(ldc);
                    }

                    else if (WhichButton.buttonContent == "OTHER" || WhichButton.buttonContent == "DRUGO")
                    {
                        Title = WhichButton.listLabel;
                        label.Text = WhichButton.listLabel;

                        ListDataClass ldc = new ListDataClass();
                        ldc.Number = i;
                        ldc.Data = data.Other;
                        Utility.LDClass.Add(ldc);
                    }

                    else if (WhichButton.buttonContent == "NOTES" || WhichButton.buttonContent == "ZAPISKI")
                    {
                        Title = WhichButton.listLabel + "s";
                        label.Text = WhichButton.listLabel;

                        ListDataClass ldc = new ListDataClass();
                        ldc.Number = i;
                        ldc.Data = data.Note;
                        Utility.LDClass.Add(ldc);
                    }

                    i++;
                }
            }

            catch
            {
                if (stream.Length != 0)
                    _ = MessageBox.Show("Error occurred when trying to load data", "Error loading data", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            lvListData.ItemsSource = Utility.LDClass;
            stream.Close();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
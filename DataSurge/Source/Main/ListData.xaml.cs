using DataSurge.Classes;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace DataSurge.Main
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
            function();
        }

        private void function()
        {
            int i = 1;

            try
            {
                foreach (DataClass data in Utility.ListData)
                {
                    if (WhichButton.buttonContent == "EMAILS" || WhichButton.buttonContent == "E-NASLOVI")
                    {
                        Title = WhichButton.listLabel + "s";
                        label.Text = WhichButton.listLabel;

                        if(data.Email != "/")
                        {
                            ListDataClass ldc = new ListDataClass();
                            ldc.Number = i;
                            ldc.Data = data.Email;
                            Utility.LDClass.Add(ldc);
                        }
                    }

                    else if (WhichButton.buttonContent == "USERNAMES" || WhichButton.buttonContent == "UPORABNIŠKA IMENA")
                    {
                        Title = WhichButton.listLabel + "s";
                        label.Text = WhichButton.listLabel;

                        if(data.Username != "/")
                        {
                            ListDataClass ldc = new ListDataClass();
                            ldc.Number = i;
                            ldc.Data = data.Username;
                            Utility.LDClass.Add(ldc);
                        }
                    }

                    else if (WhichButton.buttonContent == "PASSWORDS" || WhichButton.buttonContent == "GESLA")
                    {
                        Title = WhichButton.listLabel + "s";
                        label.Text = WhichButton.listLabel;

                        ListDataClass ldc = new ListDataClass();

                        if (data.Password != "/")
                        {
                            ldc.Number = i;
                            ldc.Data = data.Password;
                            Utility.LDClass.Add(ldc);
                        }
                    }

                    else if (WhichButton.buttonContent == "OTHER" || WhichButton.buttonContent == "DRUGO")
                    {
                        Title = WhichButton.listLabel;
                        label.Text = WhichButton.listLabel;


                        if(data.Other != "/")
                        {
                            ListDataClass ldc = new ListDataClass();
                            ldc.Number = i;
                            ldc.Data = data.Other;
                            Utility.LDClass.Add(ldc);
                        }
                    }

                    else if (WhichButton.buttonContent == "NOTES" || WhichButton.buttonContent == "ZAPISKI")
                    {
                        Title = WhichButton.listLabel + "s";
                        label.Text = WhichButton.listLabel;

                        if(data.Note != "/")
                        {
                            ListDataClass ldc = new ListDataClass();
                            ldc.Number = i;
                            ldc.Data = data.Note;
                            Utility.LDClass.Add(ldc);
                        }
                    }

                    i++;
                }
            }

            catch
            {
                _ = MessageBox.Show("Error occurred when trying to load data", "Error loading data", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            lvListData.ItemsSource = Utility.LDClass;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

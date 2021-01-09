using DataSurge.Classes;
using System.Collections.ObjectModel;
using System.Windows;

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
                    if (Helper.buttonContent == "EMAILS" || Helper.buttonContent == "E-NASLOVI")
                    {
                        Title = Helper.listLabel + "s";
                        label.Text = Helper.listLabel;

                        if (data.Email != "/")
                        {
                            ListDataClass ldc = new ListDataClass();
                            ldc.Number = i;
                            ldc.Data = data.Email;
                            Utility.LDClass.Add(ldc);
                        }
                    }

                    else if (Helper.buttonContent == "USERNAMES" || Helper.buttonContent == "UPORABNIŠKA IMENA")
                    {
                        Title = Helper.listLabel + "s";
                        label.Text = Helper.listLabel;

                        if (data.Username != "/")
                        {
                            ListDataClass ldc = new ListDataClass();
                            ldc.Number = i;
                            ldc.Data = data.Username;
                            Utility.LDClass.Add(ldc);
                        }
                    }

                    else if (Helper.buttonContent == "PASSWORDS" || Helper.buttonContent == "GESLA")
                    {
                        Title = Helper.listLabel + "s";
                        label.Text = Helper.listLabel;

                        ListDataClass ldc = new ListDataClass();

                        if (data.Password != "/")
                        {
                            ldc.Number = i;
                            ldc.Data = data.Password;
                            Utility.LDClass.Add(ldc);
                        }
                    }

                    else if (Helper.buttonContent == "OTHER" || Helper.buttonContent == "DRUGO")
                    {
                        Title = Helper.listLabel;
                        label.Text = Helper.listLabel;


                        if (data.Other != "/")
                        {
                            ListDataClass ldc = new ListDataClass();
                            ldc.Number = i;
                            ldc.Data = data.Other;
                            Utility.LDClass.Add(ldc);
                        }
                    }

                    else if (Helper.buttonContent == "NOTES" || Helper.buttonContent == "ZAPISKI")
                    {
                        Title = Helper.listLabel + "s";
                        label.Text = Helper.listLabel;

                        if (data.Note != "/")
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

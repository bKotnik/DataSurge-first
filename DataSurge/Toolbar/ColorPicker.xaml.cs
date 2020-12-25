using System.Windows;
using System.Windows.Media;

namespace DataSurge.Toolbar
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : Window
    {
        public ColorPicker()
        {
            InitializeComponent();

            LoadColors();
        }

        private void LoadColors()
        {
            currentColorMain.Text += Properties.Settings.Default.MainColor;
            currentColorSecondary.Text += Properties.Settings.Default.SecondaryColor;
            currentColorText.Text += Properties.Settings.Default.TextColor;
            currentColorBg.Text += Properties.Settings.Default.BackgroundColor;
            currentColorTitle.Text += Properties.Settings.Default.TitleColor;
            currentColorExit.Text += Properties.Settings.Default.ExitColor;
            currentColorWarning.Text += Properties.Settings.Default.WarningColor;
        }

        private void ApplyMain(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            var cc = new ColorConverter();
            try
            {
                Color color = colorPicker.SelectedColor.Value;
                SolidColorBrush brush = new SolidColorBrush(color);

                string sBrush = brush.ToString(); // stringify so I can modify it to 50 opacity
                string removed = sBrush.Remove(1, 2);
                string final50 = removed.Insert(1, "50"); // color with 50 opacity
                string final35 = removed.Insert(1, "35"); // color with 35 opacity
                string final95 = removed.Insert(1, "95"); // color with 95 opacity

                Application.Current.Resources["MainColor"] = brush;
                Application.Current.Resources["MainColor50"] = (Brush)bc.ConvertFromString(final50);

                Application.Current.Resources["MainGradientColor"] = color;
                Application.Current.Resources["MainColor35"] = (Color)cc.ConvertFrom(final35);
                Application.Current.Resources["MainColor95"] = (Color)cc.ConvertFrom(final95);
                //save to setting
                Properties.Settings.Default.MainColor = colorPicker.SelectedColor.Value.ToString();
                Properties.Settings.Default.MainColor50 = final50;
                Properties.Settings.Default.MainColor35 = final35;
                Properties.Settings.Default.MainColor95 = final95;
                Properties.Settings.Default.Save();

                //change current color textBox
                currentColorMain.Text = "Current color: " + Properties.Settings.Default.MainColor;
            }

            catch
            {
                _ = MessageBox.Show("Please select a color", "No color selected", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplySecondary(object sender, RoutedEventArgs e)
        {
            try
            {
                Color color = colorPicker.SelectedColor.Value;
                SolidColorBrush brush = new SolidColorBrush(color);
                Application.Current.Resources["SecondaryColor"] = brush;
                Application.Current.Resources["SecondaryGradientColor"] = color;
                //save to setting
                Properties.Settings.Default.SecondaryColor = colorPicker.SelectedColor.Value.ToString();
                Properties.Settings.Default.Save();

                //change current color textBox
                currentColorSecondary.Text = "Current color: " + Properties.Settings.Default.SecondaryColor;
            }

            catch
            {
                _ = MessageBox.Show("Please select a color", "No color selected", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyText(object sender, RoutedEventArgs e)
        {
            try
            {
                Color color = colorPicker.SelectedColor.Value;
                SolidColorBrush brush = new SolidColorBrush(color);
                Application.Current.Resources["TextColor"] = brush;
                Application.Current.Resources["TextGradientColor"] = color;
                //save to setting
                Properties.Settings.Default.TextColor = colorPicker.SelectedColor.Value.ToString();
                Properties.Settings.Default.Save();

                //change current color textBox
                currentColorText.Text = "Current color: " + Properties.Settings.Default.TextColor;
            }

            catch
            {
                _ = MessageBox.Show("Please select a color", "No color selected", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyBackground(object sender, RoutedEventArgs e)
        {
            try
            {
                Color color = colorPicker.SelectedColor.Value;
                SolidColorBrush brush = new SolidColorBrush(color);
                Application.Current.Resources["BackgroundColor"] = brush;
                Application.Current.Resources["BackgroundGradientColor"] = color;
                //save to setting
                Properties.Settings.Default.BackgroundColor = colorPicker.SelectedColor.Value.ToString();
                Properties.Settings.Default.Save();

                //change current color textBox
                currentColorBg.Text = "Current color: " + Properties.Settings.Default.BackgroundColor;
            }

            catch
            {
                _ = MessageBox.Show("Please select a color", "No color selected", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyTitle(object sender, RoutedEventArgs e)
        {
            try
            {
                Color color = colorPicker.SelectedColor.Value;
                SolidColorBrush brush = new SolidColorBrush(color);
                Application.Current.Resources["TitleColor"] = brush;
                Application.Current.Resources["TitleGradientColor"] = color;
                //save to setting
                Properties.Settings.Default.TitleColor = colorPicker.SelectedColor.Value.ToString();
                Properties.Settings.Default.Save();

                //change current color textBox
                currentColorTitle.Text = "Current color: " + Properties.Settings.Default.TitleColor;
            }

            catch
            {
                _ = MessageBox.Show("Please select a color", "No color selected", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyExit(object sender, RoutedEventArgs e)
        {
            try
            {
                Color color = colorPicker.SelectedColor.Value;
                SolidColorBrush brush = new SolidColorBrush(color);
                Application.Current.Resources["ExitColor"] = brush;
                //save to setting
                Properties.Settings.Default.ExitColor = colorPicker.SelectedColor.Value.ToString();
                Properties.Settings.Default.Save();

                //change current color textBox
                currentColorExit.Text = "Current color: " + Properties.Settings.Default.ExitColor;
            }

            catch
            {
                _ = MessageBox.Show("Please select a color", "No color selected", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyWarning(object sender, RoutedEventArgs e)
        {
            try
            {
                Color color = colorPicker.SelectedColor.Value;
                SolidColorBrush brush = new SolidColorBrush(color);
                Application.Current.Resources["WarningColor"] = brush;
                //save to setting
                Properties.Settings.Default.WarningColor = colorPicker.SelectedColor.Value.ToString();
                Properties.Settings.Default.Save();

                //change current color textBox
                currentColorWarning.Text = "Current color: " + Properties.Settings.Default.WarningColor;
            }

            catch
            {
                _ = MessageBox.Show("Please select a color", "No color selected", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyDefault(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();
            var cc = new ColorConverter();
            /*MAIN COLORS*/
            Application.Current.Resources["MainColor"] = (Brush)bc.ConvertFrom("#FF059401");
            Application.Current.Resources["MainColor50"] = (Brush)bc.ConvertFrom("#50059401");
            Application.Current.Resources["MainColor35"] = (Color)cc.ConvertFrom("#35059401");
            Application.Current.Resources["MainColor95"] = (Color)cc.ConvertFrom("#95059401");
            Application.Current.Resources["MainGradientColor"] = (Color)cc.ConvertFrom("#059401");

            /*SECONDARY COLORS*/
            Application.Current.Resources["SecondaryColor"] = (Brush)bc.ConvertFrom("#FFFFFFFF");
            Application.Current.Resources["SecondaryGradientColor"] = (Color)cc.ConvertFrom("#FFFFFFFF");

            /*EXIT COLOR*/
            Application.Current.Resources["ExitColor"] = (Brush)bc.ConvertFrom("#810510");

            /*TITLE COLORS*/
            Application.Current.Resources["TitleColor"] = (Brush)bc.ConvertFrom("#4db5a7");
            Application.Current.Resources["TitleGradientColor"] = (Color)cc.ConvertFrom("#4db5a7");

            /*TEXT COLORS*/
            Application.Current.Resources["TextColor"] = (Brush)bc.ConvertFrom("#E8E8E8");
            Application.Current.Resources["TextGradientColor"] = (Color)cc.ConvertFrom("#E8E8E8");

            /*BACKGROUND COLORS*/
            Application.Current.Resources["BackgroundColor"] = (Brush)bc.ConvertFrom("#090c0f");
            Application.Current.Resources["BackgroundGradientColor"] = (Color)cc.ConvertFrom("#090c0f");

            /*WARNING COLOR*/
            Application.Current.Resources["WarningColor"] = (Brush)bc.ConvertFrom("#f9b302");

            /************* SAVE DEFAULTS TO SETTINGS *************/
            Properties.Settings.Default.MainColor = "#FF059401";
            Properties.Settings.Default.MainColor35 = "#42059401";
            Properties.Settings.Default.MainColor50 = "#50059401";
            Properties.Settings.Default.MainColor95 = "#95059401";

            Properties.Settings.Default.SecondaryColor = "#FFFFFFFF";
            Properties.Settings.Default.ExitColor = "#810510";
            Properties.Settings.Default.TitleColor = "#4db5a7";
            Properties.Settings.Default.TextColor = "#E8E8E8";
            Properties.Settings.Default.BackgroundColor = "#090c0f";
            Properties.Settings.Default.WarningColor = "#f9b302";

            Properties.Settings.Default.Save();

            //change current color textBoxes
            currentColorMain.Text = "Current color: " + Properties.Settings.Default.MainColor;
            currentColorSecondary.Text = "Current color: " + Properties.Settings.Default.SecondaryColor;
            currentColorText.Text = "Current color: " + Properties.Settings.Default.TextColor;
            currentColorBg.Text = "Current color: " + Properties.Settings.Default.BackgroundColor;
            currentColorTitle.Text = "Current color: " + Properties.Settings.Default.TitleColor;
            currentColorExit.Text = "Current color: " + Properties.Settings.Default.ExitColor;
            currentColorWarning.Text = "Current color: " + Properties.Settings.Default.WarningColor;
        }
    }
}

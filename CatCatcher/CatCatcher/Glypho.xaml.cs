using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CatCatcher
{
    /// <summary>
    /// Lógica de interacción para Glypho.xaml
    /// </summary>
    public partial class Glypho : Window
    {
        public Glypho()
        {
            System.Threading.Thread.Sleep(4444);//que buenos tiempos no? :3
            InitializeComponent();
            if (SelectedGlyph.glyph == SelectedGlyph.glyphs[0])
            {
                btnConfirm00.Content = "ACTIVO";
                btnConfirm00.IsEnabled = false;
            }
            if (SelectedGlyph.glyph == SelectedGlyph.glyphs[1])
            {
                btnConfirm01.Content = "ACTIVO";
                btnConfirm01.IsEnabled = false;
            }
            if (SelectedGlyph.glyph == SelectedGlyph.glyphs[2])
            {
                btnConfirm02.Content = "ACTIVO";
                btnConfirm02.IsEnabled = false;
            }
            if (SelectedGlyph.glyph == SelectedGlyph.glyphs[3])
            {
                btnConfirm03.Content = "ACTIVO";
                btnConfirm03.IsEnabled = false;
            }
        }




        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            SelectedGlyph.glyph = SelectedGlyph.glyphs[0];
            SetEmailFreq();

            MainWindow m = new MainWindow();
            m.Show();
            Close();
        }

        private void BtnConfirm_Click1(object sender, RoutedEventArgs e)
        {
            SelectedGlyph.glyph = SelectedGlyph.glyphs[1];
            SetEmailFreq();

            MainWindow m = new MainWindow();
            m.Show();
            Close();
        }
        private void BtnConfirm_Click2(object sender, RoutedEventArgs e)
        {
            SelectedGlyph.glyph = SelectedGlyph.glyphs[2];
            SetEmailFreq();

            MainWindow m = new MainWindow();
            m.Show();
            Close();
        }
        private void BtnConfirm_Click3(object sender, RoutedEventArgs e)
        {
            SelectedGlyph.glyph = SelectedGlyph.glyphs[3];
            SetEmailFreq();

            MainWindow m = new MainWindow();
            m.Show();
            Close();
        }

        private void SetEmailFreq()
        {
            Emails.email = txtEmail.Text;
            Emails.frequency = int.Parse(txtFreq.Text);
        }
    }
}

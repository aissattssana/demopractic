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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpfsport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public SportdbContext sport = new SportdbContext();
        private void entr_Click(object sender, RoutedEventArgs e)
        { 
            
             
                if (sport.Users.Any(i => i.UserLogin == login.Text && i.UserPassword == password.Text && i.RoleId == 1))
                { 
                        MessageBox.Show("Успешная авторизация клиента");
                    Tovars tov = new Tovars();
                    tov.Show();
                    this.Hide();
                    }
                    if (sport.Users.Any(i => i.UserLogin == login.Text && i.UserPassword == password.Text && i.RoleId == 2))
                    {
                        MessageBox.Show("Успешная авторизация администратора");
                    }

                    if (sport.Users.Any(i => i.UserLogin == login.Text && i.UserPassword == password.Text && i.RoleId == 3))
                    {
                        MessageBox.Show("Успешная авторизация менеджера");
                    }
            
        }

        private void tovars_Click(object sender, RoutedEventArgs e)
        {
            Tovars tov = new Tovars();
            tov.Show();
            this.Hide();
        }

        private void orders_Click(object sender, RoutedEventArgs e)
        {
            Orders tov = new Orders();
            tov.Show();
            this.Hide();
        }
    }
}

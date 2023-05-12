using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Threading;

namespace wpfsport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SportdbContext sport;
        private List<User> users;
        public MainWindow()
        {
            InitializeComponent();
            sport = new SportdbContext();
            users = new List<User>(sport.Users.ToList());
        }
        public void GetCaptcha()
        {
            String allowchar = "";

            allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z";
            allowchar += "1,2,3,4,5,6,7,8,9,0";

            char[] a = { ',' };
            String[] ar = allowchar.Split(a);
            String pwd = "";
            string temp = "";
            Random r = new Random();

            for (int i = 0; i < 6; i++)
            {
                temp = ar[r.Next(0, ar.Length)];
                pwd += temp;
            }

            Captcha_Text.Text = pwd;
        }
        private void UnblockUser(object sender, EventArgs e)
        {
            login.IsEnabled = true;
            password.IsEnabled = true;
            Captcha_User_Text.IsEnabled = true;
        }
        private void entr_Click(object sender, RoutedEventArgs e)
        {


            if (users.Any(u => u.UserLogin == login.Text && u.UserPassword == password.Text && u.RoleId == 1))
            {
                Hide();
                new Tovars(users.Find(u => u.UserLogin == login.Text && u.UserPassword == password.Text && u.RoleId == 1)).ShowDialog();
            }
            else if (users.Any(u => u.UserLogin == login.Text && u.UserPassword == password.Text && u.RoleId == 2))
            {
                Hide();
                new AdminWin(users.Find(u => u.UserLogin == login.Text && u.UserPassword == password.Text && u.RoleId == 2)).ShowDialog();
            }

            if (sport.Users.Any(i => i.UserLogin == login.Text && i.UserPassword == password.Text && i.RoleId == 3))
                    {
                        MessageBox.Show("Успешная авторизация менеджера");
                    }
            else
            {
                GetCaptcha();
                password.Text = "";
                MessageBox.Show("Вы заблокированы на 10 секунд!");
                login.IsEnabled = false;
                password.IsEnabled = false;
                Captcha_User_Text.IsEnabled = false;

                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += new EventHandler(UnblockUser);
                timer.Interval = new TimeSpan(0, 0, 10);
                timer.Start();
            }
        }

        private void tovars_Click(object sender, RoutedEventArgs e)
        {
            Tovars tov = new Tovars(null);
            tov.Show();
            this.Hide();
        }

    }
}

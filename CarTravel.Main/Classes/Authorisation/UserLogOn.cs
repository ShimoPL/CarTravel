using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CarTravel.Main.Authorisation
{
    class UserLogOn
    {
        public users ActualUser { get; private set; }
        public bool IsLogged { get; private set; }
        public UserLogOn()
        {
            int userId = 0;
            string login = "";
            do
            {
                var loginWin = new LoginWindow
                {
                    login = login
                };
                loginWin.ShowDialog();

                if (loginWin.DialogResult == true)
                {
                    login = loginWin.login;
                    userId = CheckUserCredentials(login, loginWin.pass);
                }

                if (userId != -1)
                {
                    ActualUser = GetUserInfo(userId);
                    if (ActualUser.role != "E")
                    {
                        var loginAgain = System.Windows.MessageBox.Show("You are not permited to use management system!\nDo You want login to another accout?", "User not permited", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Exclamation, System.Windows.MessageBoxResult.Yes);
                        if (loginAgain == System.Windows.MessageBoxResult.Yes) userId = -1;
                        else Application.Current.Shutdown();
                    }
                }
            } while (userId == -1);

        }

        private int CheckUserCredentials(string login, string pass)
        {
            using (var db = new CarTravelDb())
            {
                var user = db.systemusers.Where(l => l.login == login && l.password == pass).FirstOrDefault();
                return user != null ? user.userId : -1;
            }
        }

        private users GetUserInfo(int userId)
        {
            using (var db = new CarTravelDb())
            {
                return db.users.Where(u => u.userId == userId).FirstOrDefault();
            }
        }

    }
}

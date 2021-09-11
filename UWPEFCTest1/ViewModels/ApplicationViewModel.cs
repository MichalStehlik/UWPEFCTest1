using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UWPEFCTest1.Data;

namespace UWPEFCTest1.ViewModels
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private string _loginUsername;
        private string _loginPassword;
        private string _loginError;

        public RelayCommand LoginCommand { get; set; }

        public ApplicationViewModel()
        {
            LoginCommand = new RelayCommand(
                () =>
                {
                    var user = _db.Users.Where(u => u.Username == LoginUsername).SingleOrDefault();
                    string hashedPassword;
                    using (var algo = new MD5CryptoServiceProvider())
                    {
                        hashedPassword = algo.ComputeHash(Encoding.UTF8.GetBytes(LoginPassword)).ToString();
                    }
                    if (user != null)
                    {
                        if (user.HashedPassword != hashedPassword)
                        {
                            LoginError = "Nesprávné heslo";
                        }
                        else
                        {
                            Navigate();
                        }
                    }
                    else
                    {
                        LoginError = "Neznámé jméno uživatele";
                    }
                },
                () => { return (!String.IsNullOrEmpty(LoginUsername) && !String.IsNullOrEmpty(LoginPassword)); }
            );
        }

        public string LoginUsername { get { return _loginUsername; } set { _loginUsername = value; NotifyPropertyChanged(); LoginCommand.RaiseCanExecuteChanged(); } }
        public string LoginPassword { get { return _loginPassword; } set { _loginPassword = value; NotifyPropertyChanged(); LoginCommand.RaiseCanExecuteChanged(); } }
        public string LoginError { get { return _loginError; } set { _loginError = value; NotifyPropertyChanged(); } }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public Action Navigate;
    }
}

// UI/LoginForm.cs
using System;
using System.Windows.Forms;
using TekstilScada.Services;

namespace TekstilScada.UI
{
    public partial class LoginForm : Form
    {
        private readonly AuthService _authService;

        public LoginForm()
        {
            InitializeComponent();
            _authService = new AuthService();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                lblError.Text = "Kullanıcı adı ve şifre boş olamaz.";
                return;
            }

            bool success = _authService.Login(username, password);

            if (success)
            {
                // Giriş başarılı, bu formu kapat ve ana formu aç
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                lblError.Text = "Kullanıcı adı veya şifre hatalı!";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

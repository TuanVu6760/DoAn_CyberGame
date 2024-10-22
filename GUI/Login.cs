using BUS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            string passwordHash = HashPassword(password);

            var userService = new UserService();
            var user = userService.Login(username, passwordHash);

            if (user != null)
            {
                this.Hide();
                MainForm mainForm = new MainForm(user);
                mainForm.Show();
            }
            else
            {
                // Thay đổi màu của TextBox để chỉ báo lỗi
                txtUsername.BackColor = Color.LightCoral;
                txtPassword.BackColor = Color.LightCoral;
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi Đăng Nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private string HashPassword(string password)
        {
            // Bạn có thể dùng thư viện như BCrypt hoặc SHA256 để băm mật khẩu
            return password; // Placeholder
        }

        private void chkPass_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPass.Checked)
            {
              
                txtPassword.UseSystemPasswordChar = false; 
            }
            else
            {
          
                txtPassword.UseSystemPasswordChar = true; 
            }
        }
    }
}

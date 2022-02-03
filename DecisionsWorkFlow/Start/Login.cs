using ComponentFactory.Krypton.Toolkit;
using DecisionsWorkFlow.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DecisionsWorkFlow.Start
{
    public partial class Login : KryptonForm
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            string email = kryptonTextBox1.Text;

            DatabaseDataContext database = new DatabaseDataContext();
            users user = database.users.Where(u => u.email == email).FirstOrDefault();
            if (user != null)
            {
                if (kryptonTextBox2.Text == DesencriptaPassword(user.pass))
                {
                    var fname = user.fname;
                    var lname = user.lname;

                    MessageBox.Show("Bem-vindo " + fname + " " + lname);
                    Projects p = new Projects(user.id);
                    p.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Utilizador ou password errados.\nPor favor, tentar de novo.");
                }
            }
            else
            {
                MessageBox.Show("Utilizador não existente ou não encontrado.");
            }
        }

        /// <summary>
        /// Método de desencripta a password
        /// </summary>
        /// <param name="encPassWord"></param>
        /// <returns>A password em texto legível</returns>
        private string DesencriptaPassword(string encPassWord)
        {
            //chave de encriptação
            string EncryptionKey = "p@$sar1nh@";
            //retirar os caracteres problemáticos
            encPassWord = encPassWord.Replace(" ", "+");
            //criação da chave de desencriptação
            byte[] cipherBytes = Convert.FromBase64String(encPassWord);
            //desencriptar a palavra recolhida
            using (Aes encryptor = Aes.Create())
            {
                //criar a chave de derivação
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                //recriar a password em memória
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    encPassWord = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            //devolver a password em formato legível
            return encPassWord;
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            Register register = new Register(this);
            register.ShowDialog();
        }
    }
}

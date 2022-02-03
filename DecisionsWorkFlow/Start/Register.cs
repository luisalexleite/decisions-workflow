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
    public partial class Register : KryptonForm
    {
        private Login login;
        public Register(Login _login)
        {
            login = _login;
            InitializeComponent();
        }

        private void Registo_Load(object sender, EventArgs e)
        {

        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            if (VerificaCamposVazios())
            {
                //caso todos os campos estejam preenchidos, verificar na bdd se existem algum utilizador com o email indicado
                DatabaseDataContext dwf = new DatabaseDataContext();
                users user = dwf.users.Where(u => u.email == kryptonTextBox3.Text).FirstOrDefault();
                //se não houver, criar um novo
                if (user == null)
                {
                    users regUser = new users
                    {

                        email = kryptonTextBox3.Text,
                        fname = kryptonTextBox1.Text,
                        lname = kryptonTextBox2.Text,
                        pass = EncriptaPassword(kryptonTextBox4.Text),
                        terminated = false,
                        created_at = DateTime.Now
                    };
                    dwf.users.InsertOnSubmit(regUser);
                    dwf.SubmitChanges();
                    //limpar as caixas de texto e devolver o utilizador para a form de inicio de sessão
                    ClearFields();
                    MessageBox.Show("Utilizador registado.\nPor favor, faça login.");
                    login.kryptonTextBox1.Text = regUser.email;
                    login.kryptonTextBox2.Text = "";
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Já existe um utilizador com esse email.\nNão pode existir mais que um email por utilizador.");
                }
            }
        }

        private void ClearFields()
        {
            kryptonTextBox1.Text = "";
            kryptonTextBox2.Text = "";
            kryptonTextBox3.Text = "";
            kryptonTextBox4.Text = "";
            kryptonTextBox5.Text = "";
        }

        private string EncriptaPassword(string clearText)
        {
            //chave de encriptação, conveniente ser guardada num ficheiro de configuração
            string EncryptionKey = "p@$sar1nh@";
            //tranformação da password inserida num byte[]
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            //início da encripatção da password, com o método AES
            using (Aes encryptor = Aes.Create())
            {
                //derivação da chave de encriptação através da Encryptionkey
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                //escrita da password encriptada em memória
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        /// <summary>
        /// Verificação dos valores inseridos nas caixas de texto da password 
        /// </summary>
        /// <returns>False, caso as passwords não correspondam, True, caso sejam iguais</returns>
        private bool VerificaConfirmacaoPassword()
        {
            if (kryptonTextBox4.Text != kryptonTextBox5.Text)
            {
                MessageBox.Show("A password e a verificação não correspondem.\nAs passwords devem ser iguais.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Verificação da informação obrigatória inserida.
        /// </summary>
        /// <returns>False - se algum campo estiver vazio, Procede à verificação da password se estiver tudo preenchido</returns>
        private bool VerificaCamposVazios()
        {
            if (kryptonTextBox1.Text == "" || kryptonTextBox2.Text == "" || kryptonTextBox3.Text == "" || kryptonTextBox4.Text == "" || kryptonTextBox5.Text == "")
            {
                MessageBox.Show("Um ou mais campos vazios.\nPor favor, rectificar os dados inseridos.");
                return false;
            }
            return VerificaConfirmacaoPassword();
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            ClearFields();
            this.Close();
        }
    }
}

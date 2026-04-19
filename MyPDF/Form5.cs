using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MyPDF
{
    public partial class Form5 : Form
    {

        public string? Password { get; private set; }

        public Form5()
        {
            InitializeComponent();

            this.Width = 500;
            this.Height = 300;


        }

        // ==============================
        // OKボタンをクリックしたとき
        // ==============================
        private void btnOk_Click(object sender, EventArgs e)
        {

            Password = PasswordTxt.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        // ==============================
        // Cancelボタンをクリックしたとき
        // ==============================
        private void btnCancel_Click(object sender, EventArgs e)
        {

            Password = null;
            this.DialogResult = DialogResult.Cancel;
            this.Close();

        }
    }
}

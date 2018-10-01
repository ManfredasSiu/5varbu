﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualLibrary
{
    public partial class MainWindow : Form
    {
    

        private void ShowMyBooks(object sender, EventArgs e)
        {
            MessageBox.Show("pavyko");
        }

        private LogicController logicC;

        public MainWindow(LogicController logicC)
        {
            InitializeComponent();
            label2.Text = ("Hello, " + StaticData.CurrentUser.getuserName() + "!");


            this.logicC = logicC;
            FormClosed += OnCloseRequest;

        }

        private void OnCloseRequest(Object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
         

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

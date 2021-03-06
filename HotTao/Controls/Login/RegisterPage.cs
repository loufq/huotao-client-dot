﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HotCoreUtils.Helper;
using HotTaoCore.Logic;

namespace HotTao.Controls.Login
{
    public partial class RegisterPage : UserControl
    {
        NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private Main hotForm { get; set; }
        private LoginControl loginForm { get; set; }
        public RegisterPage(Main mainWin, LoginControl loginWin)
        {
            InitializeComponent();
            hotForm = mainWin;
            loginForm = loginWin;
        }



        private void lbLoginName_Click(object sender, EventArgs e)
        {
            this.loginName.Focus();
        }

        private void lbLoginPwd_Click(object sender, EventArgs e)
        {
            this.loginPwd.Focus();
        }
        private void loginName_KeyDown(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(loginName.Text))
                lbLoginName.Visible = false;
            else
                lbLoginName.Visible = true;
        }
 
        private void loginPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(loginPwd.Text))
                lbLoginPwd.Visible = false;
            else
                lbLoginPwd.Visible = true;
        }


        private bool _isLogining = false;
        public bool loginSuccess { get; set; }

        public bool isLogining
        {
            get
            {
                return _isLogining;
            }

            set
            {
                _isLogining = value;
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                if (isLogining)
                    return;
                if (string.IsNullOrEmpty(loginName.Text))
                {
                    lbTipMsg.Text = "请输入登录账户!";
                    loginName.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(loginPwd.Text))
                {
                    lbTipMsg.Text = "请输入登录密码!";
                    loginPwd.Focus();
                    return;
                }
                string lgname = loginName.Text;
                string pwd = EncryptHelper.MD5(loginPwd.Text);
                string verifyCode ="";
                Loading ld = new Loading();
                ((Action)(delegate ()
                {
                    var data = LogicUser.Instance.Register(lgname, pwd, verifyCode, (err) =>
                    {
                        if (err != null && err.resultCode != 200)
                        {
                            this.BeginInvoke((Action)(delegate ()  //等待结束
                            {
                                lbTipMsg.Text = err.resultMsg;
                            }));
                        }
                    });
                    ld.CloseForm();
                    if (data != null)
                    {
                        this.BeginInvoke((Action)(delegate ()  //等待结束
                        {
                            loginForm.openControl(new LoginPage(hotForm, loginForm));
                        }));
                    }
                })).BeginInvoke(null, null);
                ld.ShowDialog(this);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                this.BeginInvoke((Action)(delegate ()  //等待结束
                {
                    lbTipMsg.Text = "连接服务器失败!";
                }));
            }
        }
    }
}

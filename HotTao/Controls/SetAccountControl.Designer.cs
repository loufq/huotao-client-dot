﻿namespace HotTao.Controls
{
    partial class SetAccountControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.hotGroupBox2 = new HotTao.Controls.module.HotGroupBox(this.components);
            this.hotGroupBox3 = new HotTao.Controls.module.HotGroupBox(this.components);
            this.loginName = new System.Windows.Forms.TextBox();
            this.hotGroupBox4 = new HotTao.Controls.module.HotGroupBox(this.components);
            this.loginPwd = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ckbAutoLogin = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ckbSavePwd = new System.Windows.Forms.CheckBox();
            this.hotGroupBox2.SuspendLayout();
            this.hotGroupBox3.SuspendLayout();
            this.hotGroupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // hotGroupBox2
            // 
            this.hotGroupBox2.BorderColor = System.Drawing.Color.Gainsboro;
            this.hotGroupBox2.BorderTitleColor = System.Drawing.Color.Black;
            this.hotGroupBox2.Controls.Add(this.hotGroupBox3);
            this.hotGroupBox2.Controls.Add(this.hotGroupBox4);
            this.hotGroupBox2.Controls.Add(this.label8);
            this.hotGroupBox2.Controls.Add(this.label6);
            this.hotGroupBox2.Controls.Add(this.ckbAutoLogin);
            this.hotGroupBox2.Controls.Add(this.label7);
            this.hotGroupBox2.Controls.Add(this.ckbSavePwd);
            this.hotGroupBox2.Location = new System.Drawing.Point(10, 12);
            this.hotGroupBox2.Name = "hotGroupBox2";
            this.hotGroupBox2.Size = new System.Drawing.Size(730, 238);
            this.hotGroupBox2.TabIndex = 29;
            this.hotGroupBox2.TabStop = false;
            // 
            // hotGroupBox3
            // 
            this.hotGroupBox3.BackColor = System.Drawing.Color.White;
            this.hotGroupBox3.BorderColor = System.Drawing.Color.Silver;
            this.hotGroupBox3.BorderTitleColor = System.Drawing.Color.Black;
            this.hotGroupBox3.Controls.Add(this.loginName);
            this.hotGroupBox3.Location = new System.Drawing.Point(249, 22);
            this.hotGroupBox3.Name = "hotGroupBox3";
            this.hotGroupBox3.Size = new System.Drawing.Size(189, 35);
            this.hotGroupBox3.TabIndex = 1;
            this.hotGroupBox3.TabStop = false;
            // 
            // loginName
            // 
            this.loginName.BackColor = System.Drawing.Color.White;
            this.loginName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.loginName.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.loginName.Location = new System.Drawing.Point(4, 14);
            this.loginName.Margin = new System.Windows.Forms.Padding(10);
            this.loginName.Name = "loginName";
            this.loginName.Size = new System.Drawing.Size(176, 16);
            this.loginName.TabIndex = 0;
            // 
            // hotGroupBox4
            // 
            this.hotGroupBox4.BackColor = System.Drawing.Color.White;
            this.hotGroupBox4.BorderColor = System.Drawing.Color.Silver;
            this.hotGroupBox4.BorderTitleColor = System.Drawing.Color.Black;
            this.hotGroupBox4.Controls.Add(this.loginPwd);
            this.hotGroupBox4.Location = new System.Drawing.Point(249, 68);
            this.hotGroupBox4.Name = "hotGroupBox4";
            this.hotGroupBox4.Size = new System.Drawing.Size(189, 35);
            this.hotGroupBox4.TabIndex = 2;
            this.hotGroupBox4.TabStop = false;
            // 
            // loginPwd
            // 
            this.loginPwd.BackColor = System.Drawing.Color.White;
            this.loginPwd.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.loginPwd.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.loginPwd.Location = new System.Drawing.Point(4, 14);
            this.loginPwd.Margin = new System.Windows.Forms.Padding(10);
            this.loginPwd.Name = "loginPwd";
            this.loginPwd.Size = new System.Drawing.Size(176, 16);
            this.loginPwd.TabIndex = 1;
            this.loginPwd.UseSystemPasswordChar = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(158)))));
            this.label8.Location = new System.Drawing.Point(332, -4);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(5);
            this.label8.Size = new System.Drawing.Size(67, 22);
            this.label8.TabIndex = 14;
            this.label8.Text = "账户设置";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(176, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "软件账户";
            // 
            // ckbAutoLogin
            // 
            this.ckbAutoLogin.AutoSize = true;
            this.ckbAutoLogin.Location = new System.Drawing.Point(366, 120);
            this.ckbAutoLogin.Name = "ckbAutoLogin";
            this.ckbAutoLogin.Size = new System.Drawing.Size(72, 16);
            this.ckbAutoLogin.TabIndex = 13;
            this.ckbAutoLogin.Text = "自动登录";
            this.ckbAutoLogin.UseVisualStyleBackColor = true;
            this.ckbAutoLogin.Visible = false;
            this.ckbAutoLogin.CheckedChanged += new System.EventHandler(this.ckbAutoLogin_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(200, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 7;
            this.label7.Text = "密码";
            // 
            // ckbSavePwd
            // 
            this.ckbSavePwd.AutoSize = true;
            this.ckbSavePwd.Location = new System.Drawing.Point(261, 120);
            this.ckbSavePwd.Name = "ckbSavePwd";
            this.ckbSavePwd.Size = new System.Drawing.Size(72, 16);
            this.ckbSavePwd.TabIndex = 11;
            this.ckbSavePwd.Text = "记住密码";
            this.ckbSavePwd.UseVisualStyleBackColor = true;
            // 
            // SetAccountControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.hotGroupBox2);
            this.Name = "SetAccountControl";
            this.Size = new System.Drawing.Size(750, 397);
            this.Load += new System.EventHandler(this.SetAccountControl_Load);
            this.hotGroupBox2.ResumeLayout(false);
            this.hotGroupBox2.PerformLayout();
            this.hotGroupBox3.ResumeLayout(false);
            this.hotGroupBox3.PerformLayout();
            this.hotGroupBox4.ResumeLayout(false);
            this.hotGroupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox ckbAutoLogin;
        private System.Windows.Forms.CheckBox ckbSavePwd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private module.HotGroupBox hotGroupBox2;
        private module.HotGroupBox hotGroupBox3;
        private System.Windows.Forms.TextBox loginName;
        private module.HotGroupBox hotGroupBox4;
        private System.Windows.Forms.TextBox loginPwd;
    }
}

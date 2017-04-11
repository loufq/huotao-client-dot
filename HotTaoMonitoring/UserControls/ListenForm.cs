﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WwChatHttpCore.Objects;

namespace HotTaoMonitoring.UserControls
{
    public partial class ListenForm : UserControl
    {
        private MainForm mainForm { get; set; }

        /// <summary>
        /// 微信监控的消息数据
        /// </summary>
        public List<WxMessageBodyModel> wxMessageData { get; set; }
        /// <summary>
        /// 是否显示全部
        /// </summary>
        public bool isShowAll { get; set; }

        /// <summary>
        /// 当前选中的微信群
        /// </summary>
        public string CurrentSelectedWeChat { get; set; }


        public ListenForm(MainForm form)
        {
            InitializeComponent();
            mainForm = form;
            wxMessageData = new List<WxMessageBodyModel>();
        }

        private void ListenForm_Load(object sender, EventArgs e)
        {
            SetContactsView(mainForm.contact_all);
        }



        /// <summary>
        /// 设置微信监控消息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="msgSendUser"></param>
        /// <param name="nickName"></param>
        /// <param name="content"></param>
        public void SetWxMessageData(WXUser user, string msgSendUser, string nickName, string content)
        {
            WxMessageBodyModel msg = new WxMessageBodyModel();
            if (!wxMessageData.Exists(item => { return item.MsgSendUser == msgSendUser && item.MsgUserName == user.UserName; }))
            {
                msg = new WxMessageBodyModel()
                {
                    MsgSendUser = msgSendUser,
                    MsgShowName = user.ShowName,
                    MsgText = content,
                    MsgTime = DateTime.Now.ToString("dd日 HH:mm"),
                    NotReadCount = 1,
                    MsgUserName = user.UserName,
                    MsgNickName = nickName,
                    MsgStatus = "未回复",
                };
                wxMessageData.Add(msg);
            }
            else
            {

                wxMessageData.ForEach(data =>
                {
                    if (data.MsgSendUser == msgSendUser && data.MsgUserName == user.UserName)
                    {
                        data.MsgTime = DateTime.Now.ToString("dd日 HH:mm");
                        data.MsgText = content;
                        data.NotReadCount += 1;
                        data.MsgStatus = "未回复";
                    }
                });

                msg = wxMessageData.Find(m =>
                {
                    return m.MsgSendUser == msgSendUser && m.MsgUserName == user.UserName;
                });
            }
            if (msg != null)
                SetDataContentView(msg);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void SetDataContentView(WxMessageBodyModel data)
        {
            if (dataContent.InvokeRequired)
            {
                this.dataContent.Invoke(new Action<WxMessageBodyModel>(SetDataContentView), new object[] { data });
            }
            else
            {
                EditForm ed = new EditForm(mainForm, this);
                string html = ed.GetReceiveMsgHtml(data.MsgShowName, data.MsgNickName, data.MsgText,data.MsgTime);
                ed.writeCacheData(data.MsgShowName, data.MsgNickName, html);
                if (string.IsNullOrEmpty(CurrentSelectedWeChat) || data.MsgUserName.Equals(CurrentSelectedWeChat))
                {
                    bool result = false;
                    //合并相同用户消息
                    foreach (DataGridViewRow item in dataContent.Rows)
                    {
                        if (item.Cells["MsgSendUser"].Value.ToString().Equals(data.MsgSendUser)&& item.Cells["MsgUserName"].Value.ToString().Equals(data.MsgUserName))
                        {
                            result = true;
                            item.Cells["MsgContent"].Value = data.MsgText.Replace("<br/>", "\r\n");//data.MsgTime + " [" + data.MsgNickName + "]" + data.MsgShowName + ":" + 
                            item.Cells["MsgText"].Value = data.MsgText;
                            item.Cells["MsgTime"].Value = data.MsgTime;
                            if (mainForm.editForm != null && mainForm.editForm.toUserName == data.MsgUserName && mainForm.editForm.toNickName == data.MsgNickName && !mainForm.editForm.isHide)
                            {
                                item.Cells["NotReadCount"].Value = "0";
                                wxMessageData.ForEach(d =>
                                {
                                    if (d.MsgSendUser == data.MsgSendUser && d.MsgUserName == data.MsgUserName)
                                    {
                                        data.NotReadCount = 0;
                                    }
                                });
                            }
                            else
                            {
                                item.Cells["NotReadCount"].Value = data.NotReadCount;
                                if (data.NotReadCount > 0)
                                {
                                    item.Cells["NotReadCount"].Style.ForeColor = Color.Red;
                                }
                            }
                            item.Cells["MsgStatus"].Value = data.MsgStatus;
                            break;
                        }
                    }
                    if (!result)
                    {

                        int i = dataContent.Rows.Count;
                        dataContent.Rows.Add();
                        ++i;
                        dataContent.Rows[i - 1].Cells["MsgContent"].Value = data.MsgText.Replace("<br/>", "\r\n");
                        dataContent.Rows[i - 1].Cells["MsgUserName"].Value = data.MsgUserName;
                        dataContent.Rows[i - 1].Cells["MsgNickName"].Value = data.MsgNickName;
                        dataContent.Rows[i - 1].Cells["MsgText"].Value = data.MsgText;
                        dataContent.Rows[i - 1].Cells["MsgSendUser"].Value = data.MsgSendUser;
                        dataContent.Rows[i - 1].Cells["MsgShowName"].Value = data.MsgShowName;
                        dataContent.Rows[i - 1].Cells["NotReadCount"].Value = data.NotReadCount;
                        dataContent.Rows[i - 1].Cells["MsgStatus"].Value = data.MsgStatus;
                        dataContent.Rows[i - 1].Cells["MsgTime"].Value = data.MsgTime;
                        if (data.NotReadCount > 0)
                            dataContent.Rows[i - 1].Cells["NotReadCount"].Style.ForeColor = Color.Red;
                        else
                            dataContent.Rows[i - 1].Cells["NotReadCount"].Style.ForeColor = ConstConfig.DataGridViewRowForeColor;

                        if (i % 2 == 0)
                        {
                            dataContent.Rows[i - 1].DefaultCellStyle.BackColor = ConstConfig.DataGridViewEvenRowBackColor;
                            dataContent.Rows[i - 1].DefaultCellStyle.SelectionBackColor = ConstConfig.DataGridViewEvenRowBackColor;
                        }
                        else
                        {
                            dataContent.Rows[i - 1].DefaultCellStyle.BackColor = ConstConfig.DataGridViewOddRowBackColor;
                            dataContent.Rows[i - 1].DefaultCellStyle.SelectionBackColor = ConstConfig.DataGridViewOddRowBackColor;
                        }
                        dataContent.Rows[i - 1].Height = ConstConfig.DataGridViewRowHeight;
                        dataContent.Rows[i - 1].DefaultCellStyle.ForeColor = ConstConfig.DataGridViewRowForeColor;
                    }
                }
                if (mainForm.editForm != null && mainForm.editForm.toUserName == data.MsgUserName && mainForm.editForm.toNickName == data.MsgNickName)
                {
                    mainForm.editForm.ShowReceiveMsg(data.MsgNickName, data.MsgText,data.MsgTime);
                }

            }
        }


        private void SetDataContentView(List<WxMessageBodyModel> datas)
        {
            if (dataContent.InvokeRequired)
            {
                this.dataContent.Invoke(new Action<List<WXUser>>(SetContactsView), new object[] { datas });
            }
            else
            {
                this.dataContent.Rows.Clear();
                int i = dataContent.Rows.Count;

                foreach (var data in datas)
                {
                    dataContent.Rows.Add();
                    ++i;
                    dataContent.Rows[i - 1].Cells["MsgContent"].Value = data.MsgText.Replace("<br/>", "\r\n");//data.MsgTime + " [" + data.MsgNickName + "]" + data.MsgShowName + ":" + 
                    dataContent.Rows[i - 1].Cells["MsgUserName"].Value = data.MsgUserName;
                    dataContent.Rows[i - 1].Cells["MsgNickName"].Value = data.MsgNickName;
                    dataContent.Rows[i - 1].Cells["MsgText"].Value = data.MsgText;
                    dataContent.Rows[i - 1].Cells["MsgSendUser"].Value = data.MsgSendUser;
                    dataContent.Rows[i - 1].Cells["MsgShowName"].Value = data.MsgShowName;
                    dataContent.Rows[i - 1].Cells["NotReadCount"].Value = data.NotReadCount;
                    dataContent.Rows[i - 1].Cells["MsgStatus"].Value = data.MsgStatus;
                    dataContent.Rows[i - 1].Cells["MsgTime"].Value = data.MsgTime;
                    if (data.NotReadCount > 0)
                        dataContent.Rows[i - 1].Cells["NotReadCount"].Style.ForeColor = Color.Red;
                    else
                        dataContent.Rows[i - 1].Cells["NotReadCount"].Style.ForeColor = ConstConfig.DataGridViewRowForeColor;
                    if (i % 2 == 0)
                    {
                        dataContent.Rows[i - 1].DefaultCellStyle.BackColor = ConstConfig.DataGridViewEvenRowBackColor;
                        dataContent.Rows[i - 1].DefaultCellStyle.SelectionBackColor = ConstConfig.DataGridViewEvenRowBackColor;
                    }
                    else
                    {
                        dataContent.Rows[i - 1].DefaultCellStyle.BackColor = ConstConfig.DataGridViewOddRowBackColor;
                        dataContent.Rows[i - 1].DefaultCellStyle.SelectionBackColor = ConstConfig.DataGridViewOddRowBackColor;
                    }
                    dataContent.Rows[i - 1].Height = ConstConfig.DataGridViewRowHeight;
                    dataContent.Rows[i - 1].DefaultCellStyle.ForeColor = ConstConfig.DataGridViewRowForeColor;
                }
            }
        }

        public void SetDataContentStatus(int rowIndex, string msgSendUser, string userName)
        {
            dataContent.Rows[rowIndex].Cells["MsgStatus"].Value = "已回复";
            wxMessageData.ForEach(data =>
            {
                if (data.MsgSendUser == msgSendUser && data.MsgUserName == userName)
                {
                    data.MsgStatus = "已回复";
                }
            });
        }


        /// <summary>
        /// 添加通讯录
        /// </summary>
        /// <param name="user">The user.</param>
        public void AddContactsView(WXUser user)
        {
            if (NotListenWeChatData == null) NotListenWeChatData = new List<WXUser>();
            if (ListenWeChatData == null) ListenWeChatData = new List<WXUser>();

            if (!NotListenWeChatData.Exists(item => { return item.UserName == user.UserName; }) && !ListenWeChatData.Exists(item => { return item.UserName == user.UserName; }))
            {
                NotListenWeChatData.Add(user);
                SetContactsView(user);
            }
            ListenWeChatData.ForEach(item =>
            {
                if (item.UserName == user.UserName)
                {
                    item.NickName = user.ShowName;
                }
            });
           
        }







        /// <summary>
        /// 加载微信通讯录
        /// </summary>
        /// <param name="user">The user.</param>
        private void SetContactsView(WXUser user)
        {
            if (NotListenWeChatData == null) NotListenWeChatData = new List<WXUser>();

            if (dgvWeChatList.InvokeRequired)
            {
                this.dgvWeChatList.Invoke(new Action<WXUser>(SetContactsView), new object[] { user });
            }
            else
            {
                bool result = false;
                foreach (DataGridViewRow item in dgvWeChatList.Rows)
                {
                    if (item.Cells["UserName"].Value.ToString().Equals(user.UserName))
                    {
                        result = true;
                        item.Cells["ShowName"].Value = user.ShowName;
                        item.Cells["UserName"].Value = user.UserName;
                        break;
                    }
                }
                if (!IsListenView && !result)
                {
                    int i = dgvWeChatList.Rows.Count;
                    dgvWeChatList.Rows.Add();
                    ++i;
                    dgvWeChatList.Rows[i - 1].Cells["ID"].Value = i;
                    dgvWeChatList.Rows[i - 1].Cells["ShowName"].Value = user.ShowName;
                    dgvWeChatList.Rows[i - 1].Cells["UserName"].Value = user.UserName;
                    if (i % 2 == 0)
                    {
                        dgvWeChatList.Rows[i - 1].DefaultCellStyle.BackColor = ConstConfig.DataGridViewEvenRowBackColor;
                        dgvWeChatList.Rows[i - 1].DefaultCellStyle.SelectionBackColor = ConstConfig.DataGridViewEvenRowBackColor;
                    }
                    else
                    {
                        dgvWeChatList.Rows[i - 1].DefaultCellStyle.BackColor = ConstConfig.DataGridViewOddRowBackColor;
                        dgvWeChatList.Rows[i - 1].DefaultCellStyle.SelectionBackColor = ConstConfig.DataGridViewOddRowBackColor;
                    }
                    dgvWeChatList.Rows[i - 1].Height = ConstConfig.DataGridViewRowHeight;
                    dgvWeChatList.Rows[i - 1].DefaultCellStyle.ForeColor = ConstConfig.DataGridViewRowForeColor;
                }

                if (NotListenWeChatData.Exists(item => { return item.UserName == user.UserName; }))
                    NotListenWeChatData.RemoveAll(u => { return u.UserName == user.UserName; });
                NotListenWeChatData.Add(user);
            }
        }
        /// <summary>
        /// 加载微信通讯录
        /// </summary>
        /// <param name="contact_all">The contact_all.</param>
        public void SetContactsView(List<WXUser> contact_all)
        {
            if (dgvWeChatList.InvokeRequired)
            {
                this.Invoke(new Action<List<WXUser>>(SetContactsView), new object[] { contact_all });
            }
            else
            {
                this.dgvWeChatList.Rows.Clear();
                int i = dgvWeChatList.Rows.Count;

                foreach (var user in contact_all)
                {

                    dgvWeChatList.Rows.Add();
                    ++i;
                    dgvWeChatList.Rows[i - 1].Cells["ID"].Value = i;
                    dgvWeChatList.Rows[i - 1].Cells["ShowName"].Value = user.ShowName;
                    dgvWeChatList.Rows[i - 1].Cells["UserName"].Value = user.UserName;
                    if (i % 2 == 0)
                    {
                        dgvWeChatList.Rows[i - 1].DefaultCellStyle.BackColor = ConstConfig.DataGridViewEvenRowBackColor;
                        dgvWeChatList.Rows[i - 1].DefaultCellStyle.SelectionBackColor = ConstConfig.DataGridViewEvenRowBackColor;
                    }
                    else
                    {
                        dgvWeChatList.Rows[i - 1].DefaultCellStyle.BackColor = ConstConfig.DataGridViewOddRowBackColor;
                        dgvWeChatList.Rows[i - 1].DefaultCellStyle.SelectionBackColor = ConstConfig.DataGridViewOddRowBackColor;
                    }
                    dgvWeChatList.Rows[i - 1].Height = ConstConfig.DataGridViewRowHeight;
                    dgvWeChatList.Rows[i - 1].DefaultCellStyle.ForeColor = ConstConfig.DataGridViewRowForeColor;
                    dgvWeChatList.Rows[i - 1].DefaultCellStyle.NullValue = IsListenView ? "移除" : "监控";
                }
            }
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void toolsRefresh_Click(object sender, EventArgs e)
        {
            if (!IsListenView)
            {
                if (mainForm.wxlogin != null)
                {
                    if (NotListenWeChatData == null) NotListenWeChatData = new List<WXUser>();
                    this.dgvWeChatList.Rows.Clear();
                    NotListenWeChatData.Clear();
                    mainForm.wxlogin.ReloadContact();
                }
            }
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var data = !IsListenView ? NotListenWeChatData.FindAll(item => { return item.ShowName.Contains(txtSearch.Text); }) : ListenWeChatData.FindAll(item => { return item.ShowName.Contains(txtSearch.Text); });
            SetContactsView(data);
        }

        /// <summary>
        /// 点击回复按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataContent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCellCollection cells = this.dataContent.CurrentRow.Cells;
            if (cells != null)
            {

                string _msgUserName = cells["MsgUserName"].Value.ToString();
                string _msgShowName = cells["MsgShowName"].Value.ToString();
                string _msgNickName = cells["MsgNickName"].Value.ToString();
                string _msgSendUser = cells["MsgSendUser"].Value.ToString();
                int rowIndex = cells[0].RowIndex;
                if (mainForm.editForm == null)
                {
                    mainForm.editForm = new EditForm(mainForm, this);
                    mainForm.editForm.toUserName = _msgUserName;
                    mainForm.editForm.toShowName = _msgShowName;
                    mainForm.editForm.toNickName = _msgNickName;
                    mainForm.editForm.StartPosition = FormStartPosition.CenterScreen;
                    mainForm.editForm.MsgSendUser = _msgSendUser;
                    mainForm.editForm.StartPosition = FormStartPosition.Manual;
                    RECT rect = new RECT();
                    WinApi.GetWindowRect(mainForm.Handle, ref rect);
                    mainForm.editForm.Location = new Point(rect.Right, rect.Top);
                    mainForm.editForm.Show();
                    mainForm.editForm.LoadHtml();

                }
                else if (mainForm.editForm.toUserName != _msgUserName || mainForm.editForm.toNickName != _msgNickName)
                {
                    mainForm.editForm.toShowName = _msgShowName;
                    mainForm.editForm.toUserName = _msgUserName;
                    mainForm.editForm.toNickName = _msgNickName;
                    mainForm.editForm.MsgSendUser = _msgSendUser;
                    mainForm.editForm.SetTitle(mainForm.editForm.toNickName);
                    mainForm.editForm.LoadHtml();
                    mainForm.editForm.Show();
                }
                else
                {
                    mainForm.editForm.LoadHtml();
                    mainForm.editForm.Show();
                }

                mainForm.editForm.isHide = false;
                mainForm.editForm.rowIndex = rowIndex;
                                
                WinApi.ShowWindow(mainForm.editForm.Handle, WinApi.NCmdShowFlag.SW_HIDE);
                //显示窗口
                WinApi.ShowWindow(mainForm.editForm.Handle, WinApi.NCmdShowFlag.SW_SHOWNORMAL);
                cells["NotReadCount"].Value = 0;
                cells["NotReadCount"].Style.ForeColor = ConstConfig.DataGridViewRowForeColor;
                wxMessageData.ForEach(item =>
                {
                    if (item.MsgUserName == _msgUserName && item.MsgNickName == _msgNickName)
                    {
                        item.NotReadCount = 0;
                    }
                });
            }
        }

        /// <summary>
        /// 添加/移除监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvWeChatList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ListenWeChatData == null)
                ListenWeChatData = new List<WXUser>();
            CurrentSelectedWeChat = string.Empty;
            DataGridViewCellCollection cells = this.dgvWeChatList.CurrentRow.Cells;
            if (cells != null && cells["editListen"].ColumnIndex == e.ColumnIndex)
            {
                string userName = cells["UserName"].Value.ToString();
                if (!IsListenView)
                {
                    if (!ListenWeChatData.Exists(item => { return item.UserName == userName; }))
                    {
                        //将群添加到监控列表
                        ListenWeChatData.Add(new WXUser()
                        {
                            UserName = userName,
                            NickName = cells["ShowName"].Value.ToString()
                        });

                        NotListenWeChatData.RemoveAll(item => { return item.UserName == userName; });
                    }
                }
                else
                {
                    //从监控列表中移除
                    if (ListenWeChatData.Exists(item => { return item.UserName == userName; }))
                    {
                        ListenWeChatData.RemoveAll(item => { return item.UserName == userName; });
                        //将群添加到监控列表
                        NotListenWeChatData.Add(new WXUser()
                        {
                            UserName = userName,
                            NickName = cells["ShowName"].Value.ToString()
                        });
                    }
                }
                dgvWeChatList.Rows.RemoveAt(cells["editListen"].RowIndex);
            }
            else
            {
                if (IsListenView)
                {
                    CurrentSelectedWeChat = cells["UserName"].Value.ToString();
                    var data = wxMessageData.FindAll(item =>
                    {
                        return item.MsgUserName == CurrentSelectedWeChat;
                    });
                    SetDataContentView(data);
                }
            }
        }

        /// <summary>
        /// 已监控的微信群
        /// </summary>
        public List<WXUser> ListenWeChatData { get; set; }

        /// <summary>
        /// 未监控的微信群
        /// </summary>
        public List<WXUser> NotListenWeChatData { get; set; }

        /// <summary>
        /// 是否在监控页面
        /// </summary>
        public bool IsListenView { get; set; }

        /// <summary>
        /// 微信群列表tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbTabWeChat_Click(object sender, EventArgs e)
        {
            CurrentSelectedWeChat = string.Empty;
            dgvWeChatList.ContextMenuStrip = menuStrip;
            lbTabWeChat.BackColor = Color.White;
            lbTabWeChatListen.BackColor = Color.Silver;
            if (NotListenWeChatData == null) NotListenWeChatData = new List<WXUser>();
            IsListenView = false;
            SetContactsView(NotListenWeChatData);                        
        }

        /// <summary>
        /// 已监控列表tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbTabWeChatListen_Click(object sender, EventArgs e)
        {
            CurrentSelectedWeChat = string.Empty;
            dgvWeChatList.ContextMenuStrip = menuStrip1;
            lbTabWeChat.BackColor = Color.Silver;
            lbTabWeChatListen.BackColor = Color.White;
            if (ListenWeChatData == null) ListenWeChatData = new List<WXUser>();
            IsListenView = true;
            SetContactsView(ListenWeChatData);
            SetDataContentView(wxMessageData);

            if (mainForm.editForm != null)
            {
                mainForm.editForm.isHide = true;
                mainForm.editForm.Hide();
            }

        }


        /// <summary>
        /// 关闭个人信息窗口
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void CloseMyInfoForm(object sender, MouseEventArgs e)
        {
            mainForm.CloseMyInfoForm(sender, e);
        }

        /// <summary>
        /// 一键添加监控
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void toolsAllListen_Click(object sender, EventArgs e)
        {
            NotListenWeChatData.ForEach(data =>
            {

                if (!ListenWeChatData.Exists(item => { return item.UserName == data.UserName; }))
                {
                    //将群添加到监控列表
                    ListenWeChatData.Add(new WXUser()
                    {
                        UserName = data.UserName,
                        NickName = data.NickName
                    });
                }
            });
            NotListenWeChatData.Clear();
            dgvWeChatList.Rows.Clear();

        }

        /// <summary>
        /// 一键移除监控
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void toolsClearListen_Click(object sender, EventArgs e)
        {
            ListenWeChatData.ForEach(data =>
            {
                if (!NotListenWeChatData.Exists(item => { return item.UserName == data.UserName; }))
                {
                    //将群添加到监控列表
                    NotListenWeChatData.Add(new WXUser()
                    {
                        UserName = data.UserName,
                        NickName = data.NickName
                    });
                }
            });
            ListenWeChatData.Clear();
            dgvWeChatList.Rows.Clear();
        }

        /// <summary>
        /// 清除页面所有数据
        /// </summary>
        public void ClearAllData()
        {
            wxMessageData.Clear();
            ListenWeChatData.Clear();
            NotListenWeChatData.Clear();
            dataContent.Rows.Clear();
            dgvWeChatList.Rows.Clear();
            CurrentSelectedWeChat = string.Empty;
            mainForm.editForm.Close();
            mainForm.editForm = null;
        }

    }
}
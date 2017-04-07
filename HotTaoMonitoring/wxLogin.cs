﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WwChatHttpCore.HTTP;
using WwChatHttpCore.Objects;

namespace HotTaoMonitoring
{
    public partial class wxLogin : Form
    {
        NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        #region 移动窗口
        /*
         * 首先将窗体的边框样式修改为None，让窗体没有标题栏
         * 实现这个效果使用了三个事件：鼠标按下、鼠标弹起、鼠标移动
         * 鼠标按下时更改变量isMouseDown标记窗体可以随鼠标的移动而移动
         * 鼠标移动时根据鼠标的移动量更改窗体的location属性，实现窗体移动
         * 鼠标弹起时更改变量isMouseDown标记窗体不可以随鼠标的移动而移动
         */
        private bool isMouseDown = false;
        private Point FormLocation;     //form的location
        private Point mouseOffset;      //鼠标的按下位置
        private void WinForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                FormLocation = this.Location;
                mouseOffset = Control.MousePosition;
            }
        }

        private void WinForm_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }
        private void WinForm_MouseMove(object sender, MouseEventArgs e)
        {
            int _x = 0;
            int _y = 0;
            if (isMouseDown)
            {
                Point pt = Control.MousePosition;
                _x = mouseOffset.X - pt.X;
                _y = mouseOffset.Y - pt.Y;

                this.Location = new Point(FormLocation.X - _x, FormLocation.Y - _y);
            }

        }
        #endregion


        private MainForm mainForm { get; set; }

        public bool isCloseWinForm = false;
        /// <summary>
        /// 判断是否中断二维码扫描登录
        /// </summary>
        public static bool loginClose = false;
        /// <summary>
        /// 是否检查扫描登录状态
        /// </summary>
        private static bool isLoginCheck = false;

        /// <summary>
        /// 当前登录微信用户
        /// </summary>
        private WXUser _me;

        /// <summary>
        /// 当前用户的所用联系人
        /// </summary>
        public List<WXUser> contact_all = new List<WXUser>();




        public wxLogin(MainForm form)
        {
            InitializeComponent();
            mainForm = form;
        }

        private void wxLogin_Load(object sender, EventArgs e)
        {
            isCloseWinForm = false;
            DoLogin();
        }

        /// <summary>
        /// 微信登录
        /// </summary>
        private void DoLogin()
        {
            picQRCode.Image = Properties.Resources.loading;
            picQRCode.SizeMode = PictureBoxSizeMode.CenterImage;
            lblTip.Text = "手机微信扫一扫以登录";
            lblTip.Width = 200;
            ((Action)(delegate ()
            {
                //异步加载二维码
                LoginService ls = new LoginService();
                //等待[判断手机扫面二维码结果]操作结束
                while (loginClose) { }
                Image qrcode = ls.GetQRCode();
                if (qrcode != null)
                {
                    this.BeginInvoke((Action)delegate ()
                    {
                        picQRCode.SizeMode = PictureBoxSizeMode.Zoom;
                        picQRCode.Image = qrcode;
                    });
                    object login_result = null;
                    Image headImg = null;
                    isLoginCheck = true;
                    while (isLoginCheck)  //循环判断手机扫面二维码结果
                    {
                        login_result = ls.LoginCheck();
                        if (login_result is Image) //已扫描 未登录
                        {
                            this.BeginInvoke((Action)delegate ()
                            {
                                lblTip.Text = "请点击手机上登录按钮";
                                picQRCode.SizeMode = PictureBoxSizeMode.CenterImage;  //显示头像
                                picQRCode.Image = login_result as Image;
                                headImg = login_result as Image;
                            });
                        }
                        if (login_result is string)  //已完成登录
                        {
                            //访问登录跳转URL
                            ls.GetSidUid(login_result as string);

                            ////打开主界面
                            //this.BeginInvoke((Action)delegate ()
                            //{
                            //    wxcontactsForm = new wxContacts(this, headImg);
                            //    wxcontactsForm.Show();
                            //    this.Hide();
                            //});
                            //isStartTask = true;
                            //if (historyForm != null)
                            //    historyForm.ShowStartButtonText("暂停计划");
                            //if (taskForm != null)
                            //    taskForm.ShowStartButtonText("暂停计划");


                            break;
                        }
                    }
                    loginClose = false;
                    if (isLoginCheck)
                    {
                        isLoginCheck = false;
                        DoMainLogic();
                    }
                }
            })).BeginInvoke(null, null);
        }

        /// <summary>
        /// 微信登录初始化及同步操作
        /// </summary>
        public void DoMainLogic()
        {
            //isStartTask = false;
            WXService wxs = new WXService();
            JObject init_result = wxs.WxInit();  //初始化
            contact_all.Clear();
            if (init_result != null)
            {
                InitCurrentUserData(init_result);
            }
            else return;
            JObject contact_result = wxs.GetContact(); //通讯录
            if (contact_result != null)
            {
                LoadMyContact(contact_result);
                //isStartTask = true;
            }

            string sync_flag = "";
            JObject sync_result;
            while (true)
            {
                if (isCloseWinForm) break;

                sync_flag = wxs.WxSyncCheck();  //同步检查
                if (sync_flag == null)
                {
                    continue;
                }
                //这里应该判断 sync_flag中selector的值
                else //有消息
                {
                    sync_result = wxs.WxSync();  //进行同步

                    //在此次进行判断自动回复或踢人操作
                    if (sync_result != null)
                    {

                        //判断是否掉线
                        if (sync_result["BaseResponse"]["Ret"].ToObject<int>() > 0)
                        {
                            isLoginCheck = false;
                            isCloseWinForm = true;

                            //if (historyForm != null)
                            //    historyForm.ShowStartButtonText("启动计划");
                            //if (taskForm != null)
                            //    taskForm.ShowStartButtonText("启动计划");

                            //if (wxcontactsForm != null)
                            //    CloseMyContact();

                            //hotForm.AlertTip("微信掉线，请重新授权登录");
                            break;
                        }
                        if (sync_result["AddMsgCount"] != null && sync_result["AddMsgCount"].ToString() != "0")
                        {
                            foreach (JObject m in sync_result["AddMsgList"])
                            {
                                SyncMsgHandle(wxs, m);
                            }
                        }
                    }
                }
                Thread.Sleep(2000);
            }
        }



        /// <summary>
        /// 初始化当前用户数据
        /// </summary>
        /// <param name="init_result">The init_result.</param>
        private void InitCurrentUserData(JObject init_result)
        {
            _me = new WXUser();
            _me.UserName = init_result["User"]["UserName"].ToString();
            _me.City = "";
            _me.HeadImgUrl = init_result["User"]["HeadImgUrl"].ToString();
            _me.NickName = init_result["User"]["NickName"].ToString();
            _me.Province = "";
            _me.PYQuanPin = init_result["User"]["PYQuanPin"].ToString();
            _me.RemarkName = init_result["User"]["RemarkName"].ToString();
            _me.RemarkPYQuanPin = init_result["User"]["RemarkPYQuanPin"].ToString();
            _me.Sex = init_result["User"]["Sex"].ToString();
            _me.Signature = init_result["User"]["Signature"].ToString();

            foreach (JObject contact in init_result["ContactList"])  //部分好友名单
            {
                WXUser user = new WXUser();
                user.UserName = contact["UserName"].ToString();
                user.City = contact["City"].ToString();
                user.HeadImgUrl = contact["HeadImgUrl"].ToString();
                user.NickName = contact["NickName"].ToString();
                user.Province = contact["Province"].ToString();
                user.PYQuanPin = contact["PYQuanPin"].ToString();
                user.RemarkName = contact["RemarkName"].ToString();
                user.RemarkPYQuanPin = contact["RemarkPYQuanPin"].ToString();
                user.Sex = contact["Sex"].ToString();
                user.Signature = contact["Signature"].ToString();
                user.IsOwner = Convert.ToInt32(contact["IsOwner"].ToString());
                contact_all.Add(user);
            }

        }


        /// <summary>
        /// 加载我的通讯录
        /// </summary>
        /// <param name="contact_result">The contact_result.</param>
        private void LoadMyContact(JObject contact_result)
        {
            foreach (JObject contact in contact_result["MemberList"])  //完整好友名单
            {
                WXUser user = new WXUser();
                user.UserName = contact["UserName"].ToString();
                user.City = contact["City"].ToString();
                user.HeadImgUrl = contact["HeadImgUrl"].ToString();
                user.NickName = contact["NickName"].ToString();
                user.Province = contact["Province"].ToString();
                user.PYQuanPin = contact["PYQuanPin"].ToString();
                user.RemarkName = contact["RemarkName"].ToString();
                user.RemarkPYQuanPin = contact["RemarkPYQuanPin"].ToString();
                user.Sex = contact["Sex"].ToString();
                user.Signature = contact["Signature"].ToString();
                user.IsOwner = Convert.ToInt32(contact["IsOwner"].ToString());
                //判断是否存在重复
                if (!contact_all.Exists((item) => { return item.UserName == user.UserName; }))
                {
                    contact_all.Add(user);
                    //if (wxcontactsForm != null)
                    //    wxcontactsForm.SetContactsView(user);
                }
                else
                {
                    contact_all.ForEach(item =>
                    {
                        if (item.UserName == user.UserName)
                        {
                            item.NickName = user.NickName;
                            //if (wxcontactsForm != null)
                            //    wxcontactsForm.SetContactsView(user);
                        }
                    });
                }
            }
        }



        /// <summary>
        /// 消息同步操作
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="m">The m.</param>
        private void SyncMsgHandle(WXService service, JObject m)
        {
            //用户退出或任务停止时
            if (MyUserInfo.currentUserId == 0) return;

            //自己发消息时，from为自己的id，否则为群id
            string from = m["FromUserName"].ToString();
            //不是自己发消息时，to为自己的id,否则为群id
            string to = m["ToUserName"].ToString();

            //判断发送方不是本人,且目标是群聊
            if (_me.UserName == to && from.Contains("@@"))
            {
                string content = m["Content"].ToString();
                int msgtype = 0;
                int.TryParse(m["MsgType"].ToString(), out msgtype);
                //获取当前群信息
                WXUser user = contact_all.Find((WXUser obj) => { return obj.UserName == from; });
                if (user == null) return;

                string nickName = string.Empty, msgSendUser = string.Empty, messageContent = string.Empty;
                switch (msgtype)
                {
                    case (int)WxMsgType.文本消息:
                        //获取发送者标识id;
                        msgSendUser = content.Split(':')[0];
                        messageContent = content.Split(':')[1];
                        //获取当前发送方的昵称
                        nickName = GetCurrentMessageUserNickName(service, msgSendUser, user.UserName);

                        //SendLog(user.ShowName + ":[" + nickName + "]发了一条消息:" + messageContent);

                        //自动回复
                        //AutoReplyChatroom(service, user.ShowName, from, messageContent, nickName);
                        //自动踢人
                        //RemoveChatroom(service, user, msgSendUser, from, nickName, msgtype, messageContent);
                        break;
                    case (int)WxMsgType.图片消息:
                    case (int)WxMsgType.分享链接:
                    case (int)WxMsgType.共享名片:
                        //获取发送者标识id;
                        msgSendUser = content.Split(':')[0];
                        //获取当前发送方的昵称
                        nickName = GetCurrentMessageUserNickName(service, msgSendUser, user.UserName);



                        //if (msgtype == (int)WxMsgType.图片消息)
                        //    SendLog(user.ShowName + ":[" + nickName + "]发了一张图片");
                        //else if (msgtype == (int)WxMsgType.分享链接)
                        //    SendLog(user.ShowName + ":[" + nickName + "]分享了一条链接");
                        //else if (msgtype == (int)WxMsgType.共享名片)
                        //    SendLog(user.ShowName + ":[" + nickName + "]共享了一张名片");
                        //自动踢人
                        // RemoveChatroom(service, user, msgSendUser, from, nickName, msgtype, messageContent);
                        break;
                    case (int)WxMsgType.系统消息:

                        break;
                    default:
                        break;
                }
            }
        }


        /// <summary>
        /// 获取当前发消息人的昵称
        /// </summary>
        /// <param name="CurrentMsgSendUserId">The current MSG send user identifier.</param>
        /// <param name="memberlist_result">The memberlist_result.</param>
        /// <returns>System.String.</returns>
        private string GetCurrentMessageUserNickName(WXService service, string CurrentMsgSendUserId, string groupUserName)
        {
            //根据群用户ID，获取用户信息
            try
            {
                JObject contact_result = service.GetChatRoomContactList(groupUserName);
                if (contact_result == null) return null;
                var ContactList = contact_result["ContactList"];
                if (ContactList == null || ContactList.Count() == 0) return null;

                var memberList = ContactList[0]["MemberList"];
                if (memberList == null || memberList.Count() == 0) return null;
                string nickName = string.Empty;
                foreach (var item in memberList)
                {
                    if (item["UserName"].ToString() == CurrentMsgSendUserId)
                    {
                        nickName = item["NickName"].ToString();
                        break;
                    }
                }
                return nickName;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return string.Empty;
            }
        }

    }
}

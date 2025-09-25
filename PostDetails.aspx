<%@ Page Title="Chi tiết" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="PostDetails.aspx.cs" Inherits="BlogPlatform.PostDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" Runat="Server">Chi tiết bài viết</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Panel runat="server" ID="pnlPost">
        <h2><asp:Literal runat="server" ID="litTitle" /></h2>
        <div class="meta">Tác giả: <asp:Literal runat="server" ID="litAuthor" /> | Ngày: <asp:Literal runat="server" ID="litDate" /></div>
        <div class="post-content"><asp:Literal runat="server" ID="litContent" /></div>

        <hr />
        <h3>Bình luận</h3>
        <asp:Repeater ID="rptComments" runat="server">
            <ItemTemplate>
                <div class="comment">
                    <div class="c-author"><%# Eval("Username") %> - <span class="c-date"><%# Eval("CreatedAt") %></span></div>
                    <div class="c-body"><%# Eval("Content") %></div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <asp:Panel runat="server" ID="pnlAddComment" Visible="false">
            <h4>Thêm bình luận</h4>
            <asp:Label ID="lblCommentMsg" runat="server" CssClass="message" />
            <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Rows="4"></asp:TextBox>
            <br />
            <asp:Button ID="btnSubmitComment" runat="server" Text="Gửi bình luận" OnClick="btnSubmitComment_Click" />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlLoginAsk" Visible="false">
            <p>Bạn cần <a href="Login.aspx">đăng nhập</a> để bình luận.</p>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
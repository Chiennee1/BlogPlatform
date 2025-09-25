<%@ Page Title="Đăng nhập" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BlogPlatform.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" Runat="Server">Đăng nhập</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="auth-box">
        <h2>Đăng nhập</h2>
        <asp:Label ID="lblMessage" runat="server" CssClass="message" />
        <div class="form-row">
            <label>Username</label>
            <asp:TextBox ID="txtUsername" runat="server" />
        </div>
        <div class="form-row">
            <label>Password</label>
            <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" />
        </div>
        <div class="form-row">
            <asp:Button ID="btnLogin" runat="server" Text="Đăng nhập" OnClick="btnLogin_Click" />
        </div>
    </div>
</asp:Content>
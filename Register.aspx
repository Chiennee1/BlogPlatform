<%@ Page Title="Đăng ký" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="BlogPlatform.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" Runat="Server">Đăng ký</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="auth-box">
        <h2>Đăng ký</h2>
        <asp:Label ID="lblMsg" runat="server" CssClass="message" />
        <div class="form-row">
            <label>Username</label>
            <asp:TextBox ID="txtUser" runat="server" />
        </div>
        <div class="form-row">
            <label>Email</label>
            <asp:TextBox ID="txtEmail" runat="server" />
        </div>
        <div class="form-row">
            <label>Password</label>
            <asp:TextBox ID="txtPass" runat="server" TextMode="Password" />
        </div>
        <div class="form-row">
            <label>Full name</label>
            <asp:TextBox ID="txtFullName" runat="server" />
        </div>
        <div class="form-row">
            <asp:Button ID="btnRegister" runat="server" Text="Đăng ký" OnClick="btnRegister_Click" />
        </div>
    </div>
</asp:Content>
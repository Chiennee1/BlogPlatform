<%@ Page Title="Viết bài" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CreatePost.aspx.cs" Inherits="BlogPlatform.CreatePost" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" Runat="Server">Viết bài</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="post-editor">
        <h2>Viết bài mới</h2>
        <asp:Label ID="lblInfo" runat="server" CssClass="message" />
        <div class="form-row">
            <label>Tiêu đề</label>
            <asp:TextBox ID="txtTitle" runat="server" />
        </div>
        <div class="form-row">
            <label>Chuyên mục</label>
            <asp:DropDownList ID="ddlCategory" runat="server"></asp:DropDownList>
        </div>
        <div class="form-row">
            <label>Nội dung (HTML được chấp nhận)</label>
            <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Rows="12"></asp:TextBox>
        </div>
        <div class="form-row">
            <asp:Button ID="btnPublish" runat="server" Text="Xuất bản" OnClick="btnPublish_Click" />
            &nbsp;
            <asp:Button ID="btnSaveDraft" runat="server" Text="Lưu nháp" OnClick="btnSaveDraft_Click" />
        </div>
    </div>
</asp:Content>
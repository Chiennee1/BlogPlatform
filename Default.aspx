<%@ Page Title="Trang chủ" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BlogPlatform.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" Runat="Server">Trang chủ</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="home">
        <div class="sidebar">
            <h3>Chuyên mục</h3>
            <asp:Repeater ID="rptCategories" runat="server">
                <ItemTemplate>
                    <div class="category-item"><a href="Default.aspx?cat=<%# Eval("CategoryID") %>"><%# Eval("CategoryName") %></a></div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div class="main-list">
            <h2>Bài viết</h2>
            <asp:Repeater ID="rptPosts" runat="server">
                <HeaderTemplate><div class="posts-list"></HeaderTemplate>
                <ItemTemplate>
                    <div class="post">
                        <h3><a href="PostDetails.aspx?id=<%# Eval("PostID") %>"><%# Eval("Title") %></a></h3>
                        <div class="meta">Tác giả: <%# Eval("Username") %> | Ngày: <%# Eval("CreatedAt") %></div>
                        <div class="excerpt"><%# Eval("Excerpt") %></div>
                    </div>
                </ItemTemplate>
                <FooterTemplate></div></FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
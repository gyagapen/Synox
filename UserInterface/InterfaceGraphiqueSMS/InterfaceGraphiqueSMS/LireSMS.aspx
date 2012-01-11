<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="LireSMS.aspx.cs" Inherits="InterfaceGraphiqueSMS.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
<h1>Lecture des SMS</h1>
    <asp:ListBox ID="ListMessages" runat="server" 
        onselectedindexchanged="ListMessages_SelectedIndexChanged"></asp:ListBox><br />
    Emetteur :<asp:TextBox ID="tbEmetteur" runat="server"></asp:TextBox><br />
    Message :<asp:TextBox ID="tbMessage" runat="server"></asp:TextBox>
</asp:Content>

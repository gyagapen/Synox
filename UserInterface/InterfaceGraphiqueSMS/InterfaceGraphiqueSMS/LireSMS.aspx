<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true" CodeBehind="LireSMS.aspx.cs" Inherits="InterfaceGraphiqueSMS.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
<h1>Lecture des SMS</h1>
    <asp:ListBox ID="ListMessages" runat="server" 
        CssClass="style1" Width="190px" AutoPostBack="True" 
        onselectedindexchanged="ListMessages_SelectedIndexChanged"></asp:ListBox>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            Emetteur :<asp:TextBox ID="tbEmetteur" runat="server"></asp:TextBox><br />
            Message :<asp:TextBox ID="tbMessage" runat="server"></asp:TextBox>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ListMessages" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TestWebApplication1._Default"
    MasterPageFile="~/MasterPage.Master" %>

<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <center>
            
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label><br />
            <asp:TextBox ID="TextBox1" TextMode=MultiLine runat="server" Height="128px" Width="455px"></asp:TextBox><br />
            <asp:Button ID="buttonTest" runat="server" Text="Valider" OnClick="ActivationTest" />
            
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

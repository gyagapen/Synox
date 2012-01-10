<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="InterfaceGraphiqueSMS._Default" MasterPageFile="~/Base.Master" %>


<asp:Content ContentPlaceHolderID=Main  runat=server>

    <!-- Formulaire d envoi -->
   

    <form method=get>
        Num&eacute;ro de t&eacute;l&eacute;phone :
        <asp:TextBox runat=server ID="numDestinataire"></asp:TextBox><br />
        Message à envoyer : 
        <hr />
        <asp:TextBox TextMode=MultiLine ID="contenuSMS" Height="128px" Width="455px" runat="server"></asp:TextBox><br />
        <asp:Button Text="Valider" onclick="EcrireSMS" runat="server"/>
    </form>

</asp:Content>

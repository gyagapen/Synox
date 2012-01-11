<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="InterfaceGraphiqueSMS._Default" MasterPageFile="~/Base.Master" %>


<asp:Content ContentPlaceHolderID=Main  runat=server>

    <!-- Formulaire d envoi -->
   

    <form method=get>

        <h2 align="center">Envoyer des SMS</h2>
        <hr />
        Num&eacute;ro de t&eacute;l&eacute;phone :
        <asp:TextBox runat=server ID="numDestinataire"></asp:TextBox><br /><br />
        Message à envoyer : <br />
        
        <asp:TextBox TextMode=MultiLine ID="contenuSMS" Height="128px" Width="455px" runat="server"></asp:TextBox><br />
        <asp:Button Text="Valider" onclick="EcrireSMS" runat="server" />
    </form>

</asp:Content>

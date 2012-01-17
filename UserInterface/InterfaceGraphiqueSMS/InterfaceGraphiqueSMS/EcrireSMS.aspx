<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="InterfaceGraphiqueSMS._Default" MasterPageFile="~/Base.Master" %>


<asp:Content ContentPlaceHolderID=Main  runat=server>

    <!-- Formulaire d envoi -->
   

    <form method=get>

        <h2 align="center">Envoyer des SMS</h2>
        <hr />
        Mode :
        <asp:DropDownList ID="ListeMode" runat="server" AutoPostBack="True" 
            onselectedindexchanged="ListeMode_SelectedIndexChanged">
            <asp:ListItem>Texte</asp:ListItem>
            <asp:ListItem>PDU</asp:ListItem>
        </asp:DropDownList>
        <br />
        <br />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                Num&eacute;ro du destinataire :
                <asp:TextBox runat=server ID="numDestinataire"></asp:TextBox><br />
                Encodage :
                <asp:DropDownList ID="DropDownEncodage" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="DropDownEncodage_SelectedIndexChanged"></asp:DropDownList>&nbsp;&nbsp;&nbsp; 
                <asp:CheckBox ID="CheckBoxAccuse" runat="server" Text="Accusé de réception" /><br />
                Date de validité :
                <asp:TextBox ID="tbJours" runat="server" CssClass="style2" Width="24px" 
                    AutoPostBack="True" MaxLength="3" ontextchanged="tbJours_TextChanged"></asp:TextBox>
                jours
                <asp:TextBox ID="tbHeures" runat="server" CssClass="style3" Width="24px" 
                    AutoPostBack="True" MaxLength="2" ontextchanged="tbHeures_TextChanged"></asp:TextBox>
                heures
                <asp:TextBox ID="tbMinutes" runat="server" CssClass="style4" Width="24px" 
                    AutoPostBack="True" MaxLength="2" ontextchanged="tbMinutes_TextChanged"></asp:TextBox>
                minutes<br />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ListeMode" 
                    EventName="SelectedIndexChanged">
                </asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <br />
        
        Message à envoyer : <br />
    
        
        <asp:TextBox TextMode=MultiLine ID="contenuSMS" Height="128px" Width="455px" 
            runat="server" AutoPostBack="True" MaxLength="5"></asp:TextBox><br />
        <asp:Button Text="Valider" onclick="EcrireSMS" runat="server" 
            CssClass="style1" />
    </form>

    <div id="dialog" style="display: none">Message OK !!</div>

</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .style1
        {}
        .style2
        {}
        .style3
        {}
        .style4
        {}
    </style>
</asp:Content>


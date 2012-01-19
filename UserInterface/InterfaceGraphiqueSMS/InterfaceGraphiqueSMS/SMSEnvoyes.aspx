<%@ Page Title="" Language="C#" MasterPageFile="~/Base.Master" AutoEventWireup="true"
    CodeBehind="SMSEnvoyes.aspx.cs" Inherits="InterfaceGraphiqueSMS.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- CSS -->
    <link href="css/SMSEnvoyes.css" rel="Stylesheet" type="text/css" />
        <!-- IScroll -->
    <script type="text/javascript" src="Scripts/iscroll.js"></script>
    <!-- JAVASCRIPT -->
    <script type="text/javascript">

        //savoir si c'est le navigateur est chrome ou pas
        var is_chrome = navigator.userAgent.toLowerCase().indexOf('chrome')


       /**************ISCROLL*****************/
        var myScroll;
        var a = 0;
        function loaded() {
            

            // Please note that the following is the only line needed by iScroll to work. Everything else here is to make this demo fancier.
            if (is_chrome != -1) {
                myScroll = new iScroll("<%= TableSMSEnvoyes.ClientID %>", { desktopCompatibility: true });
            }
        }

        // Prevent the whole screen to scroll when dragging elements outside of the scroller (ie:header/footer).
        // If you want to use iScroll in a portion of the screen and still be able to use the native scrolling, do *not* preventDefault on touchmove.
        document.addEventListener('touchmove', function (e) { e.preventDefault(); }, false);

        // Load iScroll when DOM content is ready.
        document.addEventListener('DOMContentLoaded', loaded, false);

        /**************FIN ISCROLL*****************/



        function selectTableSMS(idSMS) {


            InterfaceGraphiqueSMS.WebForm1.saveIdSMS(idSMS);

            $("#<%= buttonCache.ClientID %>").click();

        }


      
    </script>
    <!-- FIN JAVASCRIPT -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
    <h1>
        SMS envoyés</h1>
    <!-- Tableau des SMS envoyes -->
    <div id="scroller">
    <asp:Panel ID="Panel1" runat="server" Height="400px" ScrollBars=Auto>
        <asp:Table ID="TableSMSEnvoyes" runat="server">
        </asp:Table>
    </asp:Panel>
    </div>
    <!-- Fin tableau -->
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            Emetteur :
            <asp:TextBox ID="tbEmetteur" runat="server" ReadOnly="True"></asp:TextBox>
            Destinataire :
            <asp:TextBox ID="tbDestinataire" runat="server" ReadOnly="True"></asp:TextBox>
            <br />
            Statut :
            <asp:TextBox ID="tbStatut" runat="server" ReadOnly="True"></asp:TextBox>
            &nbsp;Encodage :
            <asp:TextBox ID="tbEncodage" runat="server" ReadOnly="True"></asp:TextBox>
            <br />
            Date demande :
            <asp:TextBox ID="tbDateDemande" runat="server"></asp:TextBox>
            &nbsp;Date envoi :
            <asp:TextBox ID="tbDateEnvoi" runat="server"></asp:TextBox>
            <br />
            Message :
            <asp:TextBox ID="tbMessage" runat="server" CssClass="style2" Height="89px" ReadOnly="True"
                Width="330px" TextMode="MultiLine"></asp:TextBox>
            <br />
            PDU :
            <asp:TextBox ID="tbPDU" runat="server" CssClass="style2" Height="89px" ReadOnly="True"
                Width="330px" TextMode="MultiLine"></asp:TextBox>
            <div style="display: none">
                <asp:Button ID="buttonCache" ClientIDMode="Predictable" ClientID="test" runat="server"
                    Text="Button" OnClick="buttonCache_clicked" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>

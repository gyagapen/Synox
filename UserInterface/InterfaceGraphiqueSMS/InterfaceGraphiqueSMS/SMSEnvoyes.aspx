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
                //myScroll = new iScroll("<%= TableSMSEnvoyes.ClientID %>", { desktopCompatibility: true });
                myScroll = new iScroll("divTable", { desktopCompatibility: true });
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

            //ouverture de la popup
            $('#tabInfo').modal({ minHeight: 450 });


        }


    </script>
    <!-- FIN JAVASCRIPT -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
    <h1>
        SMS envoyés</h1>
    <!-- Tableau des SMS envoyes -->
    <asp:Panel ID="Panel1" runat="server"  Height="400px" ScrollBars="Auto">
        <div id="divTable">
            <asp:Table ID="TableSMSEnvoyes"  runat="server" CssClass="tableauSMS">
            </asp:Table>
        </div>
    </asp:Panel>
    <!-- Fin tableau -->
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <div id="div_panel" style="display: none">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table align="center" id="tabInfo">
                    <tr>
                        <td>
                            Emetteur :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEmetteur" runat="server" ReadOnly="True"></asp:TextBox><br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Destinataire :
                        </td>
                        <td>
                            <asp:TextBox ID="tbDestinataire" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Statut :
                        </td>
                        <td>
                            <asp:TextBox ID="tbStatut" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;Encodage :
                        </td>
                        <td>
                            <asp:TextBox ID="tbEncodage" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Date demande :
                        </td>
                        <td>
                            <asp:TextBox ID="tbDateDemande" runat="server" ReadOnly="True"></asp:TextBox><br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;Date envoi :
                        </td>
                        <td>
                            <asp:TextBox ID="tbDateEnvoi" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <center>
                                Message :
                                <br />
                                <asp:TextBox ID="tbMessage" runat="server" CssClass="style2" Height="89px" ReadOnly="True"
                                    Width="330px" TextMode="MultiLine"></asp:TextBox>
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <center>
                                PDU :
                                <br />
                                <asp:TextBox ID="tbPDU" runat="server" CssClass="style2" Height="89px" ReadOnly="True"
                                    Width="330px" TextMode="MultiLine"></asp:TextBox>
                            </center>
                        </td>
                    </tr>
                </table>
                <div style="display: none">
                    <asp:Button ID="buttonCache" ClientIDMode="Predictable" runat="server" Text="Button"
                        OnClick="buttonCache_clicked" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <br />
</asp:Content>

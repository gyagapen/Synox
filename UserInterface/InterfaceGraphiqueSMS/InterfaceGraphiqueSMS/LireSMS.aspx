<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Base.Master" CodeBehind="LireSMS.aspx.cs" Inherits="InterfaceGraphiqueSMS.LireSMS" %>

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
                //myScroll = new iScroll("divTable", { desktopCompatibility: true });
            }
        }

        // Prevent the whole screen to scroll when dragging elements outside of the scroller (ie:header/footer).
        // If you want to use iScroll in a portion of the screen and still be able to use the native scrolling, do *not* preventDefault on touchmove.
        document.addEventListener('touchmove', function (e) { e.preventDefault(); }, false);

        // Load iScroll when DOM content is ready.
        document.addEventListener('DOMContentLoaded', loaded, false);

        /**************FIN ISCROLL*****************/



        function selectTableSMS(idSMS) {

            //ouverture de la popup
            InterfaceGraphiqueSMS.WebForm1.saveIdSMS(idSMS);

            $("#<%= buttonCache.ClientID %>").click();
            $("#<%= UpdatePanel1.ClientID %>").modal({ minHeight: 450 });


        }



    </script>
    <!-- FIN JAVASCRIPT -->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <h1>
        SMS Recus</h1>
    <!-- Tableau des SMS envoyes -->
       <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
    <div id="divTable">
   

 <asp:Panel ID="Panel1" runat="server" Height="400px" ScrollBars="Auto">
  
                    <asp:Table ID="TableSMSEnvoyes" runat="server" CssClass="tableauSMS">
                    </asp:Table>
                 
                    </asp:Panel>
                    
                 
    </div>
       </ContentTemplate>
        </asp:UpdatePanel>   


    
    
    <!-- Fin tableau -->
    
    <div id="divPanel1" style="display: none">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <form>
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
                    
                        <td>
                            Date reception :
                        </td>
                        <td>
                            <asp:TextBox ID="tbDateReception" runat="server" ReadOnly="True"></asp:TextBox><br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Date Lecture :
                        </td>
                        <td>
                            <asp:TextBox ID="tbDateLecture" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Accuse demand&eacute; :
                        </td>
                        <td>
                            <asp:TextBox ID="tbAccuse" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
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
                    
                </table>
                </form>
                <div style="display: none">
                    <asp:Button ID="buttonCache" ClientIDMode="Predictable" runat="server" Text="Button"
                        OnClick="buttonCache_clicked" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Timer ID="TimerRefresh" runat="server" OnTick="rafraichirPage">
        </asp:Timer>
    </div>
    <br />




</asp:Content>
